using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplicationHomeBudget.Models;

namespace WebApplicationHomeBudget.Controllers
{
    public class TransactsController : Controller
    {
        private HomeBudget3Entities db = new HomeBudget3Entities();

        HomeController home1 = new HomeController();
        CultureInfo cultureInfo = CultureInfo.GetCultureInfo("pl-PL");


        public ActionResult Index(string ifincome)
        {


            try
            {


                var transact = db.Transact.Include(t => t.client).Include(t => t.Client_subcategories).Include(t => t.Client_wallet);



                if (Session["id_client"] != null)
                {

                    int? id_client = int.Parse(Session["id_client"].ToString());


                    if (ifincome == "Transact")
                    {

                        var linqResult = from x in transact
                                         where x.id_client == id_client
                                         select x;




                        ViewBag.title = "Transakcja";
                        return View(linqResult.ToList());

                    }



                    if (ifincome == "income")
                    {
                        var linqResult = from x in transact
                                         where (x.IfIncome == true && x.id_client == id_client)
                                         select x;

                        var linqClientIncome = from x in transact
                                               where x.id_client == id_client & x.IfIncome == true
                                               select x;
                        var income = linqClientIncome.AsEnumerable().Sum(x => x.Amount);



                        string q = String.Format(cultureInfo, "{0:C}", income);
                        ViewBag.income = q;

                        ViewBag.info = "Suma dochodów:";
                        ViewBag.title = "Dochody";
                        return View(linqResult.ToList());
                    }

                    if (ifincome == "expenses")
                    {
                        var linqResult = from x in transact
                                         where (x.IfIncome == false && x.id_client == id_client)
                                         select x;

                        var linqClientIncome = from x in transact
                                               where x.id_client == id_client & x.IfIncome == false
                                               select x;
                        var income = linqClientIncome.AsEnumerable().Sum(x => x.Amount);
                        string q = String.Format(cultureInfo, "{0:C}", income);
                        ViewBag.income = q;
                        ViewBag.info = "Suma wydatków:";

                        ViewBag.title = "Wydatki";
                        return View(linqResult.ToList());
                    }


                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

        }





        // GET: Transacts/Details/5
        public ActionResult Details(int? id)
        {
            try
            {

                if (Session["id_client"] == null)
                    RedirectToAction("Log", "Home");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Transact transact = db.Transact.Find(id);


                if (transact == null)
                {
                    return HttpNotFound();
                }
                return View(transact);


            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Transacts/Create
        public ActionResult Create(string title)
        {
            MainClass mc = new MainClass();

            try
            {
                if (Session["id_client"] == null)
                    return RedirectToAction("Log", "Home");



                int id_client = int.Parse(Session["id_client"].ToString());
                var transact = db.Transact.Include(t => t.client).Include(t => t.Client_subcategories).Include(t => t.Client_wallet);


                var linqWallet = from w in db.wallet
                                 join cw in db.Client_wallet on w.id_wallet equals cw.id_wallet
                                 where cw.id_client == id_client
                                 select new { id_Client_wallet = cw.id_Client_wallet, Name_wallet = w.Name_wallet };




                var linqSubcategories = from x in db.subcategories
                                        join cs in db.Client_subcategories on x.id_subcategories equals cs.id_subcategories
                                        where cs.id_client == id_client
                                        select new { id_Client_subcategories = cs.id_Client_subcategories, Name_subcategory = x.Name_subcategory };


                if (title == "Dochody")
                {
                    linqSubcategories = from x in db.subcategories
                                        join cs in db.Client_subcategories on x.id_subcategories equals cs.id_subcategories
                                        where cs.id_client == id_client & x.id_categories >= 2
                                        select new { id_Client_subcategories = cs.id_Client_subcategories, Name_subcategory = x.Name_subcategory };

                }

                if (title == "Wydatki")
                {
                    linqSubcategories = from x in db.subcategories
                                        join cs in db.Client_subcategories on x.id_subcategories equals cs.id_subcategories
                                        where cs.id_client == id_client & x.id_categories <= 2
                                        select new { id_Client_subcategories = cs.id_Client_subcategories, Name_subcategory = x.Name_subcategory };

                }


                ViewBag.H2 = title;
                DateTime data = DateTime.Now;
                string year = data.Year.ToString();
                string month = data.Month.ToString();
                string day = data.Day.ToString();
                if (int.Parse(month) < 10)
                    month = "0" + month;

                if (int.Parse(day) < 10)
                    day = "0" + day;

                ViewBag.date_Transaction = year + "-" + month + "-" + day;



                ViewBag.id_client = new SelectList(db.client, "id_client", "firstname");
                ViewBag.id_Client_subcategories = new SelectList(linqSubcategories, "id_Client_subcategories", "Name_subcategory");
                ViewBag.id_Client_wallet = new SelectList(linqWallet, "id_Client_wallet", "Name_wallet");
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Transacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_trans,Amount,date_Transaction,descript,IfIncome,id_Client_subcategories,id_Client_wallet,id_client")] Transact transact, string id_User, string incomeExpenses)
        {
            try
            {

                if (Session["id_client"] == null)
                    return RedirectToAction("Log", "Home");

                int id_client = int.Parse(Session["id_client"].ToString());

                var linqWallet = from w in db.wallet
                                 join cw in db.Client_wallet on w.id_wallet equals cw.id_wallet
                                 where cw.id_client == id_client
                                 select new { id_Client_wallet = cw.id_Client_wallet, Name_wallet = w.Name_wallet };




                var linqSubcategories = from x in db.subcategories
                                        join cs in db.Client_subcategories on x.id_subcategories equals cs.id_subcategories
                                        where cs.id_client == id_client
                                        select new { id_Client_subcategories = cs.id_Client_subcategories, Name_subcategory = x.Name_subcategory };


                ViewBag.id_Client_subcategories = new SelectList(linqSubcategories, "id_Client_subcategories", "Name_subcategory");
                ViewBag.id_Client_wallet = new SelectList(linqWallet, "id_Client_wallet", "Name_wallet");



                float amount2;



                if (float.TryParse(transact.Amount.ToString(), out amount2))
                {
                    transact.Amount = amount2;
                    ViewBag.info = amount2 + "to liczba jest";
                    //return RedirectToAction("Create","Transacts");

                }
                else
                {
                    ViewBag.info = transact.Amount.ToString() + "to nie liczba";
                    return View();
                }




                //ViewBag.amount = amount2;
                ViewBag.info = id_User;
                ViewBag.info2 = incomeExpenses;
                ViewBag.H2 = incomeExpenses;
                ViewBag.id_client = new SelectList(db.client, "id_client", "firstname", transact.id_client);




                string stringTransact;


                if (incomeExpenses.Contains("Dochody"))
                {
                    transact.IfIncome = true;
                    stringTransact = "income";
                }
                else
                {
                    if (incomeExpenses == "Wydatki")
                    {
                        transact.IfIncome = false;
                        stringTransact = "expenses";
                    }
                    else
                    {

                        return View(transact);
                    }
                }

                transact.id_client = int.Parse(Session["id_client"].ToString());



                if (ModelState.IsValid)
                {
                    db.Transact.Add(transact);
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                    return RedirectToAction("Index", "Transacts", new { ifincome = stringTransact });
                }


                return View(transact);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }



        // GET: Transacts/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (Session["id_client"] == null)
                    return RedirectToAction("Log", "Home");

                int id_client = int.Parse(Session["id_client"].ToString());

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Transact transact = db.Transact.Find(id);
                ViewBag.Amount = transact.Amount;
                ViewBag.date_Transaction = transact.date_Transaction;
                ViewBag.descript = transact.descript;



                if (transact == null)
                {
                    return HttpNotFound();
                }
                // ViewBag.id_client = new SelectList(db.client, "id_client", "firstname", transact.id_client);

                var linqWallet = from w in db.wallet
                                 join cw in db.Client_wallet on w.id_wallet equals cw.id_wallet
                                 where cw.id_client == id_client
                                 select new { id_Client_wallet = cw.id_Client_wallet, Name_wallet = w.Name_wallet };




                var linqSubcategories = from x in db.subcategories
                                        join cs in db.Client_subcategories on x.id_subcategories equals cs.id_subcategories
                                        where cs.id_client == id_client
                                        select new { id_Client_subcategories = cs.id_Client_subcategories, Name_subcategory = x.Name_subcategory };




                ViewBag.id_Client_subcategories = new SelectList(linqSubcategories, "id_Client_subcategories", "Name_subcategory");
                ViewBag.id_Client_wallet = new SelectList(linqWallet, "id_Client_wallet", "Name_wallet");


                return View(transact);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Transacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_trans,Amount,date_Transaction,descript,IfIncome,id_Client_subcategories,id_Client_wallet,id_client")] Transact transact, string wall)
        {
            try
            {
                string stringIfIncome;

                if (transact.IfIncome == true)
                    stringIfIncome = "income";
                else
                    stringIfIncome = "expenses";

                ViewBag.infowall = wall;
                // ViewBag.infowall2 = transact.id_wallet;


                transact.id_client = int.Parse(Session["id_client"].ToString());

                if (ModelState.IsValid)
                {
                    db.Entry(transact).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Transacts", new { ifincome = stringIfIncome });
                }

                //ViewBag.id_SubCategories = new SelectList(db.subcategories, "id_subcategories", "Name_subcategory", transact.id_SubCategories);
                //ViewBag.id_wallet = new SelectList(db.wallet, "id_wallet", "Name_wallet", transact.id_wallet);
                return View(transact);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Transacts/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (Session["id_client"] == null)
                    return RedirectToAction("Log", "Home");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Transact transact = db.Transact.Find(id);
                if (transact == null)
                {
                    return HttpNotFound();
                }
                return View(transact);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Transacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                Transact transact = db.Transact.Find(id);

                string stringIfIncome;

                if (transact.IfIncome == true)
                    stringIfIncome = "income";
                else
                    stringIfIncome = "expenses";

                db.Transact.Remove(transact);
                db.SaveChanges();

                return RedirectToAction("Index", "Transacts", new { ifincome = stringIfIncome });
            }
            catch (Exception)
            { return RedirectToAction("Index", "Home"); }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Summary()
        {
            //try { 
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            var transact = db.Transact.Include(t => t.client).Include(t => t.Client_subcategories.subcategories);//  .Include(db.Client_subcategories.Include(s=>s.subcategories));//.Include(t => t.subcategories).Include(t => t.wallet);




            if (Session["id_client"] != null)
            {
                int? id_client = int.Parse(Session["id_client"].ToString());
                ///id_client = 1;
                //suma dochodów
                var linqClientIncome = from x in transact
                                       where x.id_client == id_client & x.IfIncome == true
                                       select x;

                var income = linqClientIncome.AsEnumerable().Sum(x => x.Amount);


                // var cultureInfo = CultureInfo.GetCultureInfo("pl-PL");
                string q = String.Format(cultureInfo, "{0:C}", income);

                ViewBag.income = income;
                ViewBag.income2 = q;


                //Suma wydatków
                var linqClientExpenses = from x in transact
                                         where x.id_client == id_client & x.IfIncome == false
                                         select x;

                var expenses = linqClientExpenses.AsEnumerable().Sum(x => x.Amount);


                string expenses2 = String.Format(cultureInfo, "{0:C}", expenses);

                ViewBag.expenses = expenses;
                ViewBag.expenses2 = expenses2;



                // Stan konta
                string sum = String.Format(cultureInfo, "{0:C}", income - expenses);
                ViewBag.sum = sum;




                var linqSummary = from x in transact
                                  where x.id_client == id_client
                                  group x by x.date_Transaction into dateSummary
                                  select new DateTransact
                                  {
                                      // date_transaction = dateSummary.Key,
                                      // TotalIncome = dateSummary.Sum(y => y.Amount)

                                  };


                ViewBag.linqSum = linqSummary;


                string idClient = id_client.ToString();
                string query = "select   datename(mm,date_Transaction) as date_Transaction, SUM(Amount) AS Liczba from Transact where  IfIncome = 1 AND id_client=" + idClient + "  GROUP BY   datename(mm,date_Transaction),  month(date_Transaction)  order by  month(date_Transaction) ";

                IEnumerable<DateTransact> dataSum = db.Database.SqlQuery<DateTransact>(query);

                string query2 = "select datename(mm,date_Transaction) as date_Transaction, Sum(Amount) as Liczba from Transact where IfIncome = 0 AND id_client =" + idClient + " group by  datename(mm,date_Transaction), month(date_Transaction)    order by  month(date_Transaction)  ";
                IEnumerable<DateTransact> dataSumExpenses = db.Database.SqlQuery<DateTransact>(query2);


                string query3 = "select s.Name_subcategory AS Name_subcategory, sum (t.Amount) as Suma from Transact t join Client_subcategories cs on t.id_Client_subcategories = cs.id_Client_subcategories join subcategories s on cs.id_subcategories = s.id_subcategories where IfIncome = 1 and cs.id_client = " + idClient + " group by  s.Name_subcategory";
                IEnumerable<CategoriesSum> categoriesSums = db.Database.SqlQuery<CategoriesSum>(query3);

                string query4 = "select s.Name_subcategory AS Name_subcategory, sum (t.Amount) as Suma from Transact t join Client_subcategories cs on t.id_Client_subcategories = cs.id_Client_subcategories join subcategories s on cs.id_subcategories = s.id_subcategories where IfIncome = 0 and cs.id_client = " + idClient + " group by  s.Name_subcategory";
                IEnumerable<CategoriesSum> categoriesSumsExpenses = db.Database.SqlQuery<CategoriesSum>(query4);


                string query5 = "select w.id_wallet AS id_wallet,w.Name_wallet AS Name_wallet,sum(t.Amount) as Sum from Transact t join Client_wallet cw on t.id_Client_wallet = cw.id_Client_wallet join wallet w on cw.id_wallet = w.id_wallet where t.id_client = " + idClient + " group by w.id_wallet,w.Name_wallet";
                IEnumerable<walletClass> walletData = db.Database.SqlQuery<walletClass>(query5);


                dynamic Mymodel = new ExpandoObject();
                Mymodel.dataSum = dataSum.ToList();
                Mymodel.dataSumExpenses = dataSumExpenses.ToList();
                Mymodel.categoriesSums = categoriesSums.ToList();
                Mymodel.categoriesSumsExpenses = categoriesSumsExpenses.ToList();
                Mymodel.walletData = walletData.ToList();





                return View(Mymodel);
            }


            return View();

            //}
            //catch (Exception f) { return RedirectToAction("Index", "Home"); }
        }









        public ActionResult Change()
        {
            try
            {
                if (Session["id_client"] == null)
                    return RedirectToAction("Log", "Home");

                int id_client = int.Parse(Session["id_client"].ToString());

                var linqWallet = from w in db.wallet
                                 join cw in db.Client_wallet on w.id_wallet equals cw.id_wallet
                                 where cw.id_client == id_client
                                 select new { id_Client_wallet = cw.id_Client_wallet, Name_wallet = w.Name_wallet };


                var list = new SelectList(linqWallet, "id_Client_wallet", "Name_wallet");

                ViewBag.id_wallet2 = list;


                return View();
            }
            catch (Exception) { return RedirectToAction("Index", "Home"); }
        }




        [HttpPost]
        public ActionResult Change(string wallfrom, string wallto, [Bind(Include = "id_trans,Amount,date_Transaction,descript,IfIncome,id_SubCategories,id_wallet,id_client")] Transact transact2)
        {
            try
            {
                if (Session["id_client"] == null)
                    return RedirectToAction("Log", "Home");


                int id_client = int.Parse(Session["id_client"].ToString());




                var linqWallet = from w in db.wallet
                                 join cw in db.Client_wallet on w.id_wallet equals cw.id_wallet
                                 where cw.id_client == id_client
                                 select new { id_Client_wallet = cw.id_Client_wallet, Name_wallet = w.Name_wallet };


                var list = new SelectList(linqWallet, "id_Client_wallet", "Name_wallet");

                ViewBag.id_wallet2 = list;

                ViewBag.amount = transact2.Amount.ToString();




                float amount;
                if (!float.TryParse(transact2.Amount.ToString(), out amount))
                {
                    ViewBag.w = "Błędne dane";
                    return View();
                }


                int nrfrom = int.Parse(wallfrom.ToString());
                //wallet walletfrom = db.wallet.Find(nrfrom);
                Client_wallet walletfrom = db.Client_wallet.Find(nrfrom);


                int nrto = int.Parse(wallto.ToString());
                // wallet wallet2 = db.wallet.Find(nrto);
                Client_wallet wallet2 = db.Client_wallet.Find(nrto);


                transact2.id_client = id_client;

                string description = "Transefer z: " + walletfrom.wallet.Name_wallet + " do " + wallet2.wallet.Name_wallet;
                transact2.descript = description;


                //transact2.id_Client_subcategories = 12;

                DateTime time = DateTime.Now;
                transact2.date_Transaction = time;




                //ifincome
                //id_wallet

                // var transact = db.Transact.Include(t => t.client);//.Include(t => t.subcategories).Include(t => t.wallet);
                var transact = db.Transact.Include(t => t.client).Include(t => t.Client_subcategories).Include(t => t.Client_wallet);
                //Wypłacanie

                transact2.IfIncome = false;
                transact2.id_Client_wallet = walletfrom.id_Client_wallet;

                if (ModelState.IsValid)
                {

                    var linqClientIncome = from x in transact
                                           join cw in db.Client_wallet on x.id_Client_wallet equals cw.id_Client_wallet
                                           where x.id_client == id_client & cw.id_wallet == walletfrom.id_wallet
                                           select x;

                    var income = linqClientIncome.AsEnumerable().Sum(x => x.Amount);



                    if (amount > income)
                    {
                        ViewBag.w = "Na koncie nie ma tyly pieniędzy. Stan " + walletfrom.wallet.Name_wallet + " to: " + income;
                        return View();
                    }

                    if (walletfrom.id_wallet == wallet2.id_wallet)
                    {
                        ViewBag.w = "Nie można przelać z tego samego konta na to samo ";
                        return View();
                    }

                    ViewBag.infowall = "Przelano " + amount + " z konta: " + walletfrom.wallet.Name_wallet + " na " + wallet2.wallet.Name_wallet;
                    ViewBag.infowall2 = walletfrom.wallet.Name_wallet;
                    ViewBag.infowall3 = wallet2.wallet.Name_wallet;





                    db.Transact.Add(transact2);
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                    ViewBag.w = "wypłacono";
                }
                else
                {
                    ViewBag.w = "Nie wypłacono";
                }

                //Wpłacenie


                transact2.IfIncome = true;
                transact2.id_Client_wallet = wallet2.id_Client_wallet;


                if (ModelState.IsValid)
                {
                    db.Transact.Add(transact2);
                    db.SaveChanges();

                    ViewBag.ww = "wpłacono+";
                }




                return View();
            }
            catch (Exception) { return RedirectToAction("Index", "Home"); }
        }




    }
}
