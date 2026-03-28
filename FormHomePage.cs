using QuanLyKho.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using QuanLyKho.DAL; 

namespace QuanLyKho
{
    public partial class FormHomePage : Form
    {
        public FormHomePage()
        {
            InitializeComponent();

            // Thiết lập listView1 ở chế độ Details với các cột
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Columns.Add("SKU Code",    120);
            listView1.Columns.Add("Sản phẩm",   180);
            listView1.Columns.Add("Đơn vị",      80);
            listView1.Columns.Add("Số lượng",    90);

            LoadLocation();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAdmin f = new FormAdmin();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        void LoadLocation()
        {
            List<StorageLocation> locationList = StorageLocationDAL.Instance.LoadLocationList();

            foreach (StorageLocation location in locationList)
            {
                Button btn = new Button()
                {
                    Width  = 200,
                    Height = 200,
                    Text   = location.LocationDescription1 + Environment.NewLine + "Kho: " + location.WarehouseID1
                };
                btn.Click += Btn_Click;
                btn.Tag    = location;   // lưu object StorageLocation vào Tag

                switch (location.WarehouseID1)
                {
                    case 1:  btn.BackColor = Color.LightBlue;   break;
                    case 2:  btn.BackColor = Color.LightGreen;  break;
                    case 3:  btn.BackColor = Color.LightYellow; break;
                    default: btn.BackColor = Color.LightGray;   break;
                }

                flowLayoutPanel3.Controls.Add(btn);
            }
        }

        /// <summary>
        /// Load tồn kho theo locationId và hiển thị lên listView1
        /// </summary>
        void ShowInventory(int locationId)
        {
            listView1.Items.Clear();

            List<Inventory> inventoryList = InventoryDAL.Instance.GetInventoryByLocation(locationId);

            foreach (Inventory item in inventoryList)
            {
                ListViewItem row = new ListViewItem(item.SkuCode);
                row.SubItems.Add(item.ProductName);
                row.SubItems.Add(item.Unit);
                row.SubItems.Add(item.Quantity.ToString());
                listView1.Items.Add(row);
            }
        }

        private void Btn_Click(object? sender, EventArgs e)
        {
            StorageLocation location = (StorageLocation)(sender as Button).Tag;
            int locationId = location.LocationID1;
            ShowInventory(locationId);
        }
    }
}
