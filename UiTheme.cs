using System.Drawing;
using System.Windows.Forms;

namespace QuanLyKho
{
    /// <summary>
    /// Giao diện tối lấy cảm hứng từ Uiverse (card #24233b, vùng nội dung #49465c, accent #00ca4e).
    /// Không dùng icon; tối ưu tương phản để bảng / ô nhập không “biến mất”.
    /// </summary>
    internal static class UiTheme
    {
        public static readonly Font BaseFont = new("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font BaseFontBold = new("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
        public static readonly Font TitleFont = new("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
        public static readonly Font SectionFont = new("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point);

        // —— Palette (từ CSS bạn gửi) ——
        public static Color BgApp { get; } = Color.FromArgb(22, 21, 36);
        public static Color BgSurface { get; } = Color.FromArgb(36, 35, 59);
        public static Color BgCode { get; } = Color.FromArgb(73, 70, 92);
        public static Color BgMuted { get; } = Color.FromArgb(30, 29, 48);
        public static Color TextPrimary { get; } = Color.FromArgb(248, 250, 252);
        public static Color TextMuted { get; } = Color.FromArgb(163, 173, 190);
        public static Color Accent { get; } = Color.FromArgb(0, 202, 78);
        public static Color AccentDark { get; } = Color.FromArgb(0, 160, 62);
        public static Color Success { get; } = Color.FromArgb(0, 202, 78);
        public static Color SuccessDark { get; } = Color.FromArgb(0, 140, 58);
        public static Color Danger { get; } = Color.FromArgb(255, 96, 92);
        public static Color DangerDark { get; } = Color.FromArgb(220, 70, 70);
        public static Color Warning { get; } = Color.FromArgb(255, 189, 68);
        public static Color Border { get; } = Color.FromArgb(90, 87, 118);
        public static Color HeaderBar { get; } = Color.FromArgb(26, 25, 40);
        public static Color GridAltRow { get; } = Color.FromArgb(65, 62, 88);
        public static Color SelectionBg { get; } = Color.FromArgb(72, 99, 156);

        public static Control? FindControl(Control parent, string name)
        {
            if (parent.Name == name)
                return parent;
            foreach (Control c in parent.Controls)
            {
                var r = FindControl(c, name);
                if (r != null) return r;
            }
            return null;
        }

        /// <summary>Chỉ màu / font / thẻ đăng nhập. Bố cục do FormLogin tính lại khi Load và Resize.</summary>
        public static void StyleLoginForm(Form f)
        {
            f.Font = BaseFont;
            f.BackColor = BgApp;
            f.ForeColor = TextPrimary;
            if (FindControl(f, "panel1") is Panel p1)
                StyleCardChrome(p1);

            if (FindControl(f, "panel2") is Panel p2)
                StyleCardChrome(p2);

            if (FindControl(f, "label1") is Label l1)
            {
                l1.Font = SectionFont;
                l1.ForeColor = TextMuted;
            }

            if (FindControl(f, "label2") is Label l2)
            {
                l2.Font = SectionFont;
                l2.ForeColor = TextMuted;
            }

            if (FindControl(f, "textBox1") is TextBox tb1)
            {
                StyleTextBox(tb1);
                tb1.Height = 34;
            }

            if (FindControl(f, "textBox2") is TextBox tb2)
            {
                StyleTextBox(tb2);
                tb2.Height = 34;
            }

            if (FindControl(f, "button1") is Button b1)
                StyleButtonPrimary(b1);

            if (FindControl(f, "button2") is Button b2)
                StyleButtonGhost(b2);
        }

        /// <summary>Thẻ: nền surface + viền + bóng nhẹ (mô phỏng box-shadow).</summary>
        private static void StyleCardChrome(Panel p)
        {
            p.BackColor = BgSurface;
            p.Padding = new Padding(18, 14, 18, 14);
            p.Paint -= CardPanelPaint;
            p.Paint += CardPanelPaint;
        }

        private static void CardPanelPaint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel p) return;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);
            using var pen = new Pen(Border, 1);
            g.DrawRectangle(pen, r);
            using var sh = new Pen(Color.FromArgb(40, 0, 0, 0), 1);
            g.DrawLine(sh, 2, p.Height - 2, p.Width - 2, p.Height - 2);
        }

