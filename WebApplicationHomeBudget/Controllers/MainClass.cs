using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WebApplicationHomeBudget.Models;

namespace WebApplicationHomeBudget.Controllers
{
    public class MainClass
    {

        public static HomeBudget3Entities db = new HomeBudget3Entities();

        public static string EncMD5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();
            Byte[] originalBytes = encoder.GetBytes(password);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            password = BitConverter.ToString(encodedBytes).Replace("-", "");
            var result = password.ToLower();
            return result;
        }


        public static bool SameLogin(string Login_Name, int id_client)
        {
            bool IfSameLogin = false;
            //  HomeBudget3Entities db2 = new HomeBudget3Entities();

            foreach (var item in db.client)
            {
                if (item.Login_Name_Unique == Login_Name & item.id_client != id_client)
                    IfSameLogin = true;
            }

            return IfSameLogin;
        }



        public static bool SameLogin(string Login_Name)
        {
            bool IfSameLogin = false;


            foreach (var item in db.client)
            {
                if (item.Login_Name_Unique == Login_Name)
                    IfSameLogin = true;
            }

            return IfSameLogin;
        }


        public IEnumerable<string> WalletLinq()
        {


            var linqWallet4 = from w in db.wallet
                              join cw in db.Client_wallet on w.id_wallet equals cw.id_wallet
                              where cw.id_client == 1
                              select new { id_Client_wallet = cw.id_Client_wallet, Name_wallet = w.Name_wallet }.ToString();

            return linqWallet4;
        }

        public void LinqW(out IEnumerable<string> returnq)
        {
            var linqWallet = from w in db.wallet
                             join cw in db.Client_wallet on w.id_wallet equals cw.id_wallet
                             where cw.id_client == 1
                             select new { id_Client_wallet = cw.id_Client_wallet, Name_wallet = w.Name_wallet }.ToString();

            returnq = linqWallet;

        }



    }
}