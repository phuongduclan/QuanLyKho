using System.Data;

namespace QuanLyKho.DAL
{
    public class CategoryDAL
    {
        private static CategoryDAL? instance;

        public static CategoryDAL Instance
        {
            get
            {
                instance ??= new CategoryDAL();
                return instance;
            }
            private set => instance = value;
        }

        private CategoryDAL() { }

        public DataTable List() => DataProvider.Instance.ExecuteQuery("USP_ListCategory");

        public DataTable SearchByName(string name) =>
            DataProvider.Instance.ExecuteQuery("USP_SearchCategoryByName @CategoryName", new object[] { name });

        public DataTable GetById(int id) =>
            DataProvider.Instance.ExecuteQuery("USP_GetCategoryByID @CategoryID", new object[] { id });

        public void Insert(string categoryName) =>
            DataProvider.Instance.ExecuteNonQuery("USP_InsertCategory @CategoryName", new object[] { categoryName });

        public void Update(int id, string categoryName) =>
            DataProvider.Instance.ExecuteNonQuery(
                "USP_UpdateCategory @CategoryID, @CategoryName",
                new object[] { id, categoryName });

        public void Delete(int id) =>
            DataProvider.Instance.ExecuteNonQuery("USP_DeleteCategory @CategoryID", new object[] { id });
    }
}
