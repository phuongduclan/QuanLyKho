using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using QuanLyKho.DTO;

namespace QuanLyKho.DAL
{
    public class InventoryDAL
    {
        private static InventoryDAL instance;

        public static InventoryDAL Instance
        {
            get { if (instance == null) instance = new InventoryDAL(); return instance; }
            private set { instance = value; }
        }

        private InventoryDAL() { }

        /// <summary>
        /// Gọi proc USP_GetInventoryByLocation để lấy danh sách hàng tồn kho tại một vị trí cụ thể.
        /// </summary>
        public List<Inventory> GetInventoryByLocation(int locationId)
        {
            var list = new List<Inventory>();

            string query = "USP_GetInventoryByLocation @locationid";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { locationId });

            foreach (DataRow row in data.Rows)
            {
                list.Add(new Inventory(row));
            }

            return list;
        }
    }
}
