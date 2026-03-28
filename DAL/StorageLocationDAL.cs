using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using QuanLyKho.DTO;
using System.Data.SqlClient;

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

            DataTable data=DataProvider.Instance.ExecuteQuery("USP_GetLocationList");
            foreach (DataRow item in data.Rows)
            {
                StorageLocation location = new StorageLocation(item);
                locationList.Add(location);
            }
            return locationList;
        }
    }

}
