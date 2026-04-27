using System.Data;

namespace QuanLyKho.DAL
{
    public class ProductDAL
    {
        private static ProductDAL? instance;

        public static ProductDAL Instance
        {
            get
            {
                instance ??= new ProductDAL();
                return instance;
            }
            private set => instance = value;
        }

        private ProductDAL() { }

        public DataTable List() => DataProvider.Instance.ExecuteQuery("USP_ListProduct");

        public DataTable SearchByName(string name) =>
            DataProvider.Instance.ExecuteQuery("USP_SearchProductByName @ProductName", new object[] { name });

        public DataTable GetById(int id) =>
            DataProvider.Instance.ExecuteQuery("USP_GetProductByID @ProductID", new object[] { id });

        public void Insert(string productName, string? description, int? categoryId) =>
            DataProvider.Instance.ExecuteNonQuery(
                "USP_InsertProduct @ProductName, @Description, @CategoryID",
                new object[]
                {
                    productName,
                    description ?? (object)DBNull.Value,
                    categoryId ?? (object)DBNull.Value
                });

        public void Update(int id, string? productName, string? description, int? categoryId) =>
            DataProvider.Instance.ExecuteNonQuery(
                "USP_UpdateProduct @ProductID, @ProductName, @Description, @CategoryID",
                new object[]
                {
                    id,
                    productName ?? (object)DBNull.Value,
                    description ?? (object)DBNull.Value,
                    categoryId ?? (object)DBNull.Value
                });

        public void Delete(int id) =>
            DataProvider.Instance.ExecuteNonQuery("USP_DeleteProduct @ProductID", new object[] { id });
    }
}
