using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace QuanLyKho.DTO
{
    public class StorageLocation
    {
        public StorageLocation(DataRow row)
        {
            this.LocationID= (int)row["location_id"];
            this.WarehouseID= (int)row["warehouse_id"];
            this.LocationDescription= (string)row["location_description"];
            this.Capacity= (int)row["capacity"];
        }
        public StorageLocation(int locationID, int warehouseID, string locationDescription, int capacity)
        {
            LocationID = locationID;
            WarehouseID = warehouseID;
            LocationDescription = locationDescription;
            Capacity = capacity;
        }
        private int LocationID;
        private int WarehouseID;
        private string LocationDescription;
        private int Capacity;

        public int LocationID1 { get => LocationID; set => LocationID = value; }
        public int WarehouseID1 { get => WarehouseID; set => WarehouseID = value; }
        public string LocationDescription1 { get => LocationDescription; set => LocationDescription = value; }
        public int Capacity1 { get => Capacity; set => Capacity = value; }
    }
}
