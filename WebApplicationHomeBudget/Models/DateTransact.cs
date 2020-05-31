using System.ComponentModel.DataAnnotations;

namespace WebApplicationHomeBudget.Models
{
    public class DateTransact
    {

        //public int TotalIncome { get; set; }

        //public double date_transaction { get; set; }

        //  public int id_client { get; set; }
        //[DataType(DataType.Date)]
        public string date_Transaction { get; set; }


        public double? Liczba { get; set; }


    }

    public class CategoriesSum
    {
        public string Name_subcategory { get; set; }

        public double? Suma { get; set; }


    }

    public class walletClass
    {

        public int id_wallet { get; set; }
        public string Name_wallet { get; set; }
        [DataType(DataType.Currency)]
        public double? Sum { get; set; }
        public int id_Client_wallet { get; set; }

    }

    public class Client_wallets
    {
        public int id_Client_wallet { get; set; }
        public string Name_wallet { get; set; }

    }


}