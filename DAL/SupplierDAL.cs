using Microsoft.Data.SqlClient;
using System.Data;

namespace QuanLyKho.DAL
{
    public class SupplierDAL
    {
        private static SupplierDAL? instance;

        public static SupplierDAL Instance
        {
            get
            {
                instance ??= new SupplierDAL();
                return instance;
            }
            private set => instance = value;
        }

        private SupplierDAL() { }

        public DataTable List() => DataProvider.Instance.ExecuteQuery("USP_ListSupplier");

        public DataTable SearchByName(string name) =>
            DataProvider.Instance.ExecuteQuery("USP_SearchSupplierByName @SupplierName", new object[] { name });

        public DataTable GetById(int id) =>
            DataProvider.Instance.ExecuteQuery("USP_GetSupplierByID @SupplierID", new object[] { id });

        public void Insert(string supplierName, string? address, string? email, string? phone) =>
            DataProvider.Instance.ExecuteNonQuery(
                "USP_InsertSupplier @SupplierName, @Address, @Email, @Phone",
                new object[]
                {
                    supplierName,
                    string.IsNullOrWhiteSpace(address) ? DBNull.Value : address,
                    string.IsNullOrWhiteSpace(email) ? DBNull.Value : email,
                    string.IsNullOrWhiteSpace(phone) ? DBNull.Value : phone
                });

        /// <summary>Script không có USP_UpdateSupplier — cập nhật trực tiếp bảng Supplier.</summary>
        public void Update(int id, string supplierName, string? address, string? email, string? phone)
        {
            const string sql = @"UPDATE Supplier SET
supplier_name = @n,
address = @a,
email = @e,
phone = @p
WHERE supplier_id = @id";

            DataProvider.Instance.ExecuteNonQueryTyped(
                sql,
                CommandType.Text,
                new SqlParameter("@id", id),
                new SqlParameter("@n", supplierName),
                new SqlParameter("@a", address ?? (object)DBNull.Value),
                new SqlParameter("@e", string.IsNullOrWhiteSpace(email) ? DBNull.Value : email!),
                new SqlParameter("@p", string.IsNullOrWhiteSpace(phone) ? DBNull.Value : phone!));
        }

        public void Delete(int id) =>
            DataProvider.Instance.ExecuteNonQuery("USP_DeleteSupplier @SupplierID", new object[] { id });
    }
}
