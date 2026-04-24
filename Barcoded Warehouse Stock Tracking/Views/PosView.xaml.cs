using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Barcoded_Warehouse_Stock_Tracking.Entities;

namespace Barcoded_Warehouse_Stock_Tracking.Views
{
    public class CartItem
    {
        public long ProductId { get; set; }
        public string BarcodeSnapshot { get; set; }
        public string NameSnapshot { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice => UnitPrice * Quantity;
    }

    public partial class PosView : UserControl
    {
        private ObservableCollection<CartItem> _cartItems = new ObservableCollection<CartItem>();

        public PosView()
        {
            InitializeComponent();
            dgvCart.ItemsSource = _cartItems;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomers();
            UpdateTotals();
            txtBarcode.Focus();
        }

        private void LoadCustomers()
        {
            try
            {
                cmbCustomer.ItemsSource = Database.GetCustomers().DefaultView;
                cmbCustomer.DisplayMemberPath = "Name";
                cmbCustomer.SelectedValuePath = "Id";
            }
            catch { }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                AddToCart();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddToCart();
        }

        private void AddToCart()
        {
            var bc = txtBarcode.Text.Trim();
            if (string.IsNullOrEmpty(bc)) return;

            if (Database.TryGetProductForSale(bc, out long pid, out string name, out double price, out int stock))
            {
                if (!int.TryParse(txtQty.Text, out int qty) || qty <= 0) qty = 1;

                _cartItems.Add(new CartItem
                {
                    ProductId = pid,
                    BarcodeSnapshot = bc,
                    NameSnapshot = name,
                    UnitPrice = price,
                    Quantity = qty
                });

                UpdateTotals();
                txtBarcode.Clear();
                txtQty.Text = "1";
                txtBarcode.Focus();
            }
            else
            {
                MessageBox.Show("Ürün bulunamadı!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateTotals()
        {
            double subtotal = 0;
            foreach (var it in _cartItems) subtotal += it.TotalPrice;

            double.TryParse(txtDiscount?.Text, out double disc);
            double.TryParse(txtCard?.Text, out double card);
            
            double grand = subtotal - disc;
            double cash = grand - card;
            
            if (txtCash != null) txtCash.Text = cash.ToString("N2");
            if (lblTotal != null) lblTotal.Text = $"Toplam:  {grand:N2} TL\nÖdeme:  {(card + cash):N2} TL";
        }

        private void Payment_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotals();
        }

        private void ChkCustomer_CheckedChanged(object sender, RoutedEventArgs e)
        {
            bool isChecked = chkCustomer.IsChecked == true;
            cmbCustomer.IsEnabled = isChecked;
            btnCustomers.IsEnabled = isChecked;
        }

        private void BtnCustomers_Click(object sender, RoutedEventArgs e)
        {
            // Open a small dialog for customers or just refresh
            // Since we are moving to SPA, we can either open a Dialog Window for this
            // or navigate to CustomersView. For POS customer selection, opening a Window is okay 
            // since it's a popup action. But the user asked for no new windows.
            // For now, let's keep the user in POS. We will implement CustomersView later.
            MessageBox.Show("Müşteriler sayfasına soldaki menüden geçebilirsiniz.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnComplete_Click(object sender, RoutedEventArgs e)
        {
            if (_cartItems.Count == 0) return;
            
            try
            {
                double.TryParse(txtDiscount.Text, out double disc);
                double.TryParse(txtCard.Text, out double cardVal);
                double.TryParse(txtCash.Text, out double cashVal);

                var payments = new System.Collections.Generic.List<(string, double)>();
                if (cashVal > 0) payments.Add(("Cash", cashVal));
                if (cardVal > 0) payments.Add(("Card", cardVal));

                string saleNo = string.IsNullOrWhiteSpace(txtSaleNo.Text)
                    ? "S" + DateTime.Now.Ticks : txtSaleNo.Text.Trim();
                
                long? cid = chkCustomer.IsChecked == true ? (long?)cmbCustomer.SelectedValue : null;

                var dbItems = new System.Collections.Generic.List<Database.SaleItemInput>();
                foreach(var item in _cartItems)
                {
                    dbItems.Add(new Database.SaleItemInput
                    {
                        ProductId = item.ProductId,
                        BarcodeSnapshot = item.BarcodeSnapshot,
                        NameSnapshot = item.NameSnapshot,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity
                    });
                }

                Database.CreateSale(saleNo, cid, dbItems, disc, payments, Session.UserId ?? 0);
                
                if (chkAutoPrint.IsChecked == true)
                {
                    PrintReceipt(saleNo, dbItems, disc, cashVal + cardVal);
                }

                _cartItems.Clear();
                txtDiscount.Text = "0";
                txtCard.Text = "0";
                txtSaleNo.Clear();
                UpdateTotals();
                
                MessageBox.Show("Satış başarıyla tamamlandı.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PrintReceipt(string saleNo, System.Collections.Generic.List<Database.SaleItemInput> items, double disc, double grand)
        {
            try
            {
                var pd = new PrintDocument();
                pd.PrintPage += (s, ev) =>
                {
                    Graphics g = ev.Graphics;
                    Font fTitle = new Font("Courier New", 12, System.Drawing.FontStyle.Bold);
                    Font fBody = new Font("Courier New", 9);
                    int y = 10;

                    g.DrawString("POSEIDON YAZILIM", fTitle, Brushes.Black, 10, y); y += 20;
                    g.DrawString("Fiş No: " + saleNo, fBody, Brushes.Black, 10, y); y += 15;
                    g.DrawString(DateTime.Now.ToString("g"), fBody, Brushes.Black, 10, y); y += 20;
                    g.DrawString("-------------------------------", fBody, Brushes.Black, 10, y); y += 15;

                    foreach (var it in items)
                    {
                        g.DrawString(it.NameSnapshot, fBody, Brushes.Black, 10, y); y += 12;
                        g.DrawString($"{it.Quantity} x {it.UnitPrice:N2} = {it.Quantity * it.UnitPrice:N2}",
                                     fBody, Brushes.Black, 20, y); y += 15;
                    }

                    g.DrawString("-------------------------------", fBody, Brushes.Black, 10, y); y += 15;
                    if (disc > 0) { g.DrawString($"İndirim: {disc:N2} TL", fBody, Brushes.Black, 10, y); y += 15; }
                    g.DrawString($"TOPLAM : {grand:N2} TL", fTitle, Brushes.Black, 10, y); y += 25;
                    g.DrawString("Teşekkür Ederiz", fBody, Brushes.Black, 50, y);
                };
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yazıcı hatası: " + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
