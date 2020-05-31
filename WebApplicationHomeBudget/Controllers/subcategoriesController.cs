using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplicationHomeBudget.Models;

namespace WebApplicationHomeBudget.Controllers
{
    public class subcategoriesController : Controller
    {
        private HomeBudget3Entities db = new HomeBudget3Entities();




        public ActionResult Index()
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            int? id_client = int.Parse(Session["id_client"].ToString());


            var Client_subcategories = db.Client_subcategories.Include(t => t.subcategories);



            var linqSub = from x in Client_subcategories
                          where x.id_client == id_client
                          orderby x.subcategories.id_categories descending, x.subcategories.Name_subcategory
                          select x;


            return View(linqSub.ToList());
        }



        // GET: subcategories/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcategories subcategories = db.subcategories.Find(id);
            if (subcategories == null)
            {
                return HttpNotFound();
            }
            return View(subcategories);
        }

        // GET: subcategories/Create
        public ActionResult Create()
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");


            ViewBag.id_categories = new SelectList(db.Categories, "id_categories", "Name_category");
            return View();
        }

        // POST: subcategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_subcategories,Name_subcategory,description_Catgory,id_categories")] subcategories subcategories, string wall)
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            int? id_client = int.Parse(Session["id_client"].ToString());

            if (ModelState.IsValid)
            {
                db.subcategories.Add(subcategories);
                db.SaveChanges();

                Client_subcategories client_Subcategori = new Client_subcategories();

                client_Subcategori.id_client = id_client;
                client_Subcategori.id_subcategories = subcategories.id_subcategories;

                db.Client_subcategories.Add(client_Subcategori);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.info = subcategories.id_categories;
            ViewBag.info2 = wall;

            ViewBag.id_categories = new SelectList(db.Categories, "id_categories", "Name_category", subcategories.id_categories);
            return View(subcategories);
        }

        // GET: subcategories/Edit/5
        public ActionResult Edit(int? id, int? id_client_Sub)
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcategories subcategories = db.subcategories.Find(id);
            if (subcategories == null)
            {
                return HttpNotFound();
            }

            @ViewBag.id_client = id_client_Sub;
            ViewBag.id_categories = new SelectList(db.Categories, "id_categories", "Name_category", subcategories.id_categories);
            return View(subcategories);
        }

        // POST: subcategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_subcategories,Name_subcategory,description_Catgory,id_categories")] subcategories subcategories, string id_Client_Subcategories)
        {
            int? id_client = int.Parse(Session["id_client"].ToString());

            if (ModelState.IsValid)
            {
                if (subcategories.id_subcategories > 19)
                {
                    db.Entry(subcategories).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    int id_Cli_Sub = int.Parse(id_Client_Subcategories);

                    db.subcategories.Add(subcategories);
                    db.SaveChanges();

                    //  Client_subcategories client_Subcategori = new Client_subcategories();
                    Client_subcategories client_Subcategori = db.Client_subcategories.Find(id_Cli_Sub);

                    // client_Subcategori.id_client = id_client;
                    client_Subcategori.id_subcategories = subcategories.id_subcategories;

                    db.Entry(client_Subcategori).State = EntityState.Modified;
                    // db.Client_subcategories.Add(client_Subcategori);
                    db.SaveChanges();






                    ViewBag.id_client = id_Client_Subcategories;
                    return RedirectToAction("Index");




                }



            }






            ViewBag.id_categories = new SelectList(db.Categories, "id_categories", "Name_category", subcategories.id_categories);
            return View(subcategories);
        }

        // GET: subcategories/Delete/5
        public ActionResult Delete(int? id, int id_client_Sub)
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcategories subcategories = db.subcategories.Find(id);
            ViewBag.id_client_Sub = id_client_Sub;
            if (subcategories == null)
            {
                return HttpNotFound();
            }
            return View(subcategories);
        }

        // POST: subcategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string id_client_Sub)
        {



            if (id_client_Sub != null)
            {

                int id_C_Sub = int.Parse(id_client_Sub);

                Client_subcategories client_Sub = db.Client_subcategories.Find(id_C_Sub);
                db.Client_subcategories.Remove(client_Sub);
                db.SaveChanges();

                ViewBag.info = id_C_Sub;

                if (id_C_Sub > 3)
                {
                    subcategories subcategories = db.subcategories.Find(id);
                    db.subcategories.Remove(subcategories);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");


            }

            return RedirectToAction("Index");
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
