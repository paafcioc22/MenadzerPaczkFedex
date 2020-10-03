using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenadżerPaczek.Model
{
    public class ConnToBase
    {
        protected string sqlconn;
        protected string sqlconnXL;
        class funkcje
        {
            public static int wersja_nr = 20193;
            //public static int SessionID;
            public static string serwer = "10.8.0.134\\optima";
            public static string serwerXL = "db1";
            public static string database = "CDN_Joart_BCC";//cdnxl_joart ; cdnxl_test
            public static string databaseXL = "CDNXL_JOART";//cdnxl_joart ; cdnxl_test
            public static string sqluser = "szachownica";
            public static string sqluserXL = "sa";
            public static string password = "6@#RE2us";
            public static string passwordXL = "sqlSQL123#";
            //public static int dokid;
            public static int typZamknij = 0; // //1-bufor , 0- zatwierdź ////////////////////// 
        }


        public ConnToBase()
        {
            sqlconn = $"SERVER={funkcje.serwer};" +
              $"DATABASE={funkcje.database};" +
              $"TRUSTED_CONNECTION=No; UID={funkcje.sqluser}; " +
              $"PWD={funkcje.password};Connection Timeout=30";


            sqlconnXL = $"SERVER={funkcje.serwerXL};" +
             $"DATABASE={funkcje.databaseXL};" +
             $"TRUSTED_CONNECTION=No; UID={funkcje.sqluserXL}; " +
             $"PWD={funkcje.passwordXL};Connection Timeout=30";
        }

    }
}
