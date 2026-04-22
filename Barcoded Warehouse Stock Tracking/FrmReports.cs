using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Barcoded_Warehouse_Stock_Tracking
{
    public class FrmReports : Form
    {
        private readonly TabControl _tabs = new TabControl();

        private readonly DateTimePicker _dtFrom = new DateTimePicker();
        private readonly DateTimePicker _dtTo = new DateTimePicker();
        private readonly Button _btnRefreshSales = new Button();
        private readonly DataGridView _gridDaily = new DataGridView();
        private readonly DataGridView _gridProducts = new DataGridView();

        private readonly NumericUpDown _nudThreshold = new NumericUpDown();
        private readonly Button _btnRefreshStock = new Button();
        private readonly DataGridView _gridLowStock = new DataGridView();

        private readonly Button _btnRefreshCustomers = new Button();
        private readonly DataGridView _gridCustomers = new DataGridView();

        private readonly Button _btnBackup = new Button();

        public FrmReports()
        {
            Text = "Raporlar + Yedek";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1050;
            Height = 700;

            var top = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(12) };

            _dtFrom.Format = DateTimePickerFormat.Short;
            _dtTo.Format = DateTimePickerFormat.Short;
            _dtTo.Value = DateTime.Today.AddDays(1);
            _dtFrom.Value = DateTime.Today.AddDays(-7);

            _dtFrom.Location = new Point(12, 18);
            _dtTo.Location = new Point(140, 18);

            _btnRefreshSales.Text = "Satış Raporlarını Yenile";
            _btnRefreshSales.Location = new Point(270, 14);
            _btnRefreshSales.Size = new Size(200, 32);
            _btnRefreshSales.Click += (_, __) => RefreshSales();

            _btnBackup.Text = "DB Yedekle";
            _btnBackup.Location = new Point(490, 14);
            _btnBackup.Size = new Size(140, 32);
            _btnBackup.Click += (_, __) => BackupDb();

            top.Controls.Add(_dtFrom);
            top.Controls.Add(_dtTo);
            top.Controls.Add(_btnRefreshSales);
            top.Controls.Add(_btnBackup);

            _tabs.Dock = DockStyle.Fill;

            var tabDaily = new TabPage("Günlük Satış");
            var tabProd = new TabPage("Ürün Satışları");
            var tabStock = new TabPage("Stok Azalanlar");
            var tabCust = new TabPage("Cari Bakiye");

            SetupGrid(_gridDaily);
            SetupGrid(_gridProducts);
            SetupGrid(_gridLowStock);
            SetupGrid(_gridCustomers);

            _gridDaily.Dock = DockStyle.Fill;
            _gridProducts.Dock = DockStyle.Fill;
            _gridCustomers.Dock = DockStyle.Fill;

            tabDaily.Controls.Add(_gridDaily);
            tabProd.Controls.Add(_gridProducts);
            tabCust.Controls.Add(_gridCustomers);

            // Stock tab with threshold
            var stockTop = new Panel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(12) };
            var lblT = new Label { Text = "Eşik", AutoSize = true, Location = new Point(12, 18) };
            _nudThreshold.Location = new Point(60, 16);
            _nudThreshold.Width = 90;
            _nudThreshold.Minimum = 0;
            _nudThreshold.Maximum = 100000;
            _nudThreshold.Value = 5;

            _btnRefreshStock.Text = "Yenile";
            _btnRefreshStock.Location = new Point(170, 13);
            _btnRefreshStock.Size = new Size(110, 30);
            _btnRefreshStock.Click += (_, __) => RefreshLowStock();

            stockTop.Controls.Add(lblT);
            stockTop.Controls.Add(_nudThreshold);
            stockTop.Controls.Add(_btnRefreshStock);

            _gridLowStock.Dock = DockStyle.Fill;
            tabStock.Controls.Add(_gridLowStock);
            tabStock.Controls.Add(stockTop);

            // Customers tab top
            var custTop = new Panel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(12) };
            _btnRefreshCustomers.Text = "Yenile";
            _btnRefreshCustomers.Location = new Point(12, 13);
            _btnRefreshCustomers.Size = new Size(110, 30);
            _btnRefreshCustomers.Click += (_, __) => RefreshCustomers();
            custTop.Controls.Add(_btnRefreshCustomers);
            tabCust.Controls.Add(custTop);

            _tabs.TabPages.Add(tabDaily);
            _tabs.TabPages.Add(tabProd);
            _tabs.TabPages.Add(tabStock);
            _tabs.TabPages.Add(tabCust);

            Controls.Add(_tabs);
            Controls.Add(top);

            RefreshSales();
            RefreshLowStock();
            RefreshCustomers();
        }

        private static void SetupGrid(DataGridView g)
        {
            g.AllowUserToAddRows = false;
            g.ReadOnly = true;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.AutoGenerateColumns = true;
        }

        private void RefreshSales()
        {
            var from = _dtFrom.Value.Date;
            var to = _dtTo.Value.Date;
            if (to <= from) to = from.AddDays(1);

            _gridDaily.DataSource = Database.ReportDailySales(from, to);
            _gridProducts.DataSource = Database.ReportProductSales(from, to);
        }

        private void RefreshLowStock()
        {
            _gridLowStock.DataSource = Database.ReportLowStock((int)_nudThreshold.Value);
        }

        private void RefreshCustomers()
        {
            _gridCustomers.DataSource = Database.ReportCustomerBalances();
        }

        private void BackupDb()
        {
            var src = AppPaths.DatabaseFilePath;
            if (!File.Exists(src))
            {
                MessageBox.Show("DB dosyası bulunamadı: " + src, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Veritabanı Yedeği Kaydet";
                sfd.Filter = "SQLite DB (*.db)|*.db";
                sfd.FileName = $"warehouse-backup-{DateTime.Now:yyyyMMdd-HHmmss}.db";
                if (sfd.ShowDialog(this) != DialogResult.OK) return;

                File.Copy(src, sfd.FileName, overwrite: true);
                MessageBox.Show("Yedek alındı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

