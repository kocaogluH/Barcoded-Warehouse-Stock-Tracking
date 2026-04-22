using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Guna.UI2.WinForms;
using Barcoded_Warehouse_Stock_Tracking.Business;
using Barcoded_Warehouse_Stock_Tracking.DataAccess;
using Barcoded_Warehouse_Stock_Tracking.Entities;

namespace Barcoded_Warehouse_Stock_Tracking
{
    public partial class Form1 : Form
    {
        private DashboardService _dashboardService;
        private ProductService _productService;
        private WarehouseContext _context;
        
        // Guna Dashboard Cards
        private Guna2Panel pnlTotalProducts;
        private Label lblTotalProductsVal;
        
        private Guna2Panel pnlDailySales;
        private Label lblDailySalesVal;

        private Guna2Panel pnlLowStock;
        private Label lblLowStockVal;

        public Form1()
        {
            InitializeComponent();
            _context = new WarehouseContext();
            _dashboardService = new DashboardService(_context);
            _productService = new ProductService(_context);

            this.FormClosed += (s, e) => _context?.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Session.IsLoggedIn)
            {
                var login = new LoginForm();
                if (login.ShowDialog() != DialogResult.OK)
                {
                    Application.Exit();
                    return;
                }
            }

            SetupRoleAccess();
            InitializeDashboardCards();
            InitializeContextMenu();

            if (cmbType.Items.Count > 0)
                cmbType.SelectedIndex = 0;

            // Events for realtime search, critical stock highlights and navigation buttons
            txtBarcode.TextChanged += TxtBarcode_TextChanged;
            dgvProducts.RowPrePaint += DgvProducts_RowPrePaint;
            btnReports.Click += BtnReports_Click;
            btnPos.Click += BtnPos_Click;
            btnReturns.Click += BtnReturns_Click;
            btnCustomers.Click += BtnCustomers_Click;

            RefreshAll();
        }

        private void SetupRoleAccess()
        {
            if (!Session.IsAdmin)
            {
                // Personel yetkileri sınırlama
                btnReports.Enabled = false;
            }
        }

        private void InitializeDashboardCards()
        {
            pnlTotalProducts = CreateCard("Toplam Ürün", Color.FromArgb(94, 148, 255), 20, 20, out lblTotalProductsVal);
            pnlDailySales = CreateCard("Bugünkü Satış", Color.FromArgb(46, 204, 113), 240, 20, out lblDailySalesVal);
            pnlLowStock = CreateCard("Kritik Stok (<5)", Color.FromArgb(231, 76, 60), 460, 20, out lblLowStockVal);

            var dashTab = new TabPage("🏠 Dashboard");
            dashTab.BackColor = Color.WhiteSmoke;
            dashTab.Controls.Add(pnlTotalProducts);
            dashTab.Controls.Add(pnlDailySales);
            dashTab.Controls.Add(pnlLowStock);

            tabControl.TabPages.Insert(0, dashTab);
            tabControl.SelectedIndex = 0;
        }

        private Guna2Panel CreateCard(string title, Color bg, int x, int y, out Label valLabel)
        {
            var pnl = new Guna2Panel
            {
                Location = new Point(x, y),
                Size = new Size(200, 100),
                BorderRadius = 15,
                FillColor = bg,
                ShadowDecoration = { Enabled = true }
            };

            var lblTitle = new Label
            {
                Text = title,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };

            valLabel = new Label
            {
                Text = "0",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new System.Drawing.Font("Segoe UI", 24, FontStyle.Bold),
                Location = new Point(15, 45),
                AutoSize = true
            };

            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(valLabel);
            return pnl;
        }

