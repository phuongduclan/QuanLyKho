using System.Data;
using System.Drawing;
using QuanLyKho.DAL;

namespace QuanLyKho
{
    public partial class FormAdmin : Form
    {
        private readonly DataTable _exportLines = new();
        private readonly DataTable _importLines = new();
        private ComboBox _invViewCombo = null!;
        private DataGridView _invReportGrid = null!;

        public FormAdmin()
        {
            InitializeComponent();

            _exportLines.Columns.Add("sku_id", typeof(int));
            _exportLines.Columns.Add("location_id", typeof(int));
            _exportLines.Columns.Add("sku_code", typeof(string));
            _exportLines.Columns.Add("location", typeof(string));
            _exportLines.Columns.Add("quantity", typeof(int));
            dataGridView9.DataSource = _exportLines;
            dataGridView9.ReadOnly = true;

            _importLines.Columns.Add("sku_id", typeof(int));
            _importLines.Columns.Add("supplier_id", typeof(int));
            _importLines.Columns.Add("location_id", typeof(int));
            _importLines.Columns.Add("sku_code", typeof(string));
            _importLines.Columns.Add("supplier", typeof(string));
            _importLines.Columns.Add("location", typeof(string));
            _importLines.Columns.Add("quantity", typeof(int));
            dataGridView10.DataSource = _importLines;
            dataGridView10.ReadOnly = true;

            SetupInventoryTabPage();
            WireEvents();

            tabControl1.SelectedIndexChanged += (_, _) => ReloadCurrentTab();
            Load += FormAdmin_Load;

            UiTheme.StyleAdminForm(this);
        }

        private void FormAdmin_Load(object? sender, EventArgs e)
        {
            var t = DateTime.Today;
            dateTimePicker1.Value = t.AddMonths(-1);
            dateTimePicker2.Value = t;
            dateTimePicker3.Value = t.AddMonths(-1);
            dateTimePicker4.Value = t;
            ReloadCurrentTab();
        }

        private void SetupInventoryTabPage()
        {
            var top = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 48,
                Padding = new Padding(6)
            };
            _invViewCombo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 520
            };
            _invViewCombo.Items.AddRange(new object[]
            {
                "UV_InventoryDetail",
                "UV_HienThiTongKho",
                "UV_HienThiLuuTru",
                "UV_HienThiTongSanPham"
            });
            _invViewCombo.SelectedIndex = 0;
            var btn = new Button { Text = "Tải dữ liệu", AutoSize = true };
            btn.Click += (_, _) => LoadInventoryReport();
            top.Controls.Add(_invViewCombo);
            top.Controls.Add(btn);

