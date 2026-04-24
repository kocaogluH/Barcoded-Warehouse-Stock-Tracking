using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Barcoded_Warehouse_Stock_Tracking.Views
{
    public partial class ReturnsView : UserControl
    {
        private DataTable _dt;

        public ReturnsView()
        {
            InitializeComponent();
        }

        private void txtSaleNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                LoadSale();
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadSale();
        }

        private void LoadSale()
        {
            var saleNo = txtSaleNo.Text.Trim();
            if (string.IsNullOrEmpty(saleNo)) return;

            try
            {
                _dt = Database.GetSaleItemsForReturn(saleNo);
                if (_dt.Rows.Count == 0)
                {
                    MessageBox.Show("Satış bulunamadı!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!_dt.Columns.Contains("ReturnQty"))
                    _dt.Columns.Add("ReturnQty", typeof(int));

                foreach (DataRow r in _dt.Rows)
                    r["ReturnQty"] = 0;

                dgvReturns.ItemsSource = _dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (_dt == null || _dt.Rows.Count == 0) return;

            try
            {
                var items = new List<Database.ReturnItemInput>();
                foreach (DataRow row in _dt.Rows)
                {
                    int rq = Convert.ToInt32(row["ReturnQty"]);
                    if (rq > 0)
                    {
                        items.Add(new Database.ReturnItemInput
                        {
                            SaleItemId = Convert.ToInt64(row["SaleItemId"]),
                            ProductId = Convert.ToInt64(row["ProductId"]),
                            BarcodeSnapshot = row["Barcode"].ToString(),
                            Quantity = rq,
                            UnitPrice = Convert.ToDouble(row["UnitPrice"])
                        });
                    }
                }

                if (items.Count == 0)
                {
                    MessageBox.Show("İade edilecek miktar girilmedi.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Database.CreateReturn(txtSaleNo.Text.Trim(), "RET" + DateTime.Now.Ticks, items, Session.UserId ?? 0);
                
                MessageBox.Show("İade işlemi tamamlandı.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadSale(); // Refresh
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
