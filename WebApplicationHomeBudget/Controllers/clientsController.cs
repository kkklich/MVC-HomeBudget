using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplicationHomeBudget.Models;

namespace WebApplicationHomeBudget.Controllers
{
    public class clientsController : Controller
    {
        private HomeBudget3Entities db = new HomeBudget3Entities();
        // private HomeBudget2Entities db = new HomeBudget2Entities();

        // GET: clients
        public ActionResult Index(int? id)
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var client = db.client;

            client client1 = db.client.Find(id);

            var linqClient = from c in client
                             where c.id_client == id
                             select c;

            return View(linqClient);
        }

        // GET: clients/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //    return RedirectToAction("Index", "Home");
            //}

            if (Session["id_client"] != null)
            {
                int? id_client = int.Parse(Session["id_client"].ToString());


                client client = db.client.Find(id_client);
                if (client == null)
                {
                    return HttpNotFound();
                }
                return View(client);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: clients/Create
        public ActionResult Create()
        {

            return View();
        }


        //var client2 = db.client.Include(c => c.register);
        // POST: clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id_client,firstname,surname,email,Id_login,Login_Name_Unique,Password_text ")] client client )
        public ActionResult Create(string firstname, string surename, string email, string Login, string Password, string PasswordRepeat)
        {

            var client = new client { firstname = firstname, surname = surename, email = email, Login_Name_Unique = Login, Password_text = Password };

            if (ModelState.IsValid)
            {
                db.client.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // ViewBag.Id_login = new SelectList(db.register, "id_login", "Login_Name", client.Id_login);
            return View(client);
        }

        // GET: clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            client client = db.client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            return View(client);
        }

        // POST: clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_client,firstname,surname,email,Login_Name_Unique,Password_text")] client client1, string RePassword)
        {

            db.client.Attach(client1);

            if (client1.Password_text != null)
            {

                if (client1.Password_text == RePassword)
                {
                    client1.Password_text = MainClass.EncMD5(client1.Password_text);
                    db.Entry(client1).Property(x => x.Password_text).IsModified = true;
                }
                else
                {
                    ViewBag.errorPass = "Różne hasła";
                    return View(client1);
                }

            }




            if (client1.firstname != null)
                db.Entry(client1).Property(x => x.firstname).IsModified = true;

            if (client1.surname != null)
                db.Entry(client1).Property(x => x.surname).IsModified = true;

            if (client1.email != null)
                db.Entry(client1).Property(x => x.email).IsModified = true;



            if (client1.Login_Name_Unique != null)
            {
                bool IfSameLogin;
                IfSameLogin = MainClass.SameLogin(client1.Login_Name_Unique, client1.id_client);

                if (IfSameLogin == false)
                {
                    db.Entry(client1).Property(x => x.Login_Name_Unique).IsModified = true;

                }
                else
                {
                    ViewBag.errorPass = "Login jest już zajęty";
                    return View(client1);
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
            //return View(client1);
        }

        // GET: clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            client client = db.client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            int? id_client = int.Parse(Session["id_client"].ToString());




            foreach (var x in db.Transact.Where(x => x.id_client == id_client))
            {
                db.Transact.Remove(x);
            }


            foreach (var cw in db.Client_wallet.Where(cw => cw.id_client == id_client))
            {
                db.Client_wallet.Remove(cw);
            }


            foreach (var cs in db.Client_subcategories.Where(cs => cs.id_client == id_client))
            {
                db.Client_subcategories.Remove(cs);
            }


            db.SaveChanges();


            //var linqClient = from x in db.Client_subcategories
            //                 where x.id_client == id_client
            //                 select x;

            //foreach(var item in linqClient)
            //{
            //    Client_subcategories clientSub = db.Client_subcategories.Find(item.id_Client_subcategories);
            //    db.Client_subcategories.Remove(clientSub);
            //    db.SaveChanges();

            //}


            client client = db.client.Find(id);
            db.client.Remove(client);
            db.SaveChanges();
            return RedirectToAction("DeletedClient", "Clients", new { name = client.Login_Name_Unique });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult DeletedClient(string name)
        {
            ViewBag.clientName = name;

            return View();
        }

    }
}
