using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace QuanLyKho.DTO
{
    public class Inventory
    {
        public Inventory(DataRow row)
        {
            this.LocationId          = (int)row["location_id"];
            this.LocationDescription = row["location_description"] == DBNull.Value ? "" : (string)row["location_description"];
            this.CategoryName        = (string)row["category_name"];
            this.ProductName         = (string)row["product_name"];
            this.SkuCode             = (string)row["sku_code"];
            this.Unit                = row["unit"] == DBNull.Value ? "" : (string)row["unit"];
            this.Quantity            = (int)row["quantity"];
        }

        public int    LocationId          { get; set; }
        public string LocationDescription { get; set; }
        public string CategoryName        { get; set; }
        public string ProductName         { get; set; }
        public string SkuCode             { get; set; }
        public string Unit                { get; set; }
        public int    Quantity            { get; set; }
    }
}
