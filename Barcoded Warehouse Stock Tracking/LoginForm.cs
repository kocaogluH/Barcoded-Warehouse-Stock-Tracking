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
            ClientSize = new Size(460, 700);
            BackColor = Color.FromArgb(26, 26, 46);

            // --- Logo (Dairesel) ---
            int logoSize = 130;
            var pbLogo = new PictureBox
            {
                Size = new Size(logoSize, logoSize),
                Location = new Point((460 - logoSize) / 2, 16),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };

            // Dairesel Region maskesi
            var circlePath = new System.Drawing.Drawing2D.GraphicsPath();
            circlePath.AddEllipse(0, 0, logoSize, logoSize);
            pbLogo.Region = new Region(circlePath);

            // Cover modu: logo daire çerçevesini tamamen doldurur, boşluk kalmaz
            pbLogo.Paint += (s, pe) =>
            {
                if (pbLogo.Image == null) return;
                pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path.AddEllipse(0, 0, pbLogo.Width - 1, pbLogo.Height - 1);
                    pe.Graphics.SetClip(path);
                    var img = pbLogo.Image;
                    // Cover: daire tamamen dolar, kenarlar kırpılabilir
                    float scale = Math.Max((float)pbLogo.Width / img.Width, (float)pbLogo.Height / img.Height);
                    float drawW = img.Width * scale;
                    float drawH = img.Height * scale;
                    float drawX = (pbLogo.Width - drawW) / 2f;
                    float drawY = (pbLogo.Height - drawH) / 2f;
                    pe.Graphics.DrawImage(img, drawX, drawY, drawW, drawH);
                }
            };

            try
            {
                string exeDir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                string[] logoPaths = new[]
                {
                    // 1. Proje Resources klasörü
                    System.IO.Path.Combine(exeDir, "Resources", "poseidon_logo.png"),
                    System.IO.Path.Combine(exeDir, "poseidon_logo.png"),
                    // 2. Şeffaf arka planlı üretilmiş amblem (en temiz görünüm)
                    @"C:\Users\Halil Kocaoğlu\.gemini\antigravity\brain\24d4abda-eeb1-454e-b8a7-49b150518e71\poseidon_emblem_clean_1776967179886.png",
                    // 3. Orijinal logo (arka planlı)
                    @"C:\Users\Halil Kocaoğlu\OneDrive\Masaüstü\iş\Poseidon Otomasyon&Yazılım\Logo-2-.png"
                };
                foreach (var path in logoPaths)
                {
                    if (System.IO.File.Exists(path))
                    {
                        pbLogo.Image = System.Drawing.Image.FromFile(path);
                        break;
                    }
                }
            }
            catch { }


            // --- Başlık Grubu ---
            var lblBrand = new Label
            {
                Text = "Poseidon Yazılım",
                ForeColor = Color.FromArgb(233, 69, 96),
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(460, 44),
                Location = new Point(0, 148)
            };

            var lblSub = new Label
            {
                Text = "Depo & Stok Yönetim Sistemi",
                ForeColor = Color.FromArgb(180, 180, 200),
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(460, 25),
                Location = new Point(0, 195)
            };

            // --- KART PANELİ (Ana Taşıyıcı) ---
            var card = new Guna2Panel
            {
                Size = new Size(380, 400),
                Location = new Point(40, 228),
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
                Text = "© 2026 Poseidon Yazılım — Software & Automation",
                ForeColor = Color.FromArgb(100, 100, 130),
                Font = new Font("Segoe UI", 8),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(460, 20),
                Location = new Point(0, 650)
            };

            // Ana Forma Ekle
            Controls.Add(pbLogo);
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