using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Barcoded_Warehouse_Stock_Tracking.Views
{
    public partial class ReportsView : UserControl
    {
        public ReportsView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtFrom.SelectedDate = DateTime.Today.AddDays(-7);
            dtTo.SelectedDate = DateTime.Today.AddDays(1);

            RefreshSales();
            RefreshLowStock();
            RefreshCustomers();
        }

        private void BtnRefreshSales_Click(object sender, RoutedEventArgs e)
        {
            RefreshSales();
        }

        private void RefreshSales()
        {
            var from = dtFrom.SelectedDate ?? DateTime.Today.AddDays(-7);
            var to = dtTo.SelectedDate ?? DateTime.Today.AddDays(1);
            if (to <= from) to = from.AddDays(1);

            try
            {
                dgvDaily.ItemsSource = Database.ReportDailySales(from, to).DefaultView;
                dgvProducts.ItemsSource = Database.ReportProductSales(from, to).DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRefreshStock_Click(object sender, RoutedEventArgs e)
        {
            RefreshLowStock();
        }

        private void RefreshLowStock()
        {
            if (!int.TryParse(txtThreshold.Text, out int threshold))
            {
                threshold = 5;
            }

            try
            {
                dgvLowStock.ItemsSource = Database.ReportLowStock(threshold).DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRefreshCust_Click(object sender, RoutedEventArgs e)
        {
            RefreshCustomers();
        }

        private void RefreshCustomers()
        {
            try
            {
                dgvCustomers.ItemsSource = Database.ReportCustomerBalances().DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBackup_Click(object sender, RoutedEventArgs e)
        {
            var src = AppPaths.DatabaseFilePath;
            if (!File.Exists(src))
            {
                MessageBox.Show("DB dosyası bulunamadı: " + src, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var sfd = new SaveFileDialog
            {
                Title = "Veritabanı Yedeği Kaydet",
                Filter = "SQLite DB (*.db)|*.db",
                FileName = $"poseidon-backup-{DateTime.Now:yyyyMMdd-HHmmss}.db"
            };

            if (sfd.ShowDialog() == true)
            {
                try
                {
                    File.Copy(src, sfd.FileName, overwrite: true);
                    MessageBox.Show("✔ Yedek başarıyla alındı.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Yedekleme sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
