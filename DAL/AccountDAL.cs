using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace QuanLyKho.DAL
{
    internal class AccountDAL
    {
        private static AccountDAL instance;// Singleton
        public static  AccountDAL Instance
        {
            get { if (instance == null) instance = new AccountDAL() ; return instance; }
            private set { instance = value; }
        }

        private AccountDAL() { }

        public bool Login(string userName, string passWord)
        {
            string query = "USP_Login @userName , @passWord";

            DataTable  result=DataProvider.Instance.ExecuteQuery(query,new object[] { userName, passWord });

            return result.Rows.Count >0;

        }
    }
}