        public static void StyleHomeForm(Form f)
        {
            f.Font = BaseFont;
            f.BackColor = BgApp;
            f.ForeColor = TextPrimary;

            if (FindControl(f, "menuStrip2") is MenuStrip ms)
            {
                ms.BackColor = HeaderBar;
                ms.ForeColor = Color.White;
                ms.RenderMode = ToolStripRenderMode.System;
                foreach (ToolStripItem it in ms.Items)
                {
                    it.ForeColor = Color.White;
                    it.Font = BaseFontBold;
                }
            }

            if (FindControl(f, "flowLayoutPanel3") is FlowLayoutPanel flp)
            {
                flp.BackColor = BgMuted;
                flp.Padding = new Padding(14, 14, 14, 14);
                flp.WrapContents = true;
                flp.AutoScroll = true;
            }

            if (FindControl(f, "panel2") is Panel p2)
            {
                p2.BackColor = BgSurface;
                p2.Padding = new Padding(6);
                p2.Paint -= CardPanelPaint;
                p2.Paint += CardPanelPaint;
            }

            if (FindControl(f, "listView1") is ListView lv)
            {
                lv.Font = BaseFont;
                lv.BorderStyle = BorderStyle.FixedSingle;
                lv.BackColor = BgCode;
                lv.ForeColor = TextPrimary;
                lv.OwnerDraw = false;
            }

            if (FindControl(f, "flowLayoutPanel1") is FlowLayoutPanel flp1)
            {
                flp1.BackColor = BgApp;
                flp1.Padding = new Padding(10, 6, 10, 6);
            }

            if (FindControl(f, "button2") is Button b2)
            {
                StyleButtonSecondary(b2);
                b2.Height = 48;
            }

            if (FindControl(f, "button4") is Button b4)
            {
                StyleButtonSecondary(b4);
                b4.Height = 48;
            }

            ApplyRecursiveSkipRoots(f, "menuStrip2", "flowLayoutPanel3");
        }

        public static void StyleLocationCardButton(Button btn, int warehouseId)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Border;
            btn.Cursor = Cursors.Hand;
            btn.Font = BaseFontBold;
            btn.ForeColor = TextPrimary;
            btn.UseVisualStyleBackColor = false;
            btn.Size = new Size(188, 168);
            btn.Padding = new Padding(10, 8, 10, 8);
            btn.Margin = new Padding(6);

            Color bg = warehouseId switch
            {
                1 => Color.FromArgb(48, 52, 82),
                2 => Color.FromArgb(44, 62, 58),
                3 => Color.FromArgb(62, 56, 48),
                _ => BgCode
            };
            btn.BackColor = bg;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(bg);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Light(ControlPaint.Light(bg));
        }

        public static void StyleAdminForm(Form f)
        {
            f.Font = BaseFont;
            f.BackColor = BgApp;
            f.ForeColor = TextPrimary;
            ApplyRecursiveSkipRoots(f);
        }

        private static void ApplyRecursiveSkipRoots(Control root, params string[] skipRootNames)
        {
            var skip = skipRootNames.Length == 0 ? null : new HashSet<string>(skipRootNames, StringComparer.OrdinalIgnoreCase);
            foreach (Control c in root.Controls)
            {
                if (skip != null && skip.Contains(c.Name))
                    continue;
                StyleControl(c);
                if (skip != null && skip.Contains(c.Name))
                    continue;
                ApplyRecursiveSkipRoots(c, skipRootNames);
            }
        }

        private static void StyleControl(Control c)
        {
            switch (c)
            {
                case Form form:
                    form.Font = BaseFont;
                    form.BackColor = BgApp;
                    form.ForeColor = TextPrimary;
                    break;
                case TabControl tc:
                    tc.Font = BaseFontBold;
                    tc.Padding = new Point(14, 8);
                    tc.BackColor = BgMuted;
                    break;
                case TabPage tp:
                    tp.BackColor = BgSurface;
                    tp.Padding = new Padding(12, 12, 12, 12);
                    tp.UseVisualStyleBackColor = false;
                    break;
                case FlowLayoutPanel flp:
                    flp.BackColor = Color.FromArgb(40, 39, 62);
                    flp.WrapContents = true;
                    flp.AutoScroll = true;
                    break;
                case Panel p:
                    if (p.Parent is TabPage)
                    {
                        p.BackColor = BgCode;
                        p.Padding = new Padding(10, 8, 10, 8);
                        p.AutoScroll = true;
                        p.Paint -= InsetPanelPaint;
                        p.Paint += InsetPanelPaint;
                    }
                    else if (p.Name.StartsWith("panel", StringComparison.Ordinal))
                    {
                        p.BackColor = BgSurface;
                    }
                    break;
                case Button b:
                    StyleButtonByText(b);
                    break;
                case TextBox tb:
                    StyleTextBox(tb, tb.Width);
                    break;
                case ComboBox cb:
                    cb.FlatStyle = FlatStyle.Flat;
                    cb.BackColor = BgCode;
                    cb.ForeColor = TextPrimary;
                    cb.Font = BaseFont;
                    break;
                case Label lb:
                    if (lb.Font.Size >= 14F)
                    {
                        lb.ForeColor = TextPrimary;
                        lb.Font = SectionFont;
                    }
                    else if (lb.Text is "SOẠN PHIẾU XUẤT MỚI" or "SOẠN PHIẾU NHẬP MỚI")
                    {
                        lb.ForeColor = TextPrimary;
                        lb.Font = TitleFont;
                    }
                    else
                    {
                        lb.ForeColor = TextMuted;
                        lb.Font = BaseFontBold;
                    }
                    break;
                case DataGridView dgv:
                    StyleDataGridView(dgv);
                    break;
                case DateTimePicker dtp:
                    dtp.Font = BaseFont;
                    dtp.BackColor = BgCode;
                    dtp.ForeColor = TextPrimary;
                    dtp.CalendarForeColor = TextPrimary;
                    dtp.CalendarMonthBackground = BgCode;
                    dtp.CalendarTitleBackColor = BgSurface;
                    dtp.CalendarTitleForeColor = TextPrimary;
                    break;
                case MenuStrip ms:
                    ms.BackColor = HeaderBar;
                    ms.ForeColor = Color.White;
                    foreach (ToolStripItem it in ms.Items)
                        it.ForeColor = Color.White;
                    break;
            }
        }

