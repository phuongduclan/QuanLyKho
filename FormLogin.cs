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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            UiTheme.StyleLoginForm(this);
            Shown += (_, _) => ArrangeLoginControls();
            Resize += (_, _) => ArrangeLoginControls();
            ArrangeLoginControls();
        }

        /// <summary>Bố cục responsive: thẻ full chiều ngang, ô nhập kéo theo, nút neo góc dưới phải.</summary>
        private void ArrangeLoginControls()
        {
            const int pad = 28;
            const int gap = 14;
            const int titleBand = 44;
            const int cardH = 108;
            const int bottomPad = 22;
            const int btnW = 168;
            const int btnH = 44;
            const int btnGap = 12;

            int w = Math.Max(280, ClientSize.Width - 2 * pad);
            panel1.SetBounds(pad, titleBand, w, cardH);
            panel2.SetBounds(pad, panel1.Bottom + gap, w, cardH);

            LayoutCardRow(panel1, label1, textBox1);
            LayoutCardRow(panel2, label2, textBox2);

            button1.Size = new Size(btnW, btnH);
            button2.Size = new Size(btnW, btnH);
            int yBtn = ClientSize.Height - bottomPad - btnH;
            if (yBtn < panel2.Bottom + gap)
                yBtn = panel2.Bottom + gap;

            button2.SetBounds(ClientSize.Width - pad - btnW, yBtn, btnW, btnH);
            button1.SetBounds(button2.Left - btnGap - btnW, yBtn, btnW, btnH);
        }

        private static void LayoutCardRow(Panel card, Label lbl, TextBox tb)
        {
            const int labelColMin = 148;
            int pl = card.Padding.Left;
            int pr = card.Padding.Right;
            int pt = card.Padding.Top;
            int pb = card.Padding.Bottom;

            lbl.AutoSize = true;
            lbl.Location = new Point(pl, pt + (card.ClientSize.Height - pt - pb - lbl.PreferredHeight) / 2);

            int labelW = Math.Max(labelColMin, lbl.PreferredWidth);
            int tbLeft = pl + labelW + 14;
            const int tbH = 34;
            int tbTop = pt + (card.ClientSize.Height - pt - pb - tbH) / 2;
            int tbW = Math.Max(160, card.ClientSize.Width - tbLeft - pr);
            tb.SetBounds(tbLeft, tbTop, tbW, tbH);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text;
            string passWord = textBox2.Text;
            if (Login(userName, passWord))
            {
                FormHomePage f = new FormHomePage();
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin đăng nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        bool Login(string userName, string passWord)
        {
            return AccountDAL.Instance.Login(userName, passWord);
        }  

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Thoát chương trình ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true; // Hủy sự kiện đóng form
            }
        }
    }
}
