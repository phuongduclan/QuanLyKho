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
        private int currentButtonIndex = -1; // Biến theo dõi nút vị trí hiện tại

        public FormHomePage()
        {
            InitializeComponent();

            // Thiết lập listView1 ở chế độ Details với các cột mới
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Columns.Clear(); 
            listView1.Columns.Add("Vị trí lưu kho",   100);
            listView1.Columns.Add("Danh mục",      100);
            listView1.Columns.Add("Sản phẩm",      100);
            listView1.Columns.Add("SKU Code",      100);
            listView1.Columns.Add("Đơn vị",         100);
            listView1.Columns.Add("Số lượng",       100);

            LoadLocation();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // CHUYỂN KHO: Nhảy đến vị trí đầu tiên của kho tiếp theo
            if (flowLayoutPanel3.Controls.Count == 0) return;

            int currentWarehouseID = -1;
            if (currentButtonIndex != -1)
            {
                Button currentBtn = (Button)flowLayoutPanel3.Controls[currentButtonIndex];
                currentWarehouseID = ((StorageLocation)currentBtn.Tag).WarehouseID1;
            }

            for (int i = 0; i < flowLayoutPanel3.Controls.Count; i++)
            {
                int nextIndex = (currentButtonIndex + 1 + i) % flowLayoutPanel3.Controls.Count;
                Button nextBtn = (Button)flowLayoutPanel3.Controls[nextIndex];
                int nextWarehouseID = ((StorageLocation)nextBtn.Tag).WarehouseID1;

                if (nextWarehouseID != currentWarehouseID)
                {
                    currentButtonIndex = nextIndex;
                    nextBtn.PerformClick();
                    return;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // CHUYỂN VỊ TRÍ: Nhảy sang vị trí kế tiếp trong danh sách
            if (flowLayoutPanel3.Controls.Count == 0) return;

            currentButtonIndex = (currentButtonIndex + 1) % flowLayoutPanel3.Controls.Count;
            Button nextBtn = (Button)flowLayoutPanel3.Controls[currentButtonIndex];
            nextBtn.PerformClick();
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
                // Cột chính: Mô tả vị trí
                ListViewItem row = new ListViewItem(item.LocationDescription);
                
                // Các cột phụ (SubItems)
                row.SubItems.Add(item.CategoryName);
                row.SubItems.Add(item.ProductName);
                row.SubItems.Add(item.SkuCode);
                row.SubItems.Add(item.Unit);
                row.SubItems.Add(item.Quantity.ToString());
                
                listView1.Items.Add(row);
            }
        }

        private void Btn_Click(object? sender, EventArgs e)
        {
            Button btn = sender as Button;
            StorageLocation location = (StorageLocation)btn.Tag;
            
            // Cập nhật chỉ số nút hiện tại
            currentButtonIndex = flowLayoutPanel3.Controls.IndexOf(btn);

            int locationId = location.LocationID1;
            ShowInventory(locationId);
        }
    }
}
