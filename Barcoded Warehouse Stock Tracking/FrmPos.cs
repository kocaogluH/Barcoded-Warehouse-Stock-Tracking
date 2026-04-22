using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Barcoded_Warehouse_Stock_Tracking
{
    public class FrmPos : Form
    {
        private readonly TextBox _txtBarcode = new TextBox();
        private readonly NumericUpDown _nudQty = new NumericUpDown();
        private readonly Button _btnAdd = new Button();
        private readonly DataGridView _grid = new DataGridView();
        private readonly TextBox _txtDiscount = new TextBox();
        private readonly TextBox _txtCash = new TextBox();
        private readonly TextBox _txtCard = new TextBox();
        private readonly CheckBox _chkCustomer = new CheckBox();
        private readonly ComboBox _cmbCustomer = new ComboBox();
        private readonly Button _btnCustomers = new Button();
        private readonly Label _lblTotal = new Label();
        private readonly Button _btnComplete = new Button();

        private readonly BindingSource _bs = new BindingSource();
        private readonly List<CartLine> _cart = new List<CartLine>();

        private sealed class CartLine
        {
            public long ProductId { get; set; }
            public string Barcode { get; set; }
            public string Name { get; set; }
            public double UnitPrice { get; set; }
            public int Quantity { get; set; }
            public double LineTotal => UnitPrice * Quantity;
        }

        public FrmPos()
        {
            Text = "Kasa / Satış (POS)";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 980;
            Height = 650;

            var top = new Panel { Dock = DockStyle.Top, Height = 80, Padding = new Padding(12) };
            var bottom = new Panel { Dock = DockStyle.Bottom, Height = 120, Padding = new Padding(12) };
            var center = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12) };

            var lblBarcode = new Label { Text = "Barkod", AutoSize = true, Location = new Point(12, 12) };
            _txtBarcode.Location = new Point(12, 32);
            _txtBarcode.Width = 260;

            var lblQty = new Label { Text = "Adet", AutoSize = true, Location = new Point(285, 12) };
            _nudQty.Location = new Point(285, 32);
            _nudQty.Width = 80;
            _nudQty.Minimum = 1;
            _nudQty.Maximum = 100000;
            _nudQty.Value = 1;

            _btnAdd.Text = "Sepete Ekle";
            _btnAdd.Location = new Point(380, 28);
            _btnAdd.Size = new Size(140, 30);
            _btnAdd.Click += (_, __) => AddToCart();

            var hint = new Label
            {
                Text = "İpucu: Barkod okuyucu klavye gibi çalışır. Barkodu yazıp Enter’a basabilirsin.",
                AutoSize = true,
                ForeColor = Color.DimGray,
                Location = new Point(540, 34)
            };

            _txtBarcode.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    AddToCart();
                }
            };

            top.Controls.Add(lblBarcode);
            top.Controls.Add(_txtBarcode);
            top.Controls.Add(lblQty);
            top.Controls.Add(_nudQty);
            top.Controls.Add(_btnAdd);
            top.Controls.Add(hint);

            _grid.Dock = DockStyle.Fill;
            _grid.AutoGenerateColumns = false;
            _grid.AllowUserToAddRows = false;
            _grid.ReadOnly = true;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.MultiSelect = false;

            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Barcode", HeaderText = "Barkod", Width = 140 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Ürün", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "UnitPrice", HeaderText = "Fiyat", Width = 90 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Quantity", HeaderText = "Adet", Width = 70 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LineTotal", HeaderText = "Tutar", Width = 90 });

            var ctx = new ContextMenuStrip();
            var miRemove = new ToolStripMenuItem("Satırı Sil");
            miRemove.Click += (_, __) => RemoveSelected();
            ctx.Items.Add(miRemove);
            _grid.ContextMenuStrip = ctx;

            _bs.DataSource = _cart;
            _grid.DataSource = _bs;
            center.Controls.Add(_grid);

            var lblDiscount = new Label { Text = "İndirim (TL)", AutoSize = true, Location = new Point(12, 12) };
            _txtDiscount.Location = new Point(12, 35);
            _txtDiscount.Width = 120;
            _txtDiscount.Text = "0";
            _txtDiscount.TextChanged += (_, __) => UpdateTotals();

            var lblCash = new Label { Text = "Nakit", AutoSize = true, Location = new Point(160, 12) };
            _txtCash.Location = new Point(160, 35);
            _txtCash.Width = 120;
            _txtCash.Text = "0";
            _txtCash.ReadOnly = true; // Elle girilmesin
            _txtCash.TextChanged += (_, __) => UpdateTotals();

            var lblCard = new Label { Text = "Kart", AutoSize = true, Location = new Point(300, 12) };
            _txtCard.Location = new Point(300, 35);
            _txtCard.Width = 120;
            _txtCard.Text = "0";
            _txtCard.TextChanged += (_, __) => UpdateTotals();

            _chkCustomer.Text = "Cari (Veresiye)";
            _chkCustomer.AutoSize = true;
            _chkCustomer.Location = new Point(12, 78);
            _chkCustomer.CheckedChanged += (_, __) => { _cmbCustomer.Enabled = _chkCustomer.Checked; _btnCustomers.Enabled = _chkCustomer.Checked; };

            _cmbCustomer.Location = new Point(130, 75);
            _cmbCustomer.Width = 290;
            _cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbCustomer.Enabled = false;

            _btnCustomers.Text = "Müşteriler";
            _btnCustomers.Location = new Point(430, 73);
            _btnCustomers.Size = new Size(110, 28);
            _btnCustomers.Enabled = false;
            _btnCustomers.Click += (_, __) =>
            {
                using (var f = new FrmCustomers())
                {
                    f.ShowDialog(this);
                }
                LoadCustomers();
            };

            _lblTotal.Location = new Point(460, 18);
            _lblTotal.Size = new Size(260, 50);
            _lblTotal.Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            _btnComplete.Text = "Satışı Tamamla";
            _btnComplete.Location = new Point(740, 25);
            _btnComplete.Size = new Size(200, 45);
            _btnComplete.Click += (_, __) => CompleteSale();

            bottom.Controls.Add(lblDiscount);
            bottom.Controls.Add(_txtDiscount);
            bottom.Controls.Add(lblCash);
            bottom.Controls.Add(_txtCash);
            bottom.Controls.Add(lblCard);
            bottom.Controls.Add(_txtCard);
            bottom.Controls.Add(_chkCustomer);
            bottom.Controls.Add(_cmbCustomer);
            bottom.Controls.Add(_btnCustomers);
            bottom.Controls.Add(_lblTotal);
            bottom.Controls.Add(_btnComplete);

            Controls.Add(center);
            Controls.Add(bottom);
            Controls.Add(top);

            LoadCustomers();
            UpdateTotals();
        }

        private void LoadCustomers()
        {
            try
            {
                var dt = Database.GetCustomers();
                _cmbCustomer.DataSource = dt;
                _cmbCustomer.DisplayMember = "Name";
                _cmbCustomer.ValueMember = "Id";
            }
            catch
            {
                _cmbCustomer.DataSource = null;
            }
        }

        private void AddToCart()
        {
            var barcode = _txtBarcode.Text.Trim();
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return;
            }

            int qty = (int)_nudQty.Value;

            if (!Database.TryGetProductForSale(barcode, out var pid, out var name, out var unitPrice, out var stock))
            {
                MessageBox.Show("Ürün bulunamadı: " + barcode, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtBarcode.SelectAll();
                _txtBarcode.Focus();
                return;
            }

            var existing = _cart.FirstOrDefault(x => x.Barcode == barcode);
            if (existing != null)
            {
                if (existing.Quantity + qty > stock)
                {
                    MessageBox.Show($"Yetersiz stok. Mevcut: {stock}", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                existing.Quantity += qty;
            }
            else
            {
                if (qty > stock)
                {
                    MessageBox.Show($"Yetersiz stok. Mevcut: {stock}", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _cart.Add(new CartLine
                {
                    ProductId = pid,
                    Barcode = barcode,
                    Name = name,
                    UnitPrice = unitPrice,
                    Quantity = qty
                });
            }

            _bs.ResetBindings(false);
            _txtBarcode.Clear();
            _nudQty.Value = 1;
            _txtBarcode.Focus();
            UpdateTotals();
        }

        private void RemoveSelected()
        {
            if (_grid.CurrentRow?.DataBoundItem is CartLine line)
            {
                _cart.Remove(line);
                _bs.ResetBindings(false);
                UpdateTotals();
            }
        }

        private static double ParseMoney(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            text = text.Trim();

            // allow both ',' and '.'
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out var v)) return v;
            if (double.TryParse(text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out v)) return v;
            return 0;
        }

        private bool _isUpdatingTotals = false;

        private void UpdateTotals()
        {
            if (_isUpdatingTotals) return;
            _isUpdatingTotals = true;

            double subtotal = _cart.Sum(x => x.LineTotal);
            double discount = Math.Max(0, ParseMoney(_txtDiscount.Text));
            if (discount > subtotal) discount = subtotal;
            double grand = subtotal - discount;

            double card = Math.Max(0, ParseMoney(_txtCard.Text));
            if (card > grand) card = grand; 

            double cash = grand - card; 
            _txtCash.Text = cash.ToString("0.00");

            double paid = cash + card;

            _lblTotal.Text = $"Toplam: {grand:0.00} TL\nÖdeme: {paid:0.00} TL";

            _isUpdatingTotals = false;
        }

        private void CompleteSale()
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("Sepet boş.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double subtotal = _cart.Sum(x => x.LineTotal);
            double discount = Math.Max(0, ParseMoney(_txtDiscount.Text));
            if (discount > subtotal) discount = subtotal;
            double grand = subtotal - discount;

            double cash = Math.Max(0, ParseMoney(_txtCash.Text));
            double card = Math.Max(0, ParseMoney(_txtCard.Text));
            double paid = cash + card;

            long? customerId = null;
            if (_chkCustomer.Checked)
            {
                if (_cmbCustomer.SelectedValue == null)
                {
                    MessageBox.Show("Cari müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                customerId = Convert.ToInt64(_cmbCustomer.SelectedValue);
                if (paid - grand > 0.01)
                {
                    MessageBox.Show("Ödeme toplamı genel toplamı aşamaz (veresiye).", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                if (Math.Abs(paid - grand) > 0.01)
                {
                    MessageBox.Show("Ödeme toplamı genel toplam ile aynı olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            var items = _cart.Select(x => new Database.SaleItemInput
            {
                ProductId = x.ProductId,
                BarcodeSnapshot = x.Barcode,
                NameSnapshot = x.Name,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity
            }).ToList();

            var pays = new List<(string Method, double Amount)>();
            if (cash > 0) pays.Add(("Cash", cash));
            if (card > 0) pays.Add(("Card", card));

            var saleNo = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + Guid.NewGuid().ToString("N").Substring(0, 6);

            try
            {
                Database.CreateSale(
                    saleNo: saleNo,
                    customerId: customerId,
                    items: items,
                    discountTotal: discount,
                    payments: pays,
                    createdByUserId: Session.UserId ?? 0);

                MessageBox.Show($"Satış kaydedildi.\nFiş No: {saleNo}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _cart.Clear();
                _bs.ResetBindings(false);
                _txtDiscount.Text = "0";
                _txtCash.Text = "0";
                _txtCard.Text = "0";
                _chkCustomer.Checked = false;
                UpdateTotals();
                _txtBarcode.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Satış Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

