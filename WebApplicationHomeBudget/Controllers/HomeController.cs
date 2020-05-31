using System;
using System.Web.Mvc;
using System.Web.Security;
using WebApplicationHomeBudget.Models;

namespace WebApplicationHomeBudget.Controllers
{
    public class HomeController : Controller
    {

        private HomeBudget3Entities db = new HomeBudget3Entities();


        public string nazwaLogin;
        public client cl;




        public ActionResult Index()
        {
            try
            {

                if (Session["id_client"] == null)
                {
                    return RedirectToAction("Log", "Home");
                }

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Shared");
            }
        }




        public ActionResult LogOut()
        {
            if (Session != null)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
            }
            return RedirectToAction("Log");
        }

        public ActionResult Log()
        {
            return View();
        }

        //Logowanie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Log([Bind(Include = "id_client,firstname,surname,email,Login_Name_Unique,Password_text ")] client client1)
        {

            try
            {

                int? IdNumber = null;
                foreach (var item in db.client)
                {
                    if (item.Login_Name_Unique == client1.Login_Name_Unique)
                        IdNumber = item.id_client;
                }

                if (IdNumber != null & client1.Password_text != null)
                {

                    cl = db.client.Find(IdNumber);



                    string passText = client1.Password_text;


                    string passEncrypted = MainClass.EncMD5(passText);

                    foreach (var x in db.client)
                    {
                        if (x.Login_Name_Unique == client1.Login_Name_Unique & x.Password_text == passEncrypted)
                        {
                            Session["Login_Name"] = cl.Login_Name_Unique;
                            Session["id_client"] = cl.id_client;

                            //return RedirectToAction("Edit", "clients", new { id = cl.id_client });
                            //return RedirectToAction ("Index", "Transacts", new { ifincome = "income" });
                            return RedirectToAction("Index", "Home");

                        }
                    }

                }

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Log", "Home");
            }
        }





        public ActionResult Register()
        {

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        // public ActionResult Register([Bind(Include = "id_client,firstname,surname,email,Id_login,Login_Name_Unique,Password_text ")] client client1,string PasswordRepeat)
        //{

        public ActionResult Register(string firstname, string surename, string email, string Login, string Password, string PasswordRepeat)
        {
            try
            {

                var client1 = new client { firstname = firstname, surname = surename, email = email, Login_Name_Unique = Login, Password_text = Password };


                bool IfSameLogin = MainClass.SameLogin(client1.Login_Name_Unique);

                ViewBag.error = IfSameLogin.ToString();
                ViewBag.pass2 = "asdasd";

                if (client1.Password_text != PasswordRepeat)
                    ViewBag.errorMesseage = "Hasła nie są takie same";

                if (IfSameLogin)
                    ViewBag.errorLogin = "Taki login już istnieje";


                if (client1.Password_text == PasswordRepeat & !IfSameLogin)
                {


                    //Dodać sprawdzanie czy silne hasło





                    client1.Password_text = MainClass.EncMD5(client1.Password_text);
                    ViewBag.registerInfo = "Zarejesrtowano użytkownika: " + client1.Login_Name_Unique;
                    db.client.Add(client1);
                    db.SaveChanges();


                    //dodanie kategorii do Client_subcategories
                    ViewBag.id_client = client1.id_client;
                    var client_subcategor = new Client_subcategories();


                    for (int i = 1; i <= 19; i++)
                    {
                        client_subcategor.id_client = client1.id_client;
                        client_subcategor.id_subcategories = i;
                        db.Client_subcategories.Add(client_subcategor);
                        db.SaveChanges();
                    }

                    Client_wallet client_Wallet1 = new Client_wallet();

                    for (int j = 1; j <= 3; j++)
                    {
                        client_Wallet1.id_client = client1.id_client;
                        client_Wallet1.id_wallet = j;
                        db.Client_wallet.Add(client_Wallet1);
                        db.SaveChanges();

                    }


                    //return View("Log");
                    return RedirectToAction("Log", "Home");

                }


                return View(client1);
            }
            catch (Exception)
            {
                return RedirectToAction("Log", "Home");
            }
        }

        public ActionResult ChangePassword()
        {
            return View("Register");
        }





    }
}