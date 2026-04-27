using System.Data;

namespace QuanLyKho.DAL
{
    public class WarehouseDAL
    {
        private static WarehouseDAL? instance;

        public static WarehouseDAL Instance
        {
            get
            {
                instance ??= new WarehouseDAL();
                return instance;
            }
            private set => instance = value;
        }

        private WarehouseDAL() { }

        public DataTable List() => DataProvider.Instance.ExecuteQuery("USP_ListWarehouse");

        public DataTable SearchByName(string name) =>
            DataProvider.Instance.ExecuteQuery("USP_SearchWarehouseByName @WarehouseName", new object[] { name });

        public DataTable GetById(int id) =>
            DataProvider.Instance.ExecuteQuery("USP_GetWarehouseByID @WarehouseID", new object[] { id });

        public void Insert(string name, string? address, int maxCapacity) =>
            DataProvider.Instance.ExecuteNonQuery(
                "USP_InsertWarehouse @WarehouseName, @Address, @MaxCapacity",
                new object[] { name, address ?? (object)DBNull.Value, maxCapacity });

        public void Update(int id, string? name, string? address, int? maxCapacity) =>
            DataProvider.Instance.ExecuteNonQuery(
                "USP_UpdateWarehouse @WarehouseID, @WarehouseName, @Address, @MaxCapacity",
                new object[]
                {
                    id,
                    name ?? (object)DBNull.Value,
                    address ?? (object)DBNull.Value,
                    maxCapacity ?? (object)DBNull.Value
                });

        public void Delete(int id) =>
            DataProvider.Instance.ExecuteNonQuery("USP_DeleteWarehouse @WarehouseID", new object[] { id });
    }
}
