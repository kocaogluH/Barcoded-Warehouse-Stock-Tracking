using System;
using System.Windows;
using System.Windows.Controls;

namespace Barcoded_Warehouse_Stock_Tracking.Views
{
    public partial class CustomersView : UserControl
    {
        public CustomersView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var dt = Database.GetCustomers();
                dgvCustomers.ItemsSource = dt.DefaultView;
                cmbCustomer.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Müşteri adı boş olamaz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Database.InsertCustomer(name, txtPhone.Text.Trim(), txtEmail.Text.Trim());
                txtName.Clear();
                txtPhone.Clear();
                txtEmail.Clear();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCollect_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Müşteri seçin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(txtAmount.Text, out double amt) || amt <= 0)
            {
                MessageBox.Show("Geçerli bir tutar girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string method = (cmbMethod.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Cash";
                long customerId = Convert.ToInt64(cmbCustomer.SelectedValue);
                
                Database.AddCollection(customerId, method, amt, Session.UserId ?? 0);
                
                txtAmount.Text = "0";
                LoadData();
                MessageBox.Show("Tahsilat başarıyla kaydedildi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
