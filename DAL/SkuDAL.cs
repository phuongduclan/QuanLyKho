using System.Data;

namespace QuanLyKho.DAL
{
    public class SkuDAL
    {
        private static SkuDAL? instance;

        public static SkuDAL Instance
        {
            get
            {
                instance ??= new SkuDAL();
                return instance;
            }
            private set => instance = value;
        }

        private SkuDAL() { }

        public DataTable List() => DataProvider.Instance.ExecuteQuery("USP_ListSku");

        public DataTable SearchByCode(string skuCode) =>
            DataProvider.Instance.ExecuteQuery("USP_SearchSkuByName @SkuCode", new object[] { skuCode });

        public DataTable GetById(int id) =>
            DataProvider.Instance.ExecuteQuery("USP_GetSkuByID @SkuID", new object[] { id });

        public void Insert(string skuCode, string? unit, int productId) =>
            DataProvider.Instance.ExecuteNonQuery(
                "USP_InsertSku @SkuCode, @Unit, @ProductID",
                new object[] { skuCode, unit ?? (object)DBNull.Value, productId });

        public void Update(int id, string? skuCode, string? unit, int? productId) =>
            DataProvider.Instance.ExecuteNonQuery(
                "USP_UpdateSku @SkuID, @SkuCode, @Unit, @ProductID",
                new object[]
                {
                    id,
                    skuCode ?? (object)DBNull.Value,
                    unit ?? (object)DBNull.Value,
                    productId ?? (object)DBNull.Value
                });

        public void Delete(int id) =>
            DataProvider.Instance.ExecuteNonQuery("USP_DeleteSku @SkuID", new object[] { id });
    }
}
