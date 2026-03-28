using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace QuanLyKho.DTO
{
    public class SKU
    {
        public SKU(DataRow row)
        {
            this.SkuId = (int)row["sku_id"];
            this.ProductId = (int)row["product_id"];
            this.SkuCode = (string)row["sku_code"];
            this.Unit = row["unit"] == DBNull.Value ? "" : (string)row["unit"];
        }

        public int SkuId { get; set; }
        public int ProductId { get; set; }
        public string SkuCode { get; set; }
        public string Unit { get; set; }
    }
}
