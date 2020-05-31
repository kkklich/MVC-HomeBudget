using System.Web.Helpers;
using System.Web.Mvc;
using WebApplicationHomeBudget.Models;

namespace WebApplicationHomeBudget.Controllers
{
    public class ChartController : Controller
    {

        private HomeBudget3Entities db = new HomeBudget3Entities();

        string idClient;

        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ChartIncome()
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");


            idClient = Session["id_client"].ToString();
            string query4 = "select s.Name_subcategory AS Name_subcategory, sum (t.Amount) as Suma from Transact t join Client_subcategories cs on t.id_Client_subcategories = cs.id_Client_subcategories join subcategories s on cs.id_subcategories = s.id_subcategories where IfIncome = 1 and cs.id_client = " + idClient + " group by  s.Name_subcategory";
            var data = db.Database.SqlQuery<CategoriesSum>(query4);


            var myChart = new Chart(width: 600, height: 400)
                        .AddSeries(
                name: "Employee", chartType: "Pie",
                xValue: data, xField: "Name_subcategory",
                yValues: data, yFields: "Suma")
            .Write();


            return null;

        }


        public ActionResult ChartExpenses()
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            idClient = Session["id_client"].ToString();
            string query3 = "select s.Name_subcategory AS Name_subcategory, sum (t.Amount) as Suma from Transact t join Client_subcategories cs on t.id_Client_subcategories = cs.id_Client_subcategories join subcategories s on cs.id_subcategories = s.id_subcategories where IfIncome = 0 and cs.id_client = " + idClient + " group by  s.Name_subcategory";
            var data = db.Database.SqlQuery<CategoriesSum>(query3);


            var myChart = new Chart(width: 600, height: 400)
                       .AddSeries(
                name: "Employee", chartType: "Pie",
                xValue: data, xField: "Name_subcategory",
                yValues: data, yFields: "Suma")
            .Write();


            return null;

        }



        public ActionResult ChartDateIncome()
        {

            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            idClient = Session["id_client"].ToString();

            string query = "select   datename(mm,date_Transaction) as date_Transaction, SUM(Amount) AS Liczba from Transact where  IfIncome = 1 AND id_client=" + idClient + "   GROUP BY   datename(mm,date_Transaction),  month(date_Transaction)  order by  month(date_Transaction) ";

            var data = db.Database.SqlQuery<DateTransact>(query);


            var myChart = new Chart(width: 600, height: 400, theme: ChartTheme.Green)
                       .AddSeries(
                name: "Dochody",
                xValue: data, xField: "date_Transaction",
                yValues: data, yFields: "Liczba").AddLegend()
            .Write();


            return null;

        }





        public ActionResult ChartDateExpenses()
        {

            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            idClient = Session["id_client"].ToString();

            string query = "select   datename(mm,date_Transaction) as date_Transaction, SUM(Amount) AS Liczba from Transact where  IfIncome = 0 AND id_client=" + idClient + " GROUP BY   datename(mm,date_Transaction),  month(date_Transaction)  order by  month(date_Transaction) ";

            var data = db.Database.SqlQuery<DateTransact>(query);


            var myChart = new Chart(width: 600, height: 400, theme: ChartTheme.Yellow)
                     .AddSeries(
                name: "Wydatki", axisLabel: "Liczba",
                xValue: data, xField: "date_Transaction",
                yValues: data, yFields: "Liczba")
            .Write();


            return null;

        }

        public ActionResult ChartWallet()
        {
            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            idClient = Session["id_client"].ToString();

            string query = "select w.id_wallet AS id_wallet,w.Name_wallet AS Name_wallet,sum(t.Amount) as Sum from Transact t join Client_wallet cw on t.id_Client_wallet = cw.id_Client_wallet join wallet w on cw.id_wallet = w.id_wallet where t.id_client = " + idClient + " group by w.id_wallet,w.Name_wallet ";

            var data = db.Database.SqlQuery<walletClass>(query);

            var mychart = new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
                   .AddSeries(
                name: "Portfele", chartType: "Pie",
                xValue: data, xField: "Name_wallet",
                yValues: data, yFields: "Sum").Write();

            return null;

        }




        public ActionResult ChartSummary(double income, double expenses)
        {

            if (Session["id_client"] == null)
                return RedirectToAction("Log", "Home");

            idClient = Session["id_client"].ToString();
            var x = "Dochody: " + income.ToString();
            var y = "Wydatki: " + expenses.ToString();

            var mychart = new Chart(width: 600, height: 400)
                      .AddSeries(
                name: "Portfele", chartType: "Pie",
                xValue: new[] { x, y },
                yValues: new[] { income, expenses }).Write();

            return null;

        }


    }
}