            _invReportGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
            };

            tabPage9.Padding = new Padding(6);
            tabPage9.Controls.Add(top);
            tabPage9.Controls.Add(_invReportGrid);

            UiTheme.StyleCombo(_invViewCombo);
            UiTheme.StyleDataGridView(_invReportGrid);
            if (top.Controls.Count > 1 && top.Controls[1] is Button loadBtn)
                UiTheme.StyleButtonPrimary(loadBtn);
        }

        private void LoadInventoryReport()
        {
            try
            {
                DataTable dt = _invViewCombo.SelectedIndex switch
                {
                    0 => InventoryDAL.Instance.ViewInventoryDetail(),
                    1 => InventoryDAL.Instance.ViewTotalByWarehouse(),
                    2 => InventoryDAL.Instance.ViewTotalByLocation(),
                    _ => InventoryDAL.Instance.ViewTotalBySku()
                };
                _invReportGrid.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WireEvents()
        {
            // Danh mục
            button3.Click += (_, _) => CategoryAdd();
            button4.Click += (_, _) => CategoryDelete();
            button5.Click += (_, _) => CategoryUpdate();
            button6.Click += (_, _) => CategoryView();
            button7.Click += (_, _) => CategorySearch();

            // Sản phẩm
            button8.Click += (_, _) => ProductAdd();
            button9.Click += (_, _) => ProductDelete();
            button10.Click += (_, _) => ProductUpdate();
            button11.Click += (_, _) => ProductView();
            button12.Click += (_, _) => ProductSearch();

            // SKU
            button17.Click += (_, _) => SkuAdd();
            button16.Click += (_, _) => SkuDelete();
            button15.Click += (_, _) => SkuUpdate();
            button14.Click += (_, _) => SkuView();
            button13.Click += (_, _) => SkuSearch();

            // Kho hàng
            button22.Click += (_, _) => WarehouseAdd();
            button21.Click += (_, _) => WarehouseDelete();
            button20.Click += (_, _) => WarehouseUpdate();
            button19.Click += (_, _) => WarehouseView();
            button18.Click += (_, _) => WarehouseSearch();

            // Vị trí kho
            button27.Click += (_, _) => LocationAdd();
            button26.Click += (_, _) => LocationDelete();
            button25.Click += (_, _) => LocationUpdate();
            button24.Click += (_, _) => LocationView();
            button23.Click += (_, _) => LocationSearch();

            // Nhà cung cấp
            button32.Click += (_, _) => SupplierAdd();
            button31.Click += (_, _) => SupplierDelete();
            button30.Click += (_, _) => SupplierUpdate();
            button29.Click += (_, _) => SupplierView();
            button28.Click += (_, _) => SupplierSearch();

            // Xuất kho
            button1.Click += (_, _) => ExportStats();
            button34.Click += (_, _) => ExportAddLine();
            button37.Click += (_, _) => ExportSave();

            // Nhập kho
            button2.Click += (_, _) => ImportStats();
            button36.Click += (_, _) => ImportAddLine();
            button38.Click += (_, _) => ImportSave();
        }

        private void ReloadCurrentTab()
        {
            try
            {
                switch (tabControl1.SelectedIndex)
                {
                    case 0: LoadCategoryGrid(); break;
                    case 1: LoadProductGrid(); LoadCategoryCombo(); break;
                    case 2: LoadSkuGrid(); LoadProductComboForSku(); break;
                    case 3: LoadWarehouseGrid(); break;
                    case 4: LoadLocationGrid(); LoadWarehouseComboForLocation(); break;
                    case 5: LoadSupplierGrid(); break;
                    case 6: LoadExportReceipts(); LoadSkuLocationCombosExport(); break;
                    case 7: LoadImportReceipts(); LoadImportCombos(); break;
                    case 8: LoadInventoryReport(); break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Helpers

        private static DataRowView? SelectedRow(DataGridView dgv) =>
            dgv.CurrentRow?.DataBoundItem as DataRowView;

        private static void Bind(DataGridView dgv, DataTable dt)
        {
            dgv.AutoGenerateColumns = true;
            dgv.DataSource = dt;
        }

        private static int? ParseId(TextBox tb)
        {
            if (string.IsNullOrWhiteSpace(tb.Text)) return null;
            return int.TryParse(tb.Text.Trim(), out var id) ? id : null;
        }

        private static int ParseIntRequired(TextBox tb, string label)
        {
            if (!int.TryParse(tb.Text.Trim(), out var v))
                throw new InvalidOperationException($"Nhập số nguyên hợp lệ cho {label}.");
            return v;
        }

        private static DataTable CategoryComboWithBlank()
        {
            var src = CategoryDAL.Instance.List();
            var t = src.Copy();
            var r = t.NewRow();
            r["category_id"] = -1;
            r["category_name"] = "(Không chọn)";
            t.Rows.InsertAt(r, 0);
            return t;
        }

        private static DataTable WithLabel(DataTable src, string idCol, string textCol, string labelCol)
        {
            var t = src.Copy();
            if (!t.Columns.Contains(labelCol))
                t.Columns.Add(labelCol, typeof(string));
            foreach (DataRow row in t.Rows)
                row[labelCol] = $"{row[textCol]} [#{row[idCol]}]";
            return t;
        }

        private void LoadCategoryCombo()
        {
            comboBox1.DataSource = CategoryComboWithBlank();
            comboBox1.DisplayMember = "category_name";
            comboBox1.ValueMember = "category_id";
        }

        private int? SelectedCategoryId()
        {
            if (comboBox1.SelectedValue == null || comboBox1.SelectedValue == DBNull.Value)
                return null;
            var v = Convert.ToInt32(comboBox1.SelectedValue);
            return v < 0 ? null : v;
        }

        private void LoadProductComboForSku()
        {
            var dt = WithLabel(ProductDAL.Instance.List(), "product_id", "product_name", "lbl");
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "lbl";
            comboBox2.ValueMember = "product_id";
        }

        private void LoadWarehouseComboForLocation()
        {
            var dt = WithLabel(WarehouseDAL.Instance.List(), "warehouse_id", "warehouse_name", "lbl");
            comboBox4.DataSource = dt;
            comboBox4.DisplayMember = "lbl";
            comboBox4.ValueMember = "warehouse_id";
        }

        private void LoadSkuLocationCombosExport()
        {
            var sku = WithLabel(SkuDAL.Instance.List(), "sku_id", "sku_code", "lbl");
            comboBox6.DataSource = sku;
            comboBox6.DisplayMember = "lbl";
            comboBox6.ValueMember = "sku_id";

            var loc = WithLabel(StorageLocationDAL.Instance.ListAsTable(), "location_id", "location_description", "lbl");
            comboBox7.DataSource = loc;
            comboBox7.DisplayMember = "lbl";
            comboBox7.ValueMember = "location_id";
        }

        private void LoadImportCombos()
        {
            var sku = WithLabel(SkuDAL.Instance.List(), "sku_id", "sku_code", "lbl");
            comboBox9.DataSource = sku;
            comboBox9.DisplayMember = "lbl";
            comboBox9.ValueMember = "sku_id";

            var sup = WithLabel(SupplierDAL.Instance.List(), "supplier_id", "supplier_name", "lbl");
            comboBox10.DataSource = sup;
            comboBox10.DisplayMember = "lbl";
            comboBox10.ValueMember = "supplier_id";

            var loc = WithLabel(StorageLocationDAL.Instance.ListAsTable(), "location_id", "location_description", "lbl");
            comboBox11.DataSource = loc;
            comboBox11.DisplayMember = "lbl";
            comboBox11.ValueMember = "location_id";
        }

        #endregion

        #region Danh mục

        private void LoadCategoryGrid() => Bind(dataGridView3, CategoryDAL.Instance.List());

        private void CategorySearch()
        {
            var q = textBox1.Text.Trim();
            Bind(dataGridView3, string.IsNullOrEmpty(q) ? CategoryDAL.Instance.List() : CategoryDAL.Instance.SearchByName(q));
        }

        private void CategoryView()
        {
            var drv = SelectedRow(dataGridView3);
            if (drv == null) return;
            textBox2.Text = drv.Row["category_id"].ToString();
            textBox3.Text = drv.Row["category_name"].ToString();
        }

        private void CategoryAdd()
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text)) { MessageBox.Show("Nhập tên danh mục."); return; }
            CategoryDAL.Instance.Insert(textBox3.Text.Trim());
            LoadCategoryGrid();
        }

        private void CategoryUpdate()
        {
            var id = ParseId(textBox2);
            if (id == null) { MessageBox.Show("Chọn ID (Xem) trước."); return; }
            if (string.IsNullOrWhiteSpace(textBox3.Text)) { MessageBox.Show("Nhập tên danh mục."); return; }
            CategoryDAL.Instance.Update(id.Value, textBox3.Text.Trim());
            LoadCategoryGrid();
        }

        private void CategoryDelete()
        {
            var id = ParseId(textBox2);
            if (id == null) { MessageBox.Show("Chọn ID (Xem) trước."); return; }
            if (MessageBox.Show("Xóa danh mục này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            CategoryDAL.Instance.Delete(id.Value);
            LoadCategoryGrid();
        }

        #endregion

        #region Sản phẩm

        private void LoadProductGrid() => Bind(dataGridView4, ProductDAL.Instance.List());

        private void ProductSearch()
        {
            var q = textBox4.Text.Trim();
            Bind(dataGridView4, string.IsNullOrEmpty(q) ? ProductDAL.Instance.List() : ProductDAL.Instance.SearchByName(q));
        }

        private void ProductView()
        {
            var drv = SelectedRow(dataGridView4);
            if (drv == null) return;
            textBox5.Text = drv.Row["product_id"].ToString();
            textBox6.Text = drv.Row["product_name"].ToString();
            textBox7.Text = drv.Row["description"] == DBNull.Value ? "" : drv.Row["description"].ToString();
            var cat = drv.Row["category_id"] == DBNull.Value ? -1 : Convert.ToInt32(drv.Row["category_id"]);
            LoadCategoryCombo();
            comboBox1.SelectedValue = cat;
        }

        private void ProductAdd()
        {
            if (string.IsNullOrWhiteSpace(textBox6.Text)) { MessageBox.Show("Nhập tên sản phẩm."); return; }
            ProductDAL.Instance.Insert(textBox6.Text.Trim(), textBox7.Text.Trim(), SelectedCategoryId());
            LoadProductGrid();
        }

        private void ProductUpdate()
        {
            var id = ParseId(textBox5);
            if (id == null) { MessageBox.Show("Chọn ID (Xem) trước."); return; }
            ProductDAL.Instance.Update(id.Value, textBox6.Text.Trim(), textBox7.Text.Trim(), SelectedCategoryId());
            LoadProductGrid();
        }

        private void ProductDelete()
        {
            var id = ParseId(textBox5);
            if (id == null) { MessageBox.Show("Chọn ID (Xem) trước."); return; }
            if (MessageBox.Show("Xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            ProductDAL.Instance.Delete(id.Value);
            LoadProductGrid();
        }

        #endregion

        #region SKU

        private void LoadSkuGrid() => Bind(dataGridView5, SkuDAL.Instance.List());

        private void SkuSearch()
        {
            var q = textBox8.Text.Trim();
            Bind(dataGridView5, string.IsNullOrEmpty(q) ? SkuDAL.Instance.List() : SkuDAL.Instance.SearchByCode(q));
        }

        private void SkuView()
        {
            var drv = SelectedRow(dataGridView5);
            if (drv == null) return;
            textBox11.Text = drv.Row["sku_id"].ToString();
            textBox10.Text = drv.Row["sku_code"].ToString();
            textBox9.Text = drv.Row["unit"] == DBNull.Value ? "" : drv.Row["unit"].ToString();
            LoadProductComboForSku();
            comboBox2.SelectedValue = Convert.ToInt32(drv.Row["product_id"]);
        }

        private void SkuAdd()
        {
            if (string.IsNullOrWhiteSpace(textBox10.Text)) { MessageBox.Show("Nhập SKU code."); return; }
            if (comboBox2.SelectedValue == null) { MessageBox.Show("Chọn sản phẩm."); return; }
            SkuDAL.Instance.Insert(textBox10.Text.Trim(), textBox9.Text.Trim(), Convert.ToInt32(comboBox2.SelectedValue));
            LoadSkuGrid();
        }

        private void SkuUpdate()
        {
            var id = ParseId(textBox11);
            if (id == null) { MessageBox.Show("Chọn ID (Xem) trước."); return; }
            int? pid = comboBox2.SelectedValue == null ? null : Convert.ToInt32(comboBox2.SelectedValue);
            SkuDAL.Instance.Update(id.Value, textBox10.Text.Trim(), textBox9.Text.Trim(), pid);
            LoadSkuGrid();
        }

        private void SkuDelete()
        {
            var id = ParseId(textBox11);
            if (id == null) { MessageBox.Show("Chọn ID (Xem) trước."); return; }
            if (MessageBox.Show("Xóa SKU này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            SkuDAL.Instance.Delete(id.Value);
            LoadSkuGrid();
        }

        #endregion

        #region Kho hàng

        private void LoadWarehouseGrid() => Bind(dataGridView6, WarehouseDAL.Instance.List());

        private void WarehouseSearch()
        {
            var q = textBox12.Text.Trim();
            Bind(dataGridView6, string.IsNullOrEmpty(q) ? WarehouseDAL.Instance.List() : WarehouseDAL.Instance.SearchByName(q));
        }

        private void WarehouseView()
        {
            var drv = SelectedRow(dataGridView6);
            if (drv == null) return;
            textBox15.Text = drv.Row["warehouse_id"].ToString();
            textBox14.Text = drv.Row["warehouse_name"].ToString();
            textBox13.Text = drv.Row["address"] == DBNull.Value ? "" : drv.Row["address"].ToString();
            textBox24.Text = drv.Row["max_capacity"].ToString();
        }

        private void WarehouseAdd()
        {
            if (string.IsNullOrWhiteSpace(textBox14.Text)) { MessageBox.Show("Nhập tên kho."); return; }
            var cap = ParseIntRequired(textBox24, "sức chứa");
            WarehouseDAL.Instance.Insert(textBox14.Text.Trim(), textBox13.Text.Trim(), cap);
            LoadWarehouseGrid();
        }

        private void WarehouseUpdate()
        {
            var id = ParseId(textBox15);
            if (id == null) { MessageBox.Show("Chọn ID (Xem) trước."); return; }
            int? cap = string.IsNullOrWhiteSpace(textBox24.Text) ? null : ParseIntRequired(textBox24, "sức chứa");
            WarehouseDAL.Instance.Update(id.Value, textBox14.Text.Trim(), textBox13.Text.Trim(), cap);
            LoadWarehouseGrid();
        }

        private void WarehouseDelete()
        {
            var id = ParseId(textBox15);
            if (id == null) { MessageBox.Show("Chọn ID (Xem) trước."); return; }
            if (MessageBox.Show("Xóa kho này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            WarehouseDAL.Instance.Delete(id.Value);
            LoadWarehouseGrid();
        }

        #endregion

        #region Vị trí kho

        private void LoadLocationGrid() => Bind(dataGridView7, StorageLocationDAL.Instance.ListAsTable());

        private void LocationSearch()
        {
            var q = textBox16.Text.Trim();
            Bind(dataGridView7, string.IsNullOrEmpty(q)
                ? StorageLocationDAL.Instance.ListAsTable()
                : StorageLocationDAL.Instance.SearchByDescription(q));
        }

        private void LocationView()
        {
            var drv = SelectedRow(dataGridView7);
            if (drv == null) return;
            textBox19.Text = drv.Row["location_id"].ToString();
            textBox17.Text = drv.Row["location_description"] == DBNull.Value ? "" : drv.Row["location_description"].ToString();
            textBox18.Text = drv.Row["capacity"].ToString();
            LoadWarehouseComboForLocation();
            comboBox4.SelectedValue = Convert.ToInt32(drv.Row["warehouse_id"]);
        }

        private void LocationAdd()
        {
            if (comboBox4.SelectedValue == null) { MessageBox.Show("Chọn kho."); return; }
            var cap = ParseIntRequired(textBox18, "sức chứa");
            StorageLocationDAL.Instance.Insert(textBox17.Text.Trim(), cap, Convert.ToInt32(comboBox4.SelectedValue));
            LoadLocationGrid();
        }

        private void LocationUpdate()
        {
            var id = ParseId(textBox19);
            if (id == null) { MessageBox.Show("Chọn ID (Xem) trước."); return; }
            int? cap = string.IsNullOrWhiteSpace(textBox18.Text) ? null : ParseIntRequired(textBox18, "sức chứa");
            int? wid = comboBox4.SelectedValue == null ? null : Convert.ToInt32(comboBox4.SelectedValue);
            StorageLocationDAL.Instance.Update(id.Value, textBox17.Text.Trim(), cap, wid);
            LoadLocationGrid();
        }

        private void LocationDelete()
        {
            var id = ParseId(textBox19);
            if (id == null) { MessageBox.Show("Chọn ID (Xem) trước."); return; }
            if (MessageBox.Show("Xóa vị trí này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            StorageLocationDAL.Instance.Delete(id.Value);
            LoadLocationGrid();
        }

        #endregion

        #region Nhà cung cấp

        private void LoadSupplierGrid() => Bind(dataGridView8, SupplierDAL.Instance.List());

        private void SupplierSearch()
        {
            var q = textBox20.Text.Trim();
            Bind(dataGridView8, string.IsNullOrEmpty(q) ? SupplierDAL.Instance.List() : SupplierDAL.Instance.SearchByName(q));
        }

        private void SupplierView()
        {
            var drv = SelectedRow(dataGridView8);
            if (drv == null) return;
            textBox23.Text = drv.Row["supplier_id"].ToString();
            textBox22.Text = drv.Row["supplier_name"].ToString();
            textBox21.Text = drv.Row["address"] == DBNull.Value ? "" : drv.Row["address"].ToString();
            textBox25.Text = drv.Row["email"] == DBNull.Value ? "" : drv.Row["email"].ToString();
            textBox26.Text = drv.Row["phone"] == DBNull.Value ? "" : drv.Row["phone"].ToString();
        }

        private void SupplierAdd()
        {
            if (string.IsNullOrWhiteSpace(textBox22.Text)) { MessageBox.Show("Nhập tên nhà cung cấp."); return; }
            SupplierDAL.Instance.Insert(textBox22.Text.Trim(), textBox21.Text.Trim(), textBox25.Text.Trim(), textBox26.Text.Trim());
            LoadSupplierGrid();
        }

        private void SupplierUpdate()
        {
            var id = ParseId(textBox23);
            if (id == null) { MessageBox.Show("Chọn ID (Xem) trước."); return; }
            if (string.IsNullOrWhiteSpace(textBox22.Text)) { MessageBox.Show("Nhập tên nhà cung cấp."); return; }
            SupplierDAL.Instance.Update(id.Value, textBox22.Text.Trim(), textBox21.Text.Trim(), textBox25.Text.Trim(), textBox26.Text.Trim());
            LoadSupplierGrid();
        }

        private void SupplierDelete()
        {
            var id = ParseId(textBox23);
            if (id == null) { MessageBox.Show("Chọn ID (Xem) trước."); return; }
            if (MessageBox.Show("Xóa nhà cung cấp này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            SupplierDAL.Instance.Delete(id.Value);
            LoadSupplierGrid();
        }

        #endregion

        #region Xuất kho

        private void LoadExportReceipts() =>
            Bind(dataGridView1, ExportReceiptDAL.Instance.GetByDateRange(dateTimePicker1.Value, dateTimePicker2.Value));

        private void ExportStats() => LoadExportReceipts();

        private void ExportAddLine()
        {
            if (comboBox6.SelectedValue == null || comboBox7.SelectedValue == null)
            {
                MessageBox.Show("Chọn SKU và vị trí.");
                return;
            }

            var qty = ParseIntRequired(textBox27, "số lượng");
            if (qty <= 0) { MessageBox.Show("Số lượng phải > 0."); return; }

            var skuId = Convert.ToInt32(comboBox6.SelectedValue);
            var locId = Convert.ToInt32(comboBox7.SelectedValue);
            var skuCode = ((DataRowView)comboBox6.SelectedItem).Row["sku_code"].ToString()!;
            var locText = ((DataRowView)comboBox7.SelectedItem).Row["location_description"] == DBNull.Value
                ? $"#{locId}"
                : ((DataRowView)comboBox7.SelectedItem).Row["location_description"].ToString()!;

            _exportLines.Rows.Add(skuId, locId, skuCode, locText, qty);
        }

        private void ExportSave()
        {
            if (_exportLines.Rows.Count == 0)
            {
                MessageBox.Show("Thêm ít nhất một dòng chi tiết.");
                return;
            }

            try
            {
                var purpose = textBoxPurpose.Text.Trim();
                var exportId = ExportReceiptDAL.Instance.InsertHeader(string.IsNullOrEmpty(purpose) ? null : purpose);
                foreach (DataRow r in _exportLines.Rows)
                {
                    ExportReceiptDAL.Instance.InsertDetail(
                        exportId,
                        Convert.ToInt32(r["sku_id"]),
                        Convert.ToInt32(r["location_id"]),
                        Convert.ToInt32(r["quantity"]));
                }

                _exportLines.Rows.Clear();
                textBoxPurpose.Clear();
                LoadExportReceipts();
                MessageBox.Show("Đã lưu phiếu xuất.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Nhập kho

        private void LoadImportReceipts() =>
            Bind(dataGridView2, ImportReceiptDAL.Instance.GetByDateRange(dateTimePicker3.Value, dateTimePicker4.Value));

        private void ImportStats() => LoadImportReceipts();

        private void ImportAddLine()
        {
            if (comboBox9.SelectedValue == null || comboBox10.SelectedValue == null || comboBox11.SelectedValue == null)
            {
                MessageBox.Show("Chọn SKU, nhà cung cấp và vị trí.");
                return;
            }

            var qty = ParseIntRequired(textBox28, "số lượng");
            if (qty <= 0) { MessageBox.Show("Số lượng phải > 0."); return; }

            var skuId = Convert.ToInt32(comboBox9.SelectedValue);
            var supId = Convert.ToInt32(comboBox10.SelectedValue);
            var locId = Convert.ToInt32(comboBox11.SelectedValue);
            var skuCode = ((DataRowView)comboBox9.SelectedItem).Row["sku_code"].ToString()!;
            var supName = ((DataRowView)comboBox10.SelectedItem).Row["supplier_name"].ToString()!;
            var locText = ((DataRowView)comboBox11.SelectedItem).Row["location_description"] == DBNull.Value
                ? $"#{locId}"
                : ((DataRowView)comboBox11.SelectedItem).Row["location_description"].ToString()!;

            _importLines.Rows.Add(skuId, supId, locId, skuCode, supName, locText, qty);
        }

        private void ImportSave()
        {
            if (_importLines.Rows.Count == 0)
            {
                MessageBox.Show("Thêm ít nhất một dòng chi tiết.");
                return;
            }

            try
            {
                var importId = ImportReceiptDAL.Instance.InsertHeader();
                foreach (DataRow r in _importLines.Rows)
                {
                    ImportReceiptDAL.Instance.InsertDetail(
                        importId,
                        Convert.ToInt32(r["sku_id"]),
                        Convert.ToInt32(r["supplier_id"]),
                        Convert.ToInt32(r["location_id"]),
                        Convert.ToInt32(r["quantity"]));
                }

                _importLines.Rows.Clear();
                LoadImportReceipts();
                MessageBox.Show("Đã lưu phiếu nhập.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
