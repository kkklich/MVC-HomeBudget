using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplicationHomeBudget.Models;

namespace WebApplicationHomeBudget.Controllers
{
    public class walletsController : Controller
    {
        private HomeBudget3Entities db = new HomeBudget3Entities();
        // private HomeBudget2Entities db = new HomeBudget2Entities();

        // GET: wallets 
        public ActionResult Index()
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            string idClient = Session["id_client"].ToString();

            string query = "  select w.id_wallet AS id_wallet,w.Name_wallet AS Name_wallet , cw.id_Client_wallet  AS id_Client_wallet from Client_wallet cw join wallet w on cw.id_wallet = w.id_wallet where cw.id_client = " + idClient + " group by w.id_wallet,w.Name_wallet ,cw.id_Client_wallet  order by 2";

            //  var data = db.Database.SqlQuery<wallet>(query);
            IEnumerable<walletClass> walletData = db.Database.SqlQuery<walletClass>(query);

            string query1 = "select * from Client_wallet cw join wallet w on w.id_wallet = cw.id_wallet where cw.id_client = " + idClient + "";
            IEnumerable<walletClass> walletALL = db.Database.SqlQuery<walletClass>(query);


            dynamic Mymodel = new ExpandoObject();
            Mymodel.walletData = walletData.ToList();






            return View(walletData.ToList());
        }

        // GET: wallets/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            wallet wallet = db.wallet.Find(id);

            if (wallet == null)
            {
                return HttpNotFound();
            }
            ViewBag.NameWallet = wallet.Name_wallet;
            int id_client;
            if (Session["id_client"] != null)
            {
                id_client = int.Parse(Session["id_client"].ToString());
                //Transact transact = db.Transact.Find(id);
                var transact = db.Transact.Include(t => t.client).Include(t => t.Client_subcategories).Include(t => t.Client_wallet);

                var linqResult = from x in transact
                                 where x.id_client == id_client & x.Client_wallet.id_wallet == id
                                 orderby x.IfIncome descending
                                 select x;

                ViewBag.id_wallet = wallet.id_wallet;

                return View(linqResult);
            }

            return View("Index");
        }

        // GET: wallets/Create
        public ActionResult Create()
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            return View();
        }

        // POST: wallets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_wallet,Name_wallet")] wallet wallet)
        {
            if (ModelState.IsValid)
            {


                db.wallet.Add(wallet);
                db.SaveChanges();

                Client_wallet client_Wallet1 = new Client_wallet();

                int id_client = int.Parse(Session["id_client"].ToString());

                client_Wallet1.id_client = id_client;
                client_Wallet1.id_wallet = wallet.id_wallet;

                db.Client_wallet.Add(client_Wallet1);
                db.SaveChanges();



                return RedirectToAction("Index");
            }

            return View(wallet);
        }

        // GET: wallets/Edit/5
        public ActionResult Edit(int? id, int? id_Client_wall)
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wallet wallet = db.wallet.Find(id);

            if (wallet == null)
            {
                return HttpNotFound();
            }
            return View(wallet);
        }

        // POST: wallets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_wallet,Name_wallet")] wallet wallet1, string id_Client_wall)
        {
            if (ModelState.IsValid)
            {

                if (wallet1.id_wallet > 3)
                {
                    db.Entry(wallet1).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {

                    if (id_Client_wall != null)
                    {
                        int? id_client = int.Parse(Session["id_client"].ToString());
                        int id_CLientW = int.Parse(id_Client_wall.ToString());

                        db.wallet.Add(wallet1);
                        db.SaveChanges();


                        Client_wallet client_Wallet1 = db.Client_wallet.Find(id_CLientW);

                        client_Wallet1.id_wallet = wallet1.id_wallet;

                        db.Entry(client_Wallet1).State = EntityState.Modified;

                        db.SaveChanges();


                    }


                    return RedirectToAction("Index");


                }
            }
            return View(wallet1);
        }

        // GET: wallets/Delete/5
        public ActionResult Delete(int? id, int? id_Client_wall,string info)
        {

            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wallet wallet = db.wallet.Find(id);
            ViewBag.id_Client_wall = id_Client_wall;

            ViewBag.info = info;
            if (wallet == null)
            {
                return HttpNotFound();
            }
            return View(wallet);
        }

        // POST: wallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id, string id_Client_wall)
        {


            try
            {
                

                if (id_Client_wall != null)
                {
                    int id_c_w = int.Parse(id_Client_wall);

                    Client_wallet client_Wallet1 = db.Client_wallet.Find(id_c_w);
                    db.Client_wallet.Remove(client_Wallet1);
                    db.SaveChanges();
                    // return RedirectToAction("Index");
                }
                ViewBag.info = id;
                if (id > 3)
                {
                    wallet wallet = db.wallet.Find(id);
                    db.wallet.Remove(wallet);
                    db.SaveChanges();


                    return RedirectToAction("Index");
                }



                return RedirectToAction("Index");
            }catch(Exception )
            {
                string info1 = "Nie można usunąć danego portfela, ze względu na  to, iż są na nim środki pieniężne";
                return RedirectToAction("delete", "wallets", new { id = id, id_Client_wall = id_Client_wall,info=info1 });
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
