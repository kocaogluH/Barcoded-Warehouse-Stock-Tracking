using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Barcoded_Warehouse_Stock_Tracking
{
    public class FrmReports : Form
    {
        private static readonly Color BgDark   = Color.FromArgb(26, 26, 46);
        private static readonly Color BgMid    = Color.FromArgb(22, 33, 62);
        private static readonly Color BgInput  = Color.FromArgb(35, 45, 78);
        private static readonly Color Accent   = Color.FromArgb(233, 69, 96);
        private static readonly Color AccentBlu= Color.FromArgb(52, 152, 219);
        private static readonly Color AccentGrn= Color.FromArgb(46, 204, 113);
        private static readonly Color TextMain = Color.FromArgb(234, 234, 234);
        private static readonly Color TextDim  = Color.FromArgb(140, 140, 160);

        private readonly TabControl     _tabs             = new TabControl();
        private readonly DateTimePicker _dtFrom           = new DateTimePicker();
        private readonly DateTimePicker _dtTo             = new DateTimePicker();
        private readonly Guna2Button    _btnRefreshSales  = new Guna2Button();
        private readonly DataGridView   _gridDaily        = new DataGridView();
        private readonly DataGridView   _gridProducts     = new DataGridView();
        private readonly NumericUpDown  _nudThreshold     = new NumericUpDown();
        private readonly Guna2Button    _btnRefreshStock  = new Guna2Button();
        private readonly DataGridView   _gridLowStock     = new DataGridView();
        private readonly Guna2Button    _btnRefreshCust   = new Guna2Button();
        private readonly DataGridView   _gridCustomers    = new DataGridView();
        private readonly Guna2Button    _btnBackup        = new Guna2Button();

        private static Guna2Button MakeBtn(string text, Color fill, int x, int y, int w = 180, int h = 36)
        {
            return new Guna2Button
            {
                Text = text, Location = new Point(x, y), Size = new Size(w, h),
                BorderRadius = 8, Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FillColor = fill, ForeColor = Color.White,
                HoverState = { FillColor = ControlPaint.Dark(fill, 0.1f) }
            };
        }

        private static void StyleGrid(DataGridView g, Color bg, Color accent)
        {
            g.AllowUserToAddRows = false; g.ReadOnly = true;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.AutoGenerateColumns = true; g.Dock = DockStyle.Fill;
            g.BorderStyle = BorderStyle.None; g.BackgroundColor = bg;
            g.GridColor = Color.FromArgb(40, 55, 90);
            g.DefaultCellStyle.BackColor = bg;
            g.DefaultCellStyle.ForeColor = Color.FromArgb(234, 234, 234);
            g.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(accent.R, accent.G, accent.B, 80);
            g.DefaultCellStyle.SelectionForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(22, 33, 62);
            g.ColumnHeadersDefaultCellStyle.ForeColor = accent;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.EnableHeadersVisualStyles = false; g.RowHeadersVisible = false; 
        }

        private TabPage MakeDarkTab(string title)
        {
            return new TabPage(title) { BackColor = BgDark };
        }

        public FrmReports()
        {
            Text = "Poseidon Yazılım — Raporlar & Yedek";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1080; Height = 720;
            BackColor = BgDark;

            // ── TOP ────────────────────────────────────────────────────────────
            var top = new Panel { Dock = DockStyle.Top, Height = 85, BackColor = BgMid, Padding = new Padding(15) };

            _dtFrom.Format = DateTimePickerFormat.Short; _dtFrom.Value = DateTime.Today.AddDays(-7);
            _dtFrom.Location = new Point(25, 30); _dtFrom.Width = 125; _dtFrom.CalendarForeColor = TextMain; _dtFrom.CalendarMonthBackground = BgMid;

            _dtTo.Format = DateTimePickerFormat.Short; _dtTo.Value = DateTime.Today.AddDays(1);
            _dtTo.Location = new Point(160, 30); _dtTo.Width = 125;

            _btnRefreshSales.Text = "SATIŞ RAPORLARINI YENİLE";
            _btnRefreshSales.Location = new Point(300, 26); _btnRefreshSales.Size = new Size(220, 38);
            _btnRefreshSales.BorderRadius = 8; _btnRefreshSales.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            _btnRefreshSales.FillColor = AccentBlu; _btnRefreshSales.ForeColor = Color.White;
            _btnRefreshSales.Click += (_, __) => RefreshSales();

            _btnBackup.Text = "VERİTABANI YEDEKLE";
            _btnBackup.Location = new Point(530, 26); _btnBackup.Size = new Size(180, 38);
            _btnBackup.BorderRadius = 8; _btnBackup.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            _btnBackup.FillColor = Color.FromArgb(100, 100, 130); _btnBackup.ForeColor = Color.White;
            _btnBackup.Click += (_, __) => BackupDb();

            top.Controls.Add(_dtFrom); top.Controls.Add(_dtTo);
            top.Controls.Add(_btnRefreshSales); top.Controls.Add(_btnBackup);

            // ── TABS ───────────────────────────────────────────────────────────
            _tabs.Dock = DockStyle.Fill;
            _tabs.BackColor = BgDark;
            _tabs.Font = new Font("Segoe UI", 10);

            StyleGrid(_gridDaily,    BgMid, Accent);
            StyleGrid(_gridProducts, BgMid, AccentBlu);
            StyleGrid(_gridLowStock, BgMid, Accent);
            StyleGrid(_gridCustomers,BgMid, AccentGrn);

            var tabDaily = MakeDarkTab("📅 Günlük Satış");
            tabDaily.Controls.Add(_gridDaily);

            var tabProd = MakeDarkTab("📦 Ürün Satışları");
            tabProd.Controls.Add(_gridProducts);

            var tabStock = MakeDarkTab("⚠ Stok Azalanlar");
            var stockTop = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = BgMid, Padding = new Padding(12) };
            var lblT = new Label { Text = "Eşik Seviyesi:", ForeColor = TextDim, AutoSize = true, Location = new Point(12, 18), Font = new Font("Segoe UI", 9) };
            _nudThreshold.Location = new Point(110, 16); _nudThreshold.Width = 80;
            _nudThreshold.Minimum = 0; _nudThreshold.Maximum = 100000; _nudThreshold.Value = 5;
            _nudThreshold.BackColor = BgInput; _nudThreshold.ForeColor = TextMain; _nudThreshold.Font = new Font("Segoe UI", 10);
            _btnRefreshStock.Text = "🔄  Yenile"; _btnRefreshStock.Location = new Point(210, 13); _btnRefreshStock.Size = new Size(120, 30);
            _btnRefreshStock.BorderRadius = 6; _btnRefreshStock.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            _btnRefreshStock.FillColor = Accent; _btnRefreshStock.ForeColor = Color.White;
            _btnRefreshStock.Click += (_, __) => RefreshLowStock();
            stockTop.Controls.Add(lblT); stockTop.Controls.Add(_nudThreshold); stockTop.Controls.Add(_btnRefreshStock);
            tabStock.Controls.Add(_gridLowStock); tabStock.Controls.Add(stockTop);

            var tabCust = MakeDarkTab("💳 Cari Bakiye");
            var custTop = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = BgMid, Padding = new Padding(12) };
            _btnRefreshCust.Text = "🔄  Yenile"; _btnRefreshCust.Location = new Point(12, 13); _btnRefreshCust.Size = new Size(120, 30);
            _btnRefreshCust.BorderRadius = 6; _btnRefreshCust.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            _btnRefreshCust.FillColor = AccentGrn; _btnRefreshCust.ForeColor = Color.White;
            _btnRefreshCust.Click += (_, __) => RefreshCustomers();
            custTop.Controls.Add(_btnRefreshCust);
            tabCust.Controls.Add(_gridCustomers); tabCust.Controls.Add(custTop);

            _tabs.TabPages.Add(tabDaily); _tabs.TabPages.Add(tabProd);
            _tabs.TabPages.Add(tabStock); _tabs.TabPages.Add(tabCust);

            Controls.Add(_tabs);
            Controls.Add(top);

            RefreshSales(); RefreshLowStock(); RefreshCustomers();
        }

        private void RefreshSales()
        {
            var from = _dtFrom.Value.Date;
            var to   = _dtTo.Value.Date;
            if (to <= from) to = from.AddDays(1);
            _gridDaily.DataSource    = Database.ReportDailySales(from, to);
            _gridProducts.DataSource = Database.ReportProductSales(from, to);
        }

        private void RefreshLowStock() => _gridLowStock.DataSource = Database.ReportLowStock((int)_nudThreshold.Value);
        private void RefreshCustomers() => _gridCustomers.DataSource = Database.ReportCustomerBalances();

        private void BackupDb()
        {
            var src = AppPaths.DatabaseFilePath;
            if (!File.Exists(src)) { MessageBox.Show("DB dosyası bulunamadı: " + src, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Veritabanı Yedeği Kaydet"; sfd.Filter = "SQLite DB (*.db)|*.db";
                sfd.FileName = $"miyuki-backup-{DateTime.Now:yyyyMMdd-HHmmss}.db";
                if (sfd.ShowDialog(this) != DialogResult.OK) return;
                File.Copy(src, sfd.FileName, overwrite: true);
                MessageBox.Show("✔ Yedek alındı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
