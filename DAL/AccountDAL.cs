using System.Data;

namespace QuanLyKho.DAL
{
    public class AccountDAL
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
            string query = "USP_Login @UserName , @Password";

            DataTable  result=DataProvider.Instance.ExecuteQuery(query,new object[] { userName, passWord });

            return result.Rows.Count >0;

        }
    }
}