        private void InitializeContextMenu()
        {
            var menu = new ContextMenuStrip();
            var mnuEdit = new ToolStripMenuItem("Düzenle");
            var mnuDelete = new ToolStripMenuItem("Sil");
            var mnuDetails = new ToolStripMenuItem("Detay Gör");

            mnuEdit.Click += (s, e) => MessageBox.Show("Düzenleme özelliği yakında!");

            mnuDelete.Click += (s, e) =>
            {
                if (!Session.IsAdmin)
                {
                    MessageBox.Show("Bu işlem için yetkiniz yok (Personel yetkisi).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (dgvProducts.CurrentRow != null)
                {
                    long id = Convert.ToInt64(dgvProducts.CurrentRow.Cells["No"].Value);
                    _productService.DeleteProduct(id);
                    RefreshAll();
                }
            };

            mnuDetails.Click += (s, e) => MessageBox.Show("Detay özelliği yakında!");

            menu.Items.Add(mnuEdit);
            menu.Items.Add(mnuDelete);
            menu.Items.Add(mnuDetails);

            dgvProducts.ContextMenuStrip = menu;
        }

        private void TxtBarcode_TextChanged(object sender, EventArgs e)
        {
            var term = txtBarcode.Text.Trim();
            if (string.IsNullOrWhiteSpace(term))
            {
                dgvProducts.DataSource = _productService.GetAllActiveProducts()
                    .OrderBy(p => p.Id)
                    .Select(p => new { No = p.Id, Barkod = p.Barcode, Urun = p.Name, Fiyat = p.UnitPrice, Stok = p.StockQty })
                    .ToList();
            }
            else
            {
                var filtered = _productService.SearchProducts(term);
                dgvProducts.DataSource = filtered
                    .OrderBy(p => p.Id)
                    .Select(p => new { No = p.Id, Barkod = p.Barcode, Urun = p.Name, Fiyat = p.UnitPrice, Stok = p.StockQty })
                    .ToList();

                var exact = _productService.GetProductByBarcode(term);
                if (exact != null)
                {
                    txtName.Text = exact.Name;
                    txtPrice.Text = exact.UnitPrice.ToString("0.00");
                }
            }
        }

        private void DgvProducts_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (dgvProducts.Rows[e.RowIndex].Cells["Stok"].Value != null)
            {
                 int stock = Convert.ToInt32(dgvProducts.Rows[e.RowIndex].Cells["Stok"].Value);
                 if (stock < 5)
                 {
                     dgvProducts.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                     dgvProducts.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                 }
                 else
                 {
                     dgvProducts.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                     dgvProducts.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                 }
            }
        }

        private void RefreshAll()
        {
            // EF DbContext cache'ini temizleyip veritabanından en güncel (raw sql ile güncellenmiş) verileri çekmesi için yeniden yükle
            _context?.Dispose();
            _context = new WarehouseContext();
            _productService = new ProductService(_context);
            _dashboardService = new DashboardService(_context);

            // DataGridView güncellemesi
            dgvProducts.DataSource = _productService.GetAllActiveProducts()
                .OrderBy(p => p.Id)
                .Select(p => new { No = p.Id, Barkod = p.Barcode, Urun = p.Name, Fiyat = p.UnitPrice, Stok = p.StockQty })
                .ToList();
            
            dgvMovements.DataSource = _context.StockMovements.OrderByDescending(m => m.CreatedAt)
                .Select(m => new { Barkod = m.BarcodeSnapshot, İslemTipi = m.Type, Miktar = m.Quantity, Neden = m.Reason, Tarih = m.CreatedAt })
                .Take(100)
                .ToList();

            // Sütun genişliklerinin tüm boşlukları kaplayacak şekilde esnemesi
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMovements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Kart güncellemesi
            if (lblTotalProductsVal != null) lblTotalProductsVal.Text = _dashboardService.GetTotalActiveProducts().ToString();
            if (lblDailySalesVal != null) lblDailySalesVal.Text = _dashboardService.GetTodaySalesTotal().ToString("C2");
            
            if (lblLowStockVal != null) 
            {
                var lowStockProducts = _productService.GetAllActiveProducts().Where(p => p.StockQty < 5).ToList();
                int lStock = lowStockProducts.Count;
                if (lStock > 0)
                {
                    var names = string.Join(", ", lowStockProducts.Select(p => p.Name).Take(3));
                    if (lStock > 3) names += "...";
                    lblLowStockVal.Text = $"{lStock} Adet\n({names})";
                    lblLowStockVal.Font = new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold);
                }
                else
                {
                    lblLowStockVal.Text = "0";
                    lblLowStockVal.Font = new System.Drawing.Font("Segoe UI", 24, FontStyle.Bold);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var barcode = txtBarcode.Text.Trim();
            var name = txtName.Text.Trim();
            var priceText = txtPrice.Text.Trim();

            if (string.IsNullOrWhiteSpace(barcode) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Barkod ve ürün adı zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(priceText, out var price) || price < 0)
            {
                MessageBox.Show("Geçerli bir birim fiyat girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existingProduct = _productService.GetProductByBarcode(barcode);
            if (existingProduct != null)
            {
                existingProduct.Name = name;
                existingProduct.UnitPrice = (double)price;
                _productService.UpdateProduct(existingProduct);
                MessageBox.Show("Mevcut ürün sistemde bulundu ve isim/fiyat bilgileri başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshAll();
                txtBarcode.Clear();
                txtName.Clear();
                txtPrice.Clear();
                txtBarcode.Focus();
                return;
            }

            var p = new Product { Barcode = barcode, Name = name, UnitPrice = (double)price };
            _productService.AddProduct(p);
            
            // Zen.Barcode Yazdırma Modülü
            try
            {
                Zen.Barcode.Code128BarcodeDraw br = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                System.Drawing.Image img = br.Draw(barcode, 60);

                var f = new Form { Text = "Otomatik Barkod Etiketi: " + barcode, Size = new Size(300, 200), StartPosition = FormStartPosition.CenterParent };
                var pb = new PictureBox { Image = img, SizeMode = PictureBoxSizeMode.CenterImage, Dock = DockStyle.Fill };
                var lbl = new Label { Text = name + " - " + priceText + " TL", Dock = DockStyle.Bottom, TextAlign = ContentAlignment.MiddleCenter, Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold) };
                f.Controls.Add(pb);
                f.Controls.Add(lbl);
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Barkod gösteriminde hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            RefreshAll();

            txtBarcode.Clear();
            txtName.Clear();
            txtPrice.Clear();
            txtBarcode.Focus();
        }

        private void btnAddMovement_Click(object sender, EventArgs e)
        {
            var barcode = txtBarcodeMovement.Text.Trim();
            var prod = _productService.GetProductByBarcode(barcode);
            if (prod == null)
            {
                MessageBox.Show("Bu barkoda sahip bir ürün bulunamadı. Önce ürün kartını oluşturun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int qty = (int)nudQuantity.Value;
            string type = cmbType.SelectedItem?.ToString() ?? "Giriş";
            
            if (type == "Çıkış")
            {
                if (prod.StockQty < qty)
                {
                    MessageBox.Show("Yetersiz stok.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                prod.StockQty -= qty;
            }
            else
            {
                prod.StockQty += qty;
            }

            _productService.UpdateProduct(prod);
            
            _context.StockMovements.Add(new StockMovement {
                ProductId = prod.Id,
                BarcodeSnapshot = barcode,
                Quantity = qty,
                Type = type,
                Reason = "Manuel Giriş",
                RefType = "Manual",
                CreatedByUserId = Session.UserId
            });
            _context.SaveChanges();

            // Log ekleme
            var ls = new LogService(_context);
            ls.Info("Stok Hareketi: " + type, $"Barkod: {barcode}, Miktar: {qty}", Session.UserId);

            RefreshAll();
            txtBarcodeMovement.Clear();
            nudQuantity.Value = 1;
            txtBarcodeMovement.Focus();
        }

        private void BtnPos_Click(object sender, EventArgs e)
        {
            new FrmPos().ShowDialog();
            RefreshAll();
        }

        private void BtnReturns_Click(object sender, EventArgs e)
        {
            new FrmReturns().ShowDialog();
            RefreshAll();
        }

        private void BtnCustomers_Click(object sender, EventArgs e)
        {
            new FrmCustomers().Show();
        }

        private void BtnReports_Click(object sender, EventArgs e)
        {
            if (!Session.IsAdmin)
            {
                MessageBox.Show("Bu işlem için yetkiniz yok.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var doc = new Document();
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Stok_Raporu_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");
                PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
                doc.Open();

                var titleFont = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD);
                var contentFont = FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.NORMAL);

                doc.Add(new Paragraph("Depo Stok Raporu - " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"), titleFont));
                doc.Add(new Paragraph("---------------------------------------------------------------------------------------------------------"));
                doc.Add(new Paragraph("\n"));

                var products = _productService.GetAllActiveProducts();
                foreach (var p in products)
                {
                    string info = string.Format("Barkod: {0,-15} | Miktar: {1,-5} | Fiyat: {2,-8} TL | Ürün: {3}", p.Barcode, p.StockQty, p.UnitPrice, p.Name);
                    doc.Add(new Paragraph(info, contentFont));
                }
                
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("Toplam Listelenen Ürün Tipi: " + products.Count, titleFont));
                
                doc.Close();
                MessageBox.Show("Rapor başarıyla masaüstüne PDF olarak kaydedildi!\n" + path, "Rapor Alındı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Rapor oluşturulurken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
