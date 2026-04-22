using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Barcoded_Warehouse_Stock_Tracking
{
    public class FrmCustomers : Form
    {
        private readonly DataGridView _grid = new DataGridView();
        private readonly TextBox _txtName = new TextBox();
        private readonly TextBox _txtPhone = new TextBox();
        private readonly TextBox _txtEmail = new TextBox();
        private readonly Button _btnAdd = new Button();

        private readonly ComboBox _cmbCustomer = new ComboBox();
        private readonly TextBox _txtAmount = new TextBox();
        private readonly ComboBox _cmbMethod = new ComboBox();
        private readonly Button _btnCollect = new Button();

        private DataTable _dt;

        public FrmCustomers()
        {
            Text = "Müşteriler / Cari";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 980;
            Height = 650;

            var left = new Panel { Dock = DockStyle.Left, Width = 420, Padding = new Padding(12) };
            var right = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12) };

            var lblAdd = new Label { Text = "Yeni Müşteri", AutoSize = true, Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold) };
            var lblName = new Label { Text = "Ad Soyad", AutoSize = true, Location = new Point(12, 40) };
            _txtName.Location = new Point(12, 60);
            _txtName.Width = 380;
            var lblPhone = new Label { Text = "Telefon", AutoSize = true, Location = new Point(12, 95) };
            _txtPhone.Location = new Point(12, 115);
            _txtPhone.Width = 380;
            var lblEmail = new Label { Text = "E-posta", AutoSize = true, Location = new Point(12, 150) };
            _txtEmail.Location = new Point(12, 170);
            _txtEmail.Width = 380;

            _btnAdd.Text = "Müşteri Ekle";
            _btnAdd.Location = new Point(12, 210);
            _btnAdd.Size = new Size(380, 32);
            _btnAdd.Click += (_, __) => AddCustomer();

            var sep = new Label { Text = "Tahsilat", AutoSize = true, Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), Location = new Point(12, 270) };

            _cmbCustomer.Location = new Point(12, 300);
            _cmbCustomer.Width = 380;
            _cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;

            _cmbMethod.Location = new Point(12, 335);
            _cmbMethod.Width = 180;
            _cmbMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbMethod.Items.AddRange(new object[] { "Cash", "Card" });
            _cmbMethod.SelectedIndex = 0;

            _txtAmount.Location = new Point(212, 335);
            _txtAmount.Width = 180;
            _txtAmount.Text = "0";

            _btnCollect.Text = "Tahsilat Kaydet";
            _btnCollect.Location = new Point(12, 370);
            _btnCollect.Size = new Size(380, 32);
            _btnCollect.Click += (_, __) => Collect();

            left.Controls.Add(lblAdd);
            left.Controls.Add(lblName);
            left.Controls.Add(_txtName);
            left.Controls.Add(lblPhone);
            left.Controls.Add(_txtPhone);
            left.Controls.Add(lblEmail);
            left.Controls.Add(_txtEmail);
            left.Controls.Add(_btnAdd);
            left.Controls.Add(sep);
            left.Controls.Add(_cmbCustomer);
            left.Controls.Add(_cmbMethod);
            left.Controls.Add(_txtAmount);
            left.Controls.Add(_btnCollect);

            _grid.Dock = DockStyle.Fill;
            _grid.AllowUserToAddRows = false;
            _grid.ReadOnly = true;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.AutoGenerateColumns = true;
            right.Controls.Add(_grid);

            Controls.Add(right);
            Controls.Add(left);

            LoadCustomers();
        }

        private void LoadCustomers()
        {
            _dt = Database.GetCustomers();
            _grid.DataSource = _dt;

            _cmbCustomer.DataSource = _dt;
            _cmbCustomer.DisplayMember = "Name";
            _cmbCustomer.ValueMember = "Id";
        }

        private void AddCustomer()
        {
            var name = _txtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Ad soyad zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Database.InsertCustomer(name, _txtPhone.Text.Trim(), _txtEmail.Text.Trim());

            _txtName.Clear();
            _txtPhone.Clear();
            _txtEmail.Clear();
            LoadCustomers();
        }

        private static double ParseMoney(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out var v)) return v;
            if (double.TryParse(text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out v)) return v;
            return 0;
        }

        private void Collect()
        {
            if (_cmbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Müşteri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var customerId = Convert.ToInt64(_cmbCustomer.SelectedValue);
            var method = _cmbMethod.SelectedItem?.ToString() ?? "Cash";
            var amount = Math.Max(0, ParseMoney(_txtAmount.Text));
            if (amount <= 0)
            {
                MessageBox.Show("Tutar zorunludur.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Database.AddCollection(customerId, method, amount, Session.UserId ?? 0);
            _txtAmount.Text = "0";
            LoadCustomers();
        }
    }
}

