using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Barcoded_Warehouse_Stock_Tracking
{
    public class LoginForm : Form
    {
        private readonly Guna2TextBox _txtUser = new Guna2TextBox();
        private readonly Guna2TextBox _txtPass = new Guna2TextBox();
        private readonly Guna2Button _btnLogin = new Guna2Button();
        private readonly Label _lblError = new Label();

        public LoginForm()
        {
            // --- Form Ayarları ---
            Text = "Poseidon Yazılım - Giriş";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            ClientSize = new Size(460, 620);
            BackColor = Color.FromArgb(26, 26, 46);

            // --- Başlık Grubu ---
            var lblBrand = new Label
            {
                Text = "Poseidon Yazılım",
                ForeColor = Color.FromArgb(233, 69, 96),
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(460, 50),
                Location = new Point(0, 40)
            };

            var lblSub = new Label
            {
                Text = "Depo & Stok Yönetim Sistemi",
                ForeColor = Color.FromArgb(180, 180, 200),
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(460, 25),
                Location = new Point(0, 90)
            };

            // --- KART PANELİ (Ana Taşıyıcı) ---
            var card = new Guna2Panel
            {
                Size = new Size(380, 420),
                Location = new Point(40, 130),
                FillColor = Color.FromArgb(22, 33, 62),
                BorderRadius = 20,
                ShadowDecoration = {
                    Enabled = true,
                    Color = Color.FromArgb(233, 69, 96),
                    Depth = 15,
                    BorderRadius = 20
                }
            };

            // --- HİZALAMA AYARLARI (Matematiksel Orta) ---
            int cardW = card.Width;    // 380
            int ctrlW = 300;           // İçerideki kutuların genişliği (Taşmaması için biraz daralttık)
            int xPos = (cardW - ctrlW) / 2; // (380 - 300) / 2 = 40 (Tam orta)

            var lblTitle = new Label
            {
                Text = "Hesabınıza Giriş Yapın",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(ctrlW, 40),
                Location = new Point(xPos, 30)
            };

            // --- Kullanıcı Adı ---
            _txtUser.Size = new Size(ctrlW, 50);
            _txtUser.Location = new Point(xPos, 100);
            _txtUser.PlaceholderText = "Kullanıcı Adı";
            _txtUser.BorderRadius = 10;
            _txtUser.FillColor = Color.FromArgb(35, 45, 78);
            _txtUser.BorderColor = Color.FromArgb(60, 80, 120);
            _txtUser.ForeColor = Color.White;
            _txtUser.TextOffset = new Point(10, 0);

            // --- Şifre ---
            _txtPass.Size = new Size(ctrlW, 50);
            _txtPass.Location = new Point(xPos, 165); // Üst kutudan 15px boşluk
            _txtPass.PlaceholderText = "Şifre";
            _txtPass.UseSystemPasswordChar = true;
            _txtPass.BorderRadius = 10;
            _txtPass.FillColor = Color.FromArgb(35, 45, 78);
            _txtPass.BorderColor = Color.FromArgb(60, 80, 120);
            _txtPass.ForeColor = Color.White;
            _txtPass.TextOffset = new Point(10, 0);

            // --- Giriş Butonu ---
            _btnLogin.Text = "SİSTEME GİRİŞ YAP";
            _btnLogin.Size = new Size(ctrlW, 55);
            _btnLogin.Location = new Point(xPos, 260); // Buton ile kutular arası mesafe
            _btnLogin.BorderRadius = 12;
            _btnLogin.FillColor = Color.FromArgb(233, 69, 96);
            _btnLogin.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            _btnLogin.Cursor = Cursors.Hand;
            _btnLogin.Click += (_, __) => DoLogin();

            // --- Hata Mesajı ---
            _lblError.Size = new Size(ctrlW, 25);
            _lblError.Location = new Point(xPos, 330);
            _lblError.ForeColor = Color.FromArgb(233, 69, 96);
            _lblError.TextAlign = ContentAlignment.MiddleCenter;
            _lblError.Text = "";

            // Kontrolleri Kartın İçine Ekle
            card.Controls.Add(lblTitle);
            card.Controls.Add(_txtUser);
            card.Controls.Add(_txtPass);
            card.Controls.Add(_btnLogin);
            card.Controls.Add(_lblError);

            // Sayfa Alt Yazısı
            var lblFooter = new Label
            {
                Text = "© 2025 Poseidon Yazılım",
                ForeColor = Color.FromArgb(100, 100, 130),
                Font = new Font("Segoe UI", 8),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(460, 20),
                Location = new Point(0, 565)
            };

            // Ana Forma Ekle
            Controls.Add(lblBrand);
            Controls.Add(lblSub);
            Controls.Add(card);
            Controls.Add(lblFooter);

            AcceptButton = _btnLogin;
        }

        private void DoLogin()
        {
            var user = _txtUser.Text.Trim();
            var pass = _txtPass.Text;

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                _lblError.Text = "Lütfen tüm alanları doldurun.";
                return;
            }

            try
            {
                var result = Database.AuthenticateUser(user, pass);
                if (result.HasValue)
                {
                    Session.UserId = result.Value.Id;
                    Session.Username = user;
                    Session.Role = result.Value.Role;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    _lblError.Text = "Kullanıcı adı veya şifre hatalı!";
                    _txtPass.Clear();
                    _txtPass.Focus();
                }
            }
            catch (Exception ex)
            {
                _lblError.Text = "Giriş sırasında hata oluştu.";
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}