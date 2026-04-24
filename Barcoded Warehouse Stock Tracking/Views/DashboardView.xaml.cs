using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Barcoded_Warehouse_Stock_Tracking.Business;
using Barcoded_Warehouse_Stock_Tracking.DataAccess;
using Barcoded_Warehouse_Stock_Tracking.Entities;

namespace Barcoded_Warehouse_Stock_Tracking.Views
{
    public partial class DashboardView : UserControl
    {
        private DashboardService _dashboardService;
        private ProductService _productService;
        private WarehouseContext _context;

        public DashboardView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshAll();
        }

        public void RefreshAll()
        {
            _context?.Dispose();
            _context = new WarehouseContext();
            _productService = new ProductService(_context);
            _dashboardService = new DashboardService(_context);

            dgvProducts.ItemsSource = _productService.GetAllActiveProducts()
                .OrderBy(p => p.Id)
                .ToList();

            dgvMovements.ItemsSource = _context.StockMovements
                .OrderByDescending(m => m.Id)
                .Take(100)
                .ToList();

            lblTotalProducts.Text = _dashboardService.GetTotalActiveProducts().ToString();
            lblDailySales.Text = _dashboardService.GetTodaySalesTotal().ToString("C2");

            var lowStockCount = _productService.GetAllActiveProducts().Count(p => p.StockQty < 5);
            lblLowStock.Text = lowStockCount.ToString();
        }

        private void TxtBarcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            var term = txtBarcode.Text.Trim();
            if (string.IsNullOrWhiteSpace(term))
            {
                dgvProducts.ItemsSource = _productService.GetAllActiveProducts()
                    .OrderBy(p => p.Id)
                    .ToList();
            }
            else
            {
                var filtered = _productService.SearchProducts(term);
                dgvProducts.ItemsSource = filtered
                    .OrderBy(p => p.Id)
                    .ToList();

                var exact = _productService.GetProductByBarcode(term);
                if (exact != null)
                {
                    txtName.Text = exact.Name;
                    txtPrice.Text = exact.UnitPrice.ToString("0.00");
                }
            }
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var barcode = txtBarcode.Text.Trim();
            var name = txtName.Text.Trim();
            var priceText = txtPrice.Text.Trim();

            if (string.IsNullOrWhiteSpace(barcode) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Barkod ve ürün adı zorunludur.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(priceText, out var price) || price < 0)
            {
                MessageBox.Show("Geçerli bir birim fiyat girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var existingProduct = _productService.GetProductByBarcode(barcode);
            if (existingProduct != null)
            {
                existingProduct.Name = name;
                existingProduct.UnitPrice = price;
                _productService.UpdateProduct(existingProduct);
                MessageBox.Show("Mevcut ürün güncellendi!", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var p = new Product { Barcode = barcode, Name = name, UnitPrice = price };
                _productService.AddProduct(p);
                MessageBox.Show("Yeni ürün eklendi!", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            RefreshAll();
            txtBarcode.Clear();
            txtName.Clear();
            txtPrice.Clear();
        }

        private void BtnAddMovement_Click(object sender, RoutedEventArgs e)
        {
            var barcode = txtMovementBarcode.Text.Trim();
            var prod = _productService.GetProductByBarcode(barcode);
            if (prod == null)
            {
                MessageBox.Show("Ürün bulunamadı.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Geçerli bir miktar girin.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string type = (cmbType.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Giriş";

            if (type == "Çıkış")
            {
                if (prod.StockQty < qty)
                {
                    MessageBox.Show("Yetersiz stok.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                prod.StockQty -= qty;
            }
            else
            {
                prod.StockQty += qty;
            }

            _productService.UpdateProduct(prod);

            int storedQty = (type == "Çıkış") ? -qty : qty;

            _context.StockMovements.Add(new StockMovement
            {
                ProductId = prod.Id,
                BarcodeSnapshot = barcode,
                Quantity = storedQty,
                Type = type,
                Reason = "Manuel " + type,
                RefType = "Manual",
                CreatedAt = DateTime.Now,
                CreatedByUserId = Session.UserId
            });
            _context.SaveChanges();

            new LogService(_context).Info("Stok Hareketi: " + type, $"Barkod: {barcode}, Miktar: {qty}", Session.UserId);

            RefreshAll();
            txtMovementBarcode.Clear();
            txtQuantity.Text = "1";
        }

        private void DgvProducts_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.Item is Product product)
            {
                if (product.StockQty < 5)
                {
                    e.Row.Background = new SolidColorBrush(Color.FromRgb(233, 69, 96));
                    e.Row.Foreground = Brushes.White;
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Color.FromRgb(22, 33, 62));
                    e.Row.Foreground = Brushes.White;
                }
            }
        }
    }
}
