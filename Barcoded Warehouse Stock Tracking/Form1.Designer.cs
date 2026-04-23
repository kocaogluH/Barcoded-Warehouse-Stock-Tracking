using Guna.UI2.WinForms;

namespace Barcoded_Warehouse_Stock_Tracking
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            var BgDark   = System.Drawing.Color.FromArgb(26, 26, 46);
            var BgMid    = System.Drawing.Color.FromArgb(22, 33, 62);
            var BgInput  = System.Drawing.Color.FromArgb(35, 45, 78);
            var Accent   = System.Drawing.Color.FromArgb(233, 69, 96);
            var AccentGrn= System.Drawing.Color.FromArgb(46, 204, 113);
            var AccentBlu= System.Drawing.Color.FromArgb(52, 152, 219);
            var AccentOrg= System.Drawing.Color.FromArgb(230, 126, 34);
            var TextMain = System.Drawing.Color.FromArgb(234, 234, 234);
            var TextDim  = System.Drawing.Color.FromArgb(140, 140, 160);

            this.tabControl        = new System.Windows.Forms.TabControl();
            this.tabProducts       = new System.Windows.Forms.TabPage();
            this.dgvProducts       = new System.Windows.Forms.DataGridView();
            this.tabMovements      = new System.Windows.Forms.TabPage();
            this.dgvMovements      = new System.Windows.Forms.DataGridView();

            // Guna2 controls
            this.btnAdd            = new Guna2Button();
            this.btnPos            = new Guna2Button();
            this.btnReturns        = new Guna2Button();
            this.btnCustomers      = new Guna2Button();
            this.btnReports        = new Guna2Button();
            this.btnAddMovement    = new Guna2Button();
            this.txtBarcode        = new Guna2TextBox();
            this.txtName           = new Guna2TextBox();
            this.txtPrice          = new Guna2TextBox();
            this.txtBarcodeMovement= new Guna2TextBox();
            this.cmbType           = new Guna2ComboBox();
            this.nudQuantity       = new System.Windows.Forms.NumericUpDown();
            this.lblPrice          = new System.Windows.Forms.Label();
            this.lblName           = new System.Windows.Forms.Label();
            this.lblBarcode        = new System.Windows.Forms.Label();
            this.lblType           = new System.Windows.Forms.Label();
            this.lblQuantity       = new System.Windows.Forms.Label();
            this.lblBarcodeMovement= new System.Windows.Forms.Label();

            this.tabControl.SuspendLayout();
            this.tabProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.tabMovements.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovements)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).BeginInit();
            this.SuspendLayout();

            // ── tabControl ──────────────────────────────────────────────────────
            this.tabControl.Controls.Add(this.tabProducts);
            this.tabControl.Controls.Add(this.tabMovements);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1100, 640);
            this.tabControl.TabIndex = 0;
            this.tabControl.BackColor = BgDark;

            // ── tabProducts ─────────────────────────────────────────────────────
            this.tabProducts.BackColor = BgDark;
            this.tabProducts.Controls.Add(this.dgvProducts);
            this.tabProducts.Controls.Add(this.btnAdd);
            this.tabProducts.Controls.Add(this.btnPos);
            this.tabProducts.Controls.Add(this.btnReturns);
            this.tabProducts.Controls.Add(this.btnCustomers);
            this.tabProducts.Controls.Add(this.btnReports);
            this.tabProducts.Controls.Add(this.txtPrice);
            this.tabProducts.Controls.Add(this.txtName);
            this.tabProducts.Controls.Add(this.txtBarcode);
            this.tabProducts.Controls.Add(this.lblPrice);
            this.tabProducts.Controls.Add(this.lblName);
            this.tabProducts.Controls.Add(this.lblBarcode);
            this.tabProducts.Location = new System.Drawing.Point(4, 29);
            this.tabProducts.Name = "tabProducts";
            this.tabProducts.Padding = new System.Windows.Forms.Padding(14);
            this.tabProducts.Size = new System.Drawing.Size(1092, 607);
            this.tabProducts.TabIndex = 0;
            this.tabProducts.Text = "📦  Ürünler";

            // ── dgvProducts ─────────────────────────────────────────────────────
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.Location = new System.Drawing.Point(18, 190);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.Size = new System.Drawing.Size(1056, 400);
            this.dgvProducts.TabIndex = 7;
            this.dgvProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProducts.BackgroundColor = BgMid;
            this.dgvProducts.GridColor = System.Drawing.Color.FromArgb(40, 55, 90);
            this.dgvProducts.DefaultCellStyle.BackColor = BgMid;
            this.dgvProducts.DefaultCellStyle.ForeColor = TextMain;
            this.dgvProducts.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);
            this.dgvProducts.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(233, 69, 96, 80);
            this.dgvProducts.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(22, 33, 62);
            this.dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Accent;
            this.dgvProducts.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            this.dgvProducts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvProducts.EnableHeadersVisualStyles = false;
            this.dgvProducts.RowHeadersVisible = false;

            // ── txtBarcode ──────────────────────────────────────────────────────
            this.txtBarcode.Location = new System.Drawing.Point(110, 18);
            this.txtBarcode.Width = 260; this.txtBarcode.Height = 38;
            this.txtBarcode.PlaceholderText = "Barkod okutun veya yazın...";
            this.txtBarcode.BorderRadius = 8; this.txtBarcode.Font = new System.Drawing.Font("Segoe UI", 10);
            this.txtBarcode.FillColor = BgInput; this.txtBarcode.BorderColor = Accent;
            this.txtBarcode.ForeColor = TextMain; this.txtBarcode.PlaceholderForeColor = TextDim;
            this.txtBarcode.Name = "txtBarcode"; this.txtBarcode.TabIndex = 1;

            // ── txtName ─────────────────────────────────────────────────────────
            this.txtName.Location = new System.Drawing.Point(110, 62);
            this.txtName.Width = 260; this.txtName.Height = 38;
            this.txtName.PlaceholderText = "Ürün adı";
            this.txtName.BorderRadius = 8; this.txtName.Font = new System.Drawing.Font("Segoe UI", 10);
            this.txtName.FillColor = BgInput; this.txtName.BorderColor = Accent;
            this.txtName.ForeColor = TextMain; this.txtName.PlaceholderForeColor = TextDim;
            this.txtName.Name = "txtName"; this.txtName.TabIndex = 3;

            // ── txtPrice ────────────────────────────────────────────────────────
            this.txtPrice.Location = new System.Drawing.Point(110, 106);
            this.txtPrice.Width = 260; this.txtPrice.Height = 38;
            this.txtPrice.PlaceholderText = "Birim fiyat (TL)";
            this.txtPrice.BorderRadius = 8; this.txtPrice.Font = new System.Drawing.Font("Segoe UI", 10);
            this.txtPrice.FillColor = BgInput; this.txtPrice.BorderColor = Accent;
            this.txtPrice.ForeColor = TextMain; this.txtPrice.PlaceholderForeColor = TextDim;
            this.txtPrice.Name = "txtPrice"; this.txtPrice.TabIndex = 5;

            // ── lblBarcode ──────────────────────────────────────────────────────
            this.lblBarcode.AutoSize = true; this.lblBarcode.ForeColor = TextDim;
            this.lblBarcode.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);
            this.lblBarcode.Location = new System.Drawing.Point(18, 26); this.lblBarcode.TabIndex = 0; this.lblBarcode.Text = "Barkod:";

            this.lblName.AutoSize = true; this.lblName.ForeColor = TextDim;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(18, 70); this.lblName.TabIndex = 2; this.lblName.Text = "Ürün Adı:";

            this.lblPrice.AutoSize = true; this.lblPrice.ForeColor = TextDim;
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);
            this.lblPrice.Location = new System.Drawing.Point(18, 114); this.lblPrice.TabIndex = 4; this.lblPrice.Text = "Birim Fiyat:";

            // ── btnAdd ──────────────────────────────────────────────────────────
            this.btnAdd.Text = "＋  Ürünü Kaydet";
            this.btnAdd.Location = new System.Drawing.Point(390, 18); this.btnAdd.Size = new System.Drawing.Size(200, 38);
            this.btnAdd.BorderRadius = 8; this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            this.btnAdd.FillColor = AccentGrn; this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.HoverState.FillColor = System.Drawing.Color.FromArgb(36, 174, 93);
            this.btnAdd.TabIndex = 6; this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // ── btnPos ──────────────────────────────────────────────────────────
            this.btnPos.Text = "🛒  POS / Kasa";
            this.btnPos.Location = new System.Drawing.Point(390, 62); this.btnPos.Size = new System.Drawing.Size(200, 38);
            this.btnPos.BorderRadius = 8; this.btnPos.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            this.btnPos.FillColor = Accent; this.btnPos.ForeColor = System.Drawing.Color.White;
            this.btnPos.HoverState.FillColor = System.Drawing.Color.FromArgb(210, 50, 75);
            this.btnPos.TabIndex = 8;

            // ── btnReturns ──────────────────────────────────────────────────────
            this.btnReturns.Text = "↩  İade / İptal";
            this.btnReturns.Location = new System.Drawing.Point(390, 106); this.btnReturns.Size = new System.Drawing.Size(200, 38);
            this.btnReturns.BorderRadius = 8; this.btnReturns.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            this.btnReturns.FillColor = AccentOrg; this.btnReturns.ForeColor = System.Drawing.Color.White;
            this.btnReturns.HoverState.FillColor = System.Drawing.Color.FromArgb(200, 100, 20);
            this.btnReturns.TabIndex = 9;

            // ── btnCustomers ─────────────────────────────────────────────────────
            this.btnCustomers.Text = "👤  Müşteriler / Cari";
            this.btnCustomers.Location = new System.Drawing.Point(610, 18); this.btnCustomers.Size = new System.Drawing.Size(200, 38);
            this.btnCustomers.BorderRadius = 8; this.btnCustomers.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            this.btnCustomers.FillColor = AccentBlu; this.btnCustomers.ForeColor = System.Drawing.Color.White;
            this.btnCustomers.HoverState.FillColor = System.Drawing.Color.FromArgb(40, 130, 200);
            this.btnCustomers.TabIndex = 10;

            // ── btnReports ──────────────────────────────────────────────────────
            this.btnReports.Text = "📊  Raporlar / Yedek";
            this.btnReports.Location = new System.Drawing.Point(610, 62); this.btnReports.Size = new System.Drawing.Size(200, 38);
            this.btnReports.BorderRadius = 8; this.btnReports.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            this.btnReports.FillColor = System.Drawing.Color.FromArgb(100, 100, 160); this.btnReports.ForeColor = System.Drawing.Color.White;
            this.btnReports.HoverState.FillColor = System.Drawing.Color.FromArgb(80, 80, 140);
            this.btnReports.TabIndex = 11;

            // ── tabMovements ─────────────────────────────────────────────────────
            this.tabMovements.BackColor = BgDark;
            this.tabMovements.Controls.Add(this.dgvMovements);
            this.tabMovements.Controls.Add(this.btnAddMovement);
            this.tabMovements.Controls.Add(this.cmbType);
            this.tabMovements.Controls.Add(this.lblType);
            this.tabMovements.Controls.Add(this.nudQuantity);
            this.tabMovements.Controls.Add(this.lblQuantity);
            this.tabMovements.Controls.Add(this.txtBarcodeMovement);
            this.tabMovements.Controls.Add(this.lblBarcodeMovement);
            this.tabMovements.Location = new System.Drawing.Point(4, 29);
            this.tabMovements.Name = "tabMovements";
            this.tabMovements.Padding = new System.Windows.Forms.Padding(14);
            this.tabMovements.Size = new System.Drawing.Size(1092, 607);
            this.tabMovements.TabIndex = 1;
            this.tabMovements.Text = "📋  Stok Hareketleri";

            // ── dgvMovements ─────────────────────────────────────────────────────
            this.dgvMovements.AllowUserToAddRows = false; this.dgvMovements.ReadOnly = true;
            this.dgvMovements.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvMovements.Location = new System.Drawing.Point(18, 110);
            this.dgvMovements.Name = "dgvMovements";
            this.dgvMovements.Size = new System.Drawing.Size(1056, 480); this.dgvMovements.TabIndex = 7;
            this.dgvMovements.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMovements.BackgroundColor = BgMid; this.dgvMovements.GridColor = System.Drawing.Color.FromArgb(40, 55, 90);
            this.dgvMovements.DefaultCellStyle.BackColor = BgMid;
            this.dgvMovements.DefaultCellStyle.ForeColor = TextMain;
            this.dgvMovements.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);
            this.dgvMovements.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219, 80);
            this.dgvMovements.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvMovements.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(22, 33, 62);
            this.dgvMovements.ColumnHeadersDefaultCellStyle.ForeColor = AccentBlu;
            this.dgvMovements.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            this.dgvMovements.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvMovements.EnableHeadersVisualStyles = false; this.dgvMovements.RowHeadersVisible = false;

            // ── Stok Hareketi Giriş Alanları ────────────────────────────────────
            this.lblBarcodeMovement.AutoSize = true; this.lblBarcodeMovement.ForeColor = TextDim;
            this.lblBarcodeMovement.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);
            this.lblBarcodeMovement.Location = new System.Drawing.Point(18, 24); this.lblBarcodeMovement.Text = "Barkod:";

            this.txtBarcodeMovement.Location = new System.Drawing.Point(110, 18); this.txtBarcodeMovement.Width = 240; this.txtBarcodeMovement.Height = 38;
            this.txtBarcodeMovement.PlaceholderText = "Ürün barkodu"; this.txtBarcodeMovement.BorderRadius = 8;
            this.txtBarcodeMovement.Font = new System.Drawing.Font("Segoe UI", 10);
            this.txtBarcodeMovement.FillColor = BgInput; this.txtBarcodeMovement.BorderColor = AccentBlu;
            this.txtBarcodeMovement.ForeColor = TextMain; this.txtBarcodeMovement.PlaceholderForeColor = TextDim;
            this.txtBarcodeMovement.Name = "txtBarcodeMovement"; this.txtBarcodeMovement.TabIndex = 1;

            this.lblQuantity.AutoSize = true; this.lblQuantity.ForeColor = TextDim;
            this.lblQuantity.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);
            this.lblQuantity.Location = new System.Drawing.Point(365, 24); this.lblQuantity.Text = "Miktar:";

            this.nudQuantity.Location = new System.Drawing.Point(430, 18); this.nudQuantity.Width = 90; this.nudQuantity.Height = 38;
            this.nudQuantity.Minimum = 1; this.nudQuantity.Maximum = 100000; this.nudQuantity.Value = 1;
            this.nudQuantity.BackColor = BgInput; this.nudQuantity.ForeColor = TextMain;
            this.nudQuantity.Font = new System.Drawing.Font("Segoe UI", 10);
            this.nudQuantity.Name = "nudQuantity"; this.nudQuantity.TabIndex = 3;

            this.lblType.AutoSize = true; this.lblType.ForeColor = TextDim;
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);
            this.lblType.Location = new System.Drawing.Point(535, 24); this.lblType.Text = "Tür:";

            this.cmbType.Location = new System.Drawing.Point(575, 18); this.cmbType.Width = 130; this.cmbType.Height = 38;
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Font = new System.Drawing.Font("Segoe UI", 10);
            this.cmbType.FillColor = BgInput; this.cmbType.ForeColor = TextMain;
            this.cmbType.Items.AddRange(new object[] { "Giriş", "Çıkış" });
            this.cmbType.Name = "cmbType"; this.cmbType.TabIndex = 5;

            this.btnAddMovement.Text = "✔  Hareket Kaydet";
            this.btnAddMovement.Location = new System.Drawing.Point(720, 18); this.btnAddMovement.Size = new System.Drawing.Size(200, 38);
            this.btnAddMovement.BorderRadius = 8; this.btnAddMovement.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            this.btnAddMovement.FillColor = AccentGrn; this.btnAddMovement.ForeColor = System.Drawing.Color.White;
            this.btnAddMovement.HoverState.FillColor = System.Drawing.Color.FromArgb(36, 174, 93);
            this.btnAddMovement.TabIndex = 6; this.btnAddMovement.Click += new System.EventHandler(this.btnAddMovement_Click);

            // ── Form1 ────────────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 640);
            this.Controls.Add(this.tabControl);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Poseidon Yazılım — Depo & Stok Takip Sistemi";
            this.BackColor = BgDark;
            this.Load += new System.EventHandler(this.Form1_Load);

            this.tabControl.ResumeLayout(false);
            this.tabProducts.ResumeLayout(false);
            this.tabProducts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.tabMovements.ResumeLayout(false);
            this.tabMovements.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovements)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabProducts;
        private System.Windows.Forms.DataGridView dgvProducts;
        private Guna2Button btnAdd;
        private Guna2Button btnPos;
        private Guna2Button btnReturns;
        private Guna2Button btnCustomers;
        private Guna2Button btnReports;
        private Guna2TextBox txtPrice;
        private Guna2TextBox txtName;
        private Guna2TextBox txtBarcode;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.TabPage tabMovements;
        private System.Windows.Forms.DataGridView dgvMovements;
        private Guna2Button btnAddMovement;
        private Guna2ComboBox cmbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.NumericUpDown nudQuantity;
        private System.Windows.Forms.Label lblQuantity;
        private Guna2TextBox txtBarcodeMovement;
        private System.Windows.Forms.Label lblBarcodeMovement;
    }
}
