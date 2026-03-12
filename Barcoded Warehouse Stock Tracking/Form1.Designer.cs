namespace Barcoded_Warehouse_Stock_Tracking
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabProducts = new System.Windows.Forms.TabPage();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.tabMovements = new System.Windows.Forms.TabPage();
            this.dgvMovements = new System.Windows.Forms.DataGridView();
            this.btnAddMovement = new System.Windows.Forms.Button();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.nudQuantity = new System.Windows.Forms.NumericUpDown();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.txtBarcodeMovement = new System.Windows.Forms.TextBox();
            this.lblBarcodeMovement = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.tabMovements.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovements)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabProducts);
            this.tabControl.Controls.Add(this.tabMovements);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(900, 550);
            this.tabControl.TabIndex = 0;
            // 
            // ===== tabProducts (Ürünler) =====
            // 
            this.tabProducts.Controls.Add(this.dgvProducts);
            this.tabProducts.Controls.Add(this.btnAdd);
            this.tabProducts.Controls.Add(this.txtPrice);
            this.tabProducts.Controls.Add(this.txtName);
            this.tabProducts.Controls.Add(this.txtBarcode);
            this.tabProducts.Controls.Add(this.lblPrice);
            this.tabProducts.Controls.Add(this.lblName);
            this.tabProducts.Controls.Add(this.lblBarcode);
            this.tabProducts.Location = new System.Drawing.Point(4, 22);
            this.tabProducts.Name = "tabProducts";
            this.tabProducts.Padding = new System.Windows.Forms.Padding(12);
            this.tabProducts.Size = new System.Drawing.Size(892, 524);
            this.tabProducts.TabIndex = 0;
            this.tabProducts.Text = "Ürünler";
            this.tabProducts.UseVisualStyleBackColor = true;
            // 
            // lblBarcode
            // 
            this.lblBarcode.AutoSize = true;
            this.lblBarcode.Location = new System.Drawing.Point(16, 20);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(50, 13);
            this.lblBarcode.TabIndex = 0;
            this.lblBarcode.Text = "Barkod:";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(100, 17);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(200, 20);
            this.txtBarcode.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(16, 50);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(61, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Ürün Adı:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(100, 47);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(200, 20);
            this.txtName.TabIndex = 3;
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(16, 80);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(63, 13);
            this.lblPrice.TabIndex = 4;
            this.lblPrice.Text = "Birim Fiyat:";
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(100, 77);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(200, 20);
            this.txtPrice.TabIndex = 5;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(100, 110);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(200, 30);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Ürünü Kaydet";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgvProducts
            // 
            this.dgvProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new System.Drawing.Point(16, 155);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.Size = new System.Drawing.Size(860, 355);
            this.dgvProducts.TabIndex = 7;
            // 
            // ===== tabMovements (Stok Hareketleri) =====
            // 
            this.tabMovements.Controls.Add(this.dgvMovements);
            this.tabMovements.Controls.Add(this.btnAddMovement);
            this.tabMovements.Controls.Add(this.cmbType);
            this.tabMovements.Controls.Add(this.lblType);
            this.tabMovements.Controls.Add(this.nudQuantity);
            this.tabMovements.Controls.Add(this.lblQuantity);
            this.tabMovements.Controls.Add(this.txtBarcodeMovement);
            this.tabMovements.Controls.Add(this.lblBarcodeMovement);
            this.tabMovements.Location = new System.Drawing.Point(4, 22);
            this.tabMovements.Name = "tabMovements";
            this.tabMovements.Padding = new System.Windows.Forms.Padding(12);
            this.tabMovements.Size = new System.Drawing.Size(892, 524);
            this.tabMovements.TabIndex = 1;
            this.tabMovements.Text = "Stok Hareketleri";
            this.tabMovements.UseVisualStyleBackColor = true;
            // 
            // lblBarcodeMovement
            // 
            this.lblBarcodeMovement.AutoSize = true;
            this.lblBarcodeMovement.Location = new System.Drawing.Point(16, 20);
            this.lblBarcodeMovement.Name = "lblBarcodeMovement";
            this.lblBarcodeMovement.Size = new System.Drawing.Size(50, 13);
            this.lblBarcodeMovement.TabIndex = 0;
            this.lblBarcodeMovement.Text = "Barkod:";
            // 
            // txtBarcodeMovement
            // 
            this.txtBarcodeMovement.Location = new System.Drawing.Point(100, 17);
            this.txtBarcodeMovement.Name = "txtBarcodeMovement";
            this.txtBarcodeMovement.Size = new System.Drawing.Size(200, 20);
            this.txtBarcodeMovement.TabIndex = 1;
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(16, 50);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(46, 13);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "Miktar:";
            // 
            // nudQuantity
            // 
            this.nudQuantity.Location = new System.Drawing.Point(100, 48);
            this.nudQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudQuantity.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.nudQuantity.Name = "nudQuantity";
            this.nudQuantity.Size = new System.Drawing.Size(100, 20);
            this.nudQuantity.TabIndex = 3;
            this.nudQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(16, 80);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(32, 13);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Tür:";
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] { "Giriş", "Çıkış" });
            this.cmbType.Location = new System.Drawing.Point(100, 77);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(121, 21);
            this.cmbType.TabIndex = 5;
            // 
            // btnAddMovement
            // 
            this.btnAddMovement.Location = new System.Drawing.Point(100, 110);
            this.btnAddMovement.Name = "btnAddMovement";
            this.btnAddMovement.Size = new System.Drawing.Size(200, 30);
            this.btnAddMovement.TabIndex = 6;
            this.btnAddMovement.Text = "Hareket Kaydet";
            this.btnAddMovement.UseVisualStyleBackColor = true;
            this.btnAddMovement.Click += new System.EventHandler(this.btnAddMovement_Click);
            // 
            // dgvMovements
            // 
            this.dgvMovements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMovements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMovements.Location = new System.Drawing.Point(16, 155);
            this.dgvMovements.Name = "dgvMovements";
            this.dgvMovements.ReadOnly = true;
            this.dgvMovements.AllowUserToAddRows = false;
            this.dgvMovements.Size = new System.Drawing.Size(860, 355);
            this.dgvMovements.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 550);
            this.Controls.Add(this.tabControl);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Barkodlu Depo Stok Takip";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl.ResumeLayout(false);
            this.tabProducts.ResumeLayout(false);
            this.tabProducts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.tabMovements.ResumeLayout(false);
            this.tabMovements.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovements)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabProducts;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.TabPage tabMovements;
        private System.Windows.Forms.DataGridView dgvMovements;
        private System.Windows.Forms.Button btnAddMovement;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.NumericUpDown nudQuantity;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.TextBox txtBarcodeMovement;
        private System.Windows.Forms.Label lblBarcodeMovement;
    }
}
