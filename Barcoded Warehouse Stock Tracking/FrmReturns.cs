using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Barcoded_Warehouse_Stock_Tracking
{
    public class FrmReturns : Form
    {
        private readonly TextBox _txtSaleNo = new TextBox();
        private readonly Button _btnLoad = new Button();
        private readonly DataGridView _grid = new DataGridView();
        private readonly Button _btnReturn = new Button();
        private DataTable _dt;

        public FrmReturns()
        {
            Text = "İade / İptal";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 980;
            Height = 650;

            var top = new Panel { Dock = DockStyle.Top, Height = 70, Padding = new Padding(12) };
            var bottom = new Panel { Dock = DockStyle.Bottom, Height = 70, Padding = new Padding(12) };
            var center = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12) };

            var lbl = new Label { Text = "Fiş / Satış No", AutoSize = true, Location = new Point(12, 10) };
            _txtSaleNo.Location = new Point(12, 30);
            _txtSaleNo.Width = 360;

            _btnLoad.Text = "Satışı Bul";
            _btnLoad.Location = new Point(390, 28);
            _btnLoad.Size = new Size(120, 30);
            _btnLoad.Click += (_, __) => LoadSale();

            top.Controls.Add(lbl);
            top.Controls.Add(_txtSaleNo);
            top.Controls.Add(_btnLoad);

            _grid.Dock = DockStyle.Fill;
            _grid.AllowUserToAddRows = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.MultiSelect = false;
            _grid.AutoGenerateColumns = false;

            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Barcode", HeaderText = "Barkod", Width = 140, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Ürün", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "UnitPrice", HeaderText = "Fiyat", Width = 90, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoldQty", HeaderText = "Satılan", Width = 70, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ReturnedQty", HeaderText = "İade", Width = 70, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ReturnQty", HeaderText = "İade Adet", Width = 90, ReadOnly = false });

            center.Controls.Add(_grid);

            _btnReturn.Text = "İadeyi Kaydet (Stok Geri Al)";
            _btnReturn.Dock = DockStyle.Right;
            _btnReturn.Width = 260;
            _btnReturn.Click += (_, __) => SaveReturn();

            var info = new Label
            {
                Text = "Not: Sadece 'Completed' satışlardan iade yapılır. Kısmi iade desteklenir.",
                AutoSize = true,
                ForeColor = Color.DimGray,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft
            };

            bottom.Controls.Add(_btnReturn);
            bottom.Controls.Add(info);

            Controls.Add(center);
            Controls.Add(bottom);
            Controls.Add(top);
        }

        private void LoadSale()
        {
            var saleNo = _txtSaleNo.Text.Trim();
            if (string.IsNullOrWhiteSpace(saleNo))
            {
                MessageBox.Show("Satış no zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _dt = Database.GetSaleItemsForReturn(saleNo);
            if (_dt.Rows.Count == 0)
            {
                MessageBox.Show("Satış bulunamadı veya iade edilemez (status Completed olmalı).", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _grid.DataSource = null;
                return;
            }

            if (!_dt.Columns.Contains("ReturnQty"))
            {
                _dt.Columns.Add("ReturnQty", typeof(int));
            }

            foreach (DataRow row in _dt.Rows)
            {
                row["ReturnQty"] = 0;
            }

            _grid.DataSource = _dt;
        }

        private void SaveReturn()
        {
            if (_dt == null || _dt.Rows.Count == 0)
            {
                MessageBox.Show("Önce satış yükleyin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var saleNo = _txtSaleNo.Text.Trim();

            var items = new List<Database.ReturnItemInput>();
            foreach (DataRow row in _dt.Rows)
            {
                int sold = Convert.ToInt32(row["SoldQty"]);
                int returned = Convert.ToInt32(row["ReturnedQty"]);
                int max = sold - returned;
                int qty = 0;

                if (row["ReturnQty"] != DBNull.Value)
                {
                    qty = Convert.ToInt32(row["ReturnQty"]);
                }

                if (qty <= 0) continue;
                if (qty > max)
                {
                    MessageBox.Show("İade adedi satılan-adet(geri kalan) değerini aşamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                items.Add(new Database.ReturnItemInput
                {
                    SaleItemId = Convert.ToInt64(row["SaleItemId"]),
                    ProductId = Convert.ToInt64(row["ProductId"]),
                    BarcodeSnapshot = Convert.ToString(row["Barcode"]),
                    Quantity = qty,
                    UnitPrice = Convert.ToDouble(row["UnitPrice"], CultureInfo.InvariantCulture)
                });
            }

            if (items.Count == 0)
            {
                MessageBox.Show("İade edilecek adet girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var returnNo = DateTime.Now.ToString("yyyyMMddHHmmss") + "-R-" + Guid.NewGuid().ToString("N").Substring(0, 4);
            try
            {
                Database.CreateReturn(saleNo, returnNo, items, Session.UserId ?? 0);
                MessageBox.Show($"İade kaydedildi.\nİade No: {returnNo}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSale();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "İade Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

