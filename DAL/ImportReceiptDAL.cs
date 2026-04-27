using Microsoft.Data.SqlClient;
using System.Data;

namespace QuanLyKho.DAL
{
    public class ImportReceiptDAL
    {
        private static ImportReceiptDAL? instance;

        public static ImportReceiptDAL Instance
        {
            get
            {
                instance ??= new ImportReceiptDAL();
                return instance;
            }
            private set => instance = value;
        }

        private ImportReceiptDAL() { }

        public DataTable GetByDateRange(DateTime from, DateTime to) =>
            DataProvider.Instance.ExecuteQuery(
                "USP_GetImportReceiptByDate @NgayBatDau, @NgayKetThuc",
                new object[] { from.Date, to.Date });

        public int InsertHeader() =>
            DataProvider.Instance.ExecuteStoredProcedureThenIdentCurrent("USP_InsertImportReceipt", null, "ImportReceipt");

        /// <summary>USP_InsertImportDetail chỉ UPDATE tồn kho — đảm bảo đã có dòng Inventory (0) trước khi nhập.</summary>
        public static void EnsureInventoryRow(int locationId, int skuId)
        {
            const string sql = @"
IF NOT EXISTS (SELECT 1 FROM Inventory WHERE location_id = @lid AND sku_id = @sid)
    INSERT INTO Inventory (location_id, sku_id, quantity) VALUES (@lid, @sid, 0)";

            DataProvider.Instance.ExecuteNonQueryTyped(
                sql,
                CommandType.Text,
                new SqlParameter("@lid", locationId),
                new SqlParameter("@sid", skuId));
        }

        public void InsertDetail(int importId, int skuId, int supplierId, int locationId, int quantity)
        {
            EnsureInventoryRow(locationId, skuId);
            DataProvider.Instance.ExecuteNonQuery(
                "USP_NhapKho @ImportID, @SkuID, @SupplierID, @LocationID, @Quantity",
                new object[] { importId, skuId, supplierId, locationId, quantity });
        }
    }
}
