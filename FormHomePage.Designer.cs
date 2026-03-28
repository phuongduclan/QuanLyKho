namespace QuanLyKho
{
    partial class FormHomePage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel2 = new Panel();
            listView1 = new ListView();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            button4 = new Button();
            flowLayoutPanel3 = new FlowLayoutPanel();
            menuStrip2 = new MenuStrip();
            adminToolStripMenuItem = new ToolStripMenuItem();
            panel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            menuStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip2
            // 
            menuStrip2.ImageScalingSize = new Size(24, 24);
            menuStrip2.Items.AddRange(new ToolStripItem[] { adminToolStripMenuItem });
            menuStrip2.Location = new Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new Size(1347, 33);
            menuStrip2.TabIndex = 5;
            menuStrip2.Text = "menuStrip2";
            // 
            // adminToolStripMenuItem
            // 
            adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            adminToolStripMenuItem.Size = new Size(81, 29);
            adminToolStripMenuItem.Text = "Admin";
            adminToolStripMenuItem.Click += adminToolStripMenuItem_Click;
            // 
            // flowLayoutPanel3  –  Khu vực cards vị trí (trái)
            // 
            flowLayoutPanel3.AutoScroll = true;
            flowLayoutPanel3.Location = new Point(12, 45);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(747, 906);
            flowLayoutPanel3.TabIndex = 3;
            // 
            // panel2  –  Khu vực tồn kho (phải, trên)
            // 
            panel2.Controls.Add(listView1);
            panel2.Location = new Point(765, 45);
            panel2.Name = "panel2";
            panel2.Size = new Size(570, 854);
            panel2.TabIndex = 1;
            // 
            // listView1
            // 
            listView1.Dock = DockStyle.Fill;
            listView1.GridLines = true;
            listView1.Location = new Point(0, 0);
            listView1.Name = "listView1";
            listView1.Size = new Size(570, 854);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.List;
            // 
            // flowLayoutPanel1  –  Nút hành động (phải, dưới)
            // 
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(button4);
            flowLayoutPanel1.Location = new Point(765, 907);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(2);
            flowLayoutPanel1.Size = new Size(570, 56);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // button2  –  Xác nhận
            // 
            button2.Location = new Point(5, 5);
            button2.Name = "button2";
            button2.Size = new Size(275, 46);
            button2.TabIndex = 4;
            button2.Text = "Xác nhận";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button4  –  Chuyển vị trí
            // 
            button4.Location = new Point(288, 5);
            button4.Name = "button4";
            button4.Size = new Size(275, 46);
            button4.TabIndex = 6;
            button4.Text = "Chuyển vị trí";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // FormHomePage
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1347, 963);
            Controls.Add(flowLayoutPanel3);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panel2);
            Controls.Add(menuStrip2);
            MainMenuStrip = menuStrip2;
            Name = "FormHomePage";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Phần mềm quản lý kho";
            panel2.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel2;
        private ListView listView1;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel3;
        private Button button2;
        private Button button4;
        private MenuStrip menuStrip2;
        private ToolStripMenuItem adminToolStripMenuItem;
    }
}