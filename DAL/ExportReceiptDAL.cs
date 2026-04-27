using Microsoft.Data.SqlClient;
using System.Data;

namespace QuanLyKho.DAL
{
    public class ExportReceiptDAL
    {
        private static ExportReceiptDAL? instance;

        public static ExportReceiptDAL Instance
        {
            get
            {
                instance ??= new ExportReceiptDAL();
                return instance;
            }
            private set => instance = value;
        }

        private ExportReceiptDAL() { }

        public DataTable GetByDateRange(DateTime from, DateTime to) =>
            DataProvider.Instance.ExecuteQuery(
                "USP_GetExportReceiptByDate @NgayBatDau, @NgayKetThuc",
                new object[] { from.Date, to.Date });

        public int InsertHeader(string? purpose)
        {
            var pars = new[]
            {
                new SqlParameter("@Purpose", string.IsNullOrWhiteSpace(purpose) ? (object)DBNull.Value : purpose.Trim())
            };
            return DataProvider.Instance.ExecuteStoredProcedureThenIdentCurrent(
                "USP_InsertExportReceipt",
                pars,
                "ExportReceipt");
        }

        public void InsertDetail(int exportId, int skuId, int locationId, int quantity) =>
            DataProvider.Instance.ExecuteNonQuery(
                "USP_XuatKho @ExportID, @SkuID, @LocationID, @Quantity",
                new object[] { exportId, skuId, locationId, quantity });
    }
}
