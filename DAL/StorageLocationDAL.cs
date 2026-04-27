using System.Data;
using QuanLyKho.DTO;

namespace QuanLyKho.DAL
{
    public class StorageLocationDAL
    {
        private static StorageLocationDAL instance;// Singleton
        public static StorageLocationDAL Instance
        {
            get { if (instance == null) instance = new StorageLocationDAL(); return instance; }
            private set { instance = value; }
        }
        private StorageLocationDAL() { }

        public List<StorageLocation> LoadLocationList()
        {
            List<StorageLocation> locationList = new List<StorageLocation>();

            DataTable data=DataProvider.Instance.ExecuteQuery("USP_GetStorageLocation");
            foreach (DataRow item in data.Rows)
            {
                StorageLocation location = new StorageLocation(item);
                locationList.Add(location);
            }
            return locationList;
        }

        public DataTable ListAsTable() =>
            DataProvider.Instance.ExecuteQuery("USP_GetStorageLocation");

        public DataTable SearchByDescription(string description) =>
            DataProvider.Instance.ExecuteQuery("USP_SearchLocationByDescription @Description", new object[] { description });

        public void Insert(string description, int capacity, int warehouseId) =>
            DataProvider.Instance.ExecuteNonQuery(
                "USP_InsertStorageLocation @Description, @Capacity, @WarehouseID",
                new object[] { description, capacity, warehouseId });

        public void Update(int locationId, string? description, int? capacity, int? warehouseId) =>
            DataProvider.Instance.ExecuteNonQuery(
                "USP_UpdateStorageLocation @LocationID, @Description, @Capacity, @WarehouseID",
                new object[]
                {
                    locationId,
                    description ?? (object)DBNull.Value,
                    capacity ?? (object)DBNull.Value,
                    warehouseId ?? (object)DBNull.Value
                });

        public void Delete(int locationId) =>
            DataProvider.Instance.ExecuteNonQuery("USP_DeleteStorageLocation @LocationID", new object[] { locationId });
    }
}