        private static void InsetPanelPaint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel p) return;
            var r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);
            using var pen = new Pen(Color.FromArgb(55, 52, 75), 1);
            e.Graphics.DrawRectangle(pen, r);
        }

        private static void StyleButtonByText(Button b)
        {
            b.UseVisualStyleBackColor = false;
            b.FlatStyle = FlatStyle.Flat;
            b.Cursor = Cursors.Hand;
            b.Font = BaseFontBold;
            b.FlatAppearance.BorderSize = 0;

            var t = b.Text.Trim();
            if (t.Contains("LƯU", StringComparison.Ordinal))
            {
                StyleButtonSuccess(b);
                return;
            }

            if (t is "Xóa")
            {
                StyleButtonDanger(b);
                return;
            }

            if (t is "Thêm")
            {
                StyleButtonPrimary(b);
                return;
            }

            if (t is "Sửa")
            {
                b.BackColor = Warning;
                b.ForeColor = Color.FromArgb(30, 25, 20);
                b.FlatAppearance.MouseOverBackColor = ControlPaint.Light(Warning);
                return;
            }

            if (t is "Xem")
            {
                StyleButtonGhost(b);
                return;
            }

            if (t.Contains("Tìm", StringComparison.Ordinal) || t is "Thống kê" || t.Contains("Thêm món", StringComparison.Ordinal))
            {
                StyleButtonPrimary(b);
                return;
            }

            if (t is "Tải dữ liệu")
            {
                StyleButtonPrimary(b);
                return;
            }

            StyleButtonSecondary(b);
        }

        public static void StyleButtonPrimary(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Accent;
            b.ForeColor = Color.White;
            b.FlatAppearance.MouseOverBackColor = AccentDark;
            b.FlatAppearance.MouseDownBackColor = SuccessDark;
        }

        public static void StyleButtonSecondary(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Border;
            b.BackColor = BgSurface;
            b.ForeColor = TextPrimary;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 48, 78);
        }

        public static void StyleButtonGhost(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Border;
            b.BackColor = Color.FromArgb(48, 46, 72);
            b.ForeColor = TextMuted;
            b.FlatAppearance.MouseOverBackColor = BgCode;
        }

        public static void StyleButtonSuccess(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Success;
            b.ForeColor = Color.White;
            b.FlatAppearance.MouseOverBackColor = SuccessDark;
        }

        public static void StyleButtonDanger(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Danger;
            b.ForeColor = Color.White;
            b.FlatAppearance.MouseOverBackColor = DangerDark;
        }

        public static void StyleTextBox(TextBox tb, int? width = null)
        {
            tb.BorderStyle = BorderStyle.FixedSingle;
            tb.BackColor = BgCode;
            tb.ForeColor = TextPrimary;
            tb.Font = BaseFont;
            if (width.HasValue)
                tb.Width = width.Value;
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.BackgroundColor = BgCode;
            dgv.GridColor = Border;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowHeadersVisible = false;
            dgv.Font = BaseFont;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.ColumnHeadersHeight = 38;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ScrollBars = ScrollBars.Both;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = BgSurface;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextPrimary;
            dgv.ColumnHeadersDefaultCellStyle.Font = BaseFontBold;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = BgSurface;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextPrimary;
            dgv.DefaultCellStyle.BackColor = BgCode;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = SelectionBg;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = GridAltRow;
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = TextPrimary;
            dgv.RowTemplate.Height = 30;
            dgv.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
        }

        public static void StyleCombo(ComboBox cb)
        {
            cb.FlatStyle = FlatStyle.Flat;
            cb.BackColor = BgCode;
            cb.ForeColor = TextPrimary;
            cb.Font = BaseFont;
        }
    }
}
