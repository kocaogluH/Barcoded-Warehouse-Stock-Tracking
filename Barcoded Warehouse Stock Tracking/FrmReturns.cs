using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Guna.UI2.WinForms;

namespace Barcoded_Warehouse_Stock_Tracking
{
    public class FrmReturns : Form
    {
        private static readonly Color BgDark = Color.FromArgb(26, 26, 46);
        private static readonly Color BgMid = Color.FromArgb(22, 33, 62);
        private static readonly Color BgInput = Color.FromArgb(35, 45, 78);
        private static readonly Color Accent = Color.FromArgb(233, 69, 96);
        private static readonly Color AccentBlu = Color.FromArgb(52, 152, 219);
        private static readonly Color AccentOrg = Color.FromArgb(230, 126, 34);
        private static readonly Color TextMain = Color.FromArgb(234, 234, 234);
        private static readonly Color TextDim = Color.FromArgb(140, 140, 160);

        private readonly Guna2TextBox _txtSaleNo = new Guna2TextBox();
        private readonly Guna2Button _btnLoad = new Guna2Button();
        private readonly Guna2DataGridView _grid = new Guna2DataGridView();
        private readonly Guna2Button _btnReturn = new Guna2Button();
        private DataTable _dt;

        public FrmReturns()
        {
            Text = "Poseidon Yazılım — İade / İptal";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1000; Height = 700;
            BackColor = BgDark;

            // ── TOP PANEL (Search) ──────────────────────────────────────────────
            var top = new Panel { Dock = DockStyle.Top, Height = 140, BackColor = BgMid, Padding = new Padding(20) };

            var lblHead = new Label
            {
                Text = "↩  İade / İptal İşlemi",
                ForeColor = Accent, Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true, Location = new Point(25, 20)
            };

            var lblFis = new Label { Text = "Sorgulanacak Fiş / Satış No", ForeColor = TextDim, AutoSize = true, Location = new Point(25, 65), Font = new Font("Segoe UI", 9) };

            _txtSaleNo.Location = new Point(25, 78);
            _txtSaleNo.Width = 320; _txtSaleNo.Height = 38;
            _txtSaleNo.PlaceholderText = "🔍 Fiş No Yazın...";
            _txtSaleNo.BorderRadius = 8; _txtSaleNo.Font = new Font("Segoe UI", 10);
            _txtSaleNo.FillColor = BgInput; _txtSaleNo.BorderColor = AccentBlu;
            _txtSaleNo.ForeColor = TextMain; _txtSaleNo.PlaceholderForeColor = TextDim;
            _txtSaleNo.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; LoadSale(); } };

            _btnLoad.Text = "SATIŞI BUL";
            _btnLoad.Location = new Point(355, 88); _btnLoad.Size = new Size(130, 38);
            _btnLoad.BorderRadius = 8; _btnLoad.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            _btnLoad.FillColor = AccentBlu; _btnLoad.ForeColor = Color.White;
            _btnLoad.Click += (_, __) => LoadSale();

            top.Controls.Add(lblHead);
            top.Controls.Add(lblFis);
            top.Controls.Add(_txtSaleNo);
            top.Controls.Add(_btnLoad);

            // ── CENTER (Grid) ───────────────────────────────────────────────────
            var center = new Panel { Dock = DockStyle.Fill, BackColor = BgDark, Padding = new Padding(15) };
            _grid.Dock = DockStyle.Fill;
            _grid.AllowUserToAddRows = false;
            _grid.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Dark;
            _grid.ThemeStyle.BackColor = BgMid;
            _grid.ThemeStyle.RowsStyle.BackColor = BgMid;
            _grid.ThemeStyle.RowsStyle.ForeColor = TextMain;
            _grid.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(22, 33, 62);
            _grid.ThemeStyle.HeaderStyle.ForeColor = Accent;
            _grid.BorderStyle = BorderStyle.None;
            _grid.RowHeadersVisible = false;

            // --- Modern Styling ---
            _grid.RowTemplate.Height = 35;
            _grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _grid.GridColor = Color.FromArgb(40, 55, 90);
            _grid.ColumnHeadersHeight = 40;
            _grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            _grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 80, 140);
            _grid.DefaultCellStyle.SelectionForeColor = Color.White;

            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Barcode",    HeaderText = "Barkod",     Width = 150, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name",       HeaderText = "Ürün",       AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "UnitPrice",  HeaderText = "Fiyat",      Width = 100, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoldQty",    HeaderText = "Satılan",    Width = 80, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ReturnedQty",HeaderText = "İade Edilen",Width = 100, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ReturnQty",  HeaderText = "İade Adet",  Width = 100, ReadOnly = false });

            center.Controls.Add(_grid);

            // ── BOTTOM PANEL (Action) ──────────────────────────────────────────
            var bottom = new Panel { Dock = DockStyle.Bottom, Height = 90, BackColor = BgMid, Padding = new Padding(20) };

            var info = new Label
            {
                Text = "ℹ  Sadece 'Completed' satışlardan iade yapılır. Kısmi iade desteklenir.",
                ForeColor = TextDim, Font = new Font("Segoe UI", 9, FontStyle.Italic),
                AutoSize = true, Location = new Point(20, 35)
            };

            _btnReturn.Text = "↩  İadeyi Kaydet (Stok Geri Al)";
            _btnReturn.Location = new Point(680, 20); _btnReturn.Size = new Size(280, 50);
            _btnReturn.BorderRadius = 12; _btnReturn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            _btnReturn.FillColor = AccentOrg; _btnReturn.ForeColor = Color.White;
            _btnReturn.Click += (_, __) => SaveReturn();

            bottom.Controls.Add(info);
            bottom.Controls.Add(_btnReturn);

            Controls.Add(center);
            Controls.Add(bottom);
            Controls.Add(top);
        }

        private void LoadSale()
        {
            var saleNo = _txtSaleNo.Text.Trim();
            if (string.IsNullOrEmpty(saleNo)) return;
            _dt = Database.GetSaleItemsForReturn(saleNo);
            if (_dt.Rows.Count == 0) { MessageBox.Show("Satış bulunamadı!"); return; }
            if (!_dt.Columns.Contains("ReturnQty")) _dt.Columns.Add("ReturnQty", typeof(int));
            foreach (DataRow r in _dt.Rows) r["ReturnQty"] = 0;
            _grid.DataSource = _dt;
        }

        private void SaveReturn()
        {
            if (_dt == null || _grid.Rows.Count == 0) return;
            try
            {
                var items = new System.Collections.Generic.List<Database.ReturnItemInput>();
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
                if (items.Count == 0) return;
                Database.CreateReturn(_txtSaleNo.Text.Trim(), "RET" + DateTime.Now.Ticks, items, Session.UserId ?? 0);
                MessageBox.Show("İade işlemi tamamlandı.");
                LoadSale();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
