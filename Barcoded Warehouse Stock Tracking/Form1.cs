using System;
using System.Windows.Forms;

namespace Barcoded_Warehouse_Stock_Tracking
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (cmbType.Items.Count > 0)
            {
                cmbType.SelectedIndex = 0;
            }
            RefreshProducts();
            RefreshMovements();
        }

        private void RefreshProducts()
        {
            dgvProducts.DataSource = Database.GetProducts();
        }

        private void RefreshMovements()
        {
            dgvMovements.DataSource = Database.GetMovements();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var barcode = txtBarcode.Text.Trim();
            var name = txtName.Text.Trim();
            var priceText = txtPrice.Text.Trim();

            if (string.IsNullOrWhiteSpace(barcode) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Barkod ve ürün adı zorunludur.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(priceText, out var price) || price < 0)
            {
                MessageBox.Show("Geçerli bir birim fiyat girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Database.ProductExists(barcode))
            {
                MessageBox.Show("Bu barkoda sahip bir ürün zaten mevcut.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Database.InsertProduct(barcode, name, price);
            RefreshProducts();

            txtBarcode.Clear();
            txtName.Clear();
            txtPrice.Clear();
            txtBarcode.Focus();
        }

        private void btnAddMovement_Click(object sender, EventArgs e)
        {
            var barcode = txtBarcodeMovement.Text.Trim();
            if (string.IsNullOrWhiteSpace(barcode))
            {
                MessageBox.Show("Barkod zorunludur.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Database.ProductExists(barcode))
            {
                MessageBox.Show("Bu barkoda sahip bir ürün bulunamadı. Önce ürün kartını oluşturun.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int quantity = (int)nudQuantity.Value;
            string type = cmbType.SelectedItem?.ToString() ?? "Giriş";

            Database.InsertMovement(barcode, quantity, type, DateTime.Now);
            RefreshMovements();

            txtBarcodeMovement.Clear();
            nudQuantity.Value = 1;
            txtBarcodeMovement.Focus();
        }
    }
}
