using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Barcoded_Warehouse_Stock_Tracking.Business;
using Barcoded_Warehouse_Stock_Tracking.DataAccess;

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
            using (var ctx = new WarehouseContext())
            {
                if (!System.Linq.Enumerable.Any(ctx.Users))
                {
                    ctx.Users.Add(new Entities.User
                    {
                        Username = "admin",
                        PasswordHash = Security.HashPassword("1234"),
                        Role = "Admin",
                        IsActive = 1
                    });
                    try { ctx.SaveChanges(); } catch { }
                }
            }

            Text = "Giriş - Depo Stok Takip";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(400, 300);
            BackColor = Color.White;

            var lblTitle = new Label { Text = "Sisteme Giriş Yapın", AutoSize = true, Location = new Point(40, 20), Font = new Font("Segoe UI", 16, FontStyle.Bold) };
            
            _txtUser.Location = new Point(40, 70);
            _txtUser.Width = 300;
            _txtUser.Height = 40;
            _txtUser.PlaceholderText = "Kullanıcı Adı";
            _txtUser.BorderRadius = 8;
            _txtUser.Font = new Font("Segoe UI", 10);

            _txtPass.Location = new Point(40, 130);
            _txtPass.Width = 300;
            _txtPass.Height = 40;
            _txtPass.PlaceholderText = "Şifre";
            _txtPass.UseSystemPasswordChar = true;
            _txtPass.BorderRadius = 8;
            _txtPass.Font = new Font("Segoe UI", 10);

            _btnLogin.Text = "GİRİŞ YAP";
            _btnLogin.Location = new Point(40, 190);
            _btnLogin.Width = 300;
            _btnLogin.Height = 45;
            _btnLogin.BorderRadius = 8;
            _btnLogin.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            _btnLogin.Click += (_, __) => DoLogin();

            _lblError.ForeColor = Color.DarkRed;
            _lblError.AutoSize = false;
            _lblError.TextAlign = ContentAlignment.MiddleCenter;
            _lblError.Location = new Point(40, 245);
            _lblError.Size = new Size(300, 25);
            _lblError.Font = new Font("Segoe UI", 9);

            Controls.Add(lblTitle);
            Controls.Add(_txtUser);
            Controls.Add(_txtPass);
            Controls.Add(_btnLogin);
            Controls.Add(_lblError);

            AcceptButton = _btnLogin;
        }

        private void DoLogin()
        {
            _lblError.Text = "";
            var username = _txtUser.Text.Trim();
            var password = _txtPass.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                _lblError.Text = "Kullanıcı adı ve şifre zorunludur.";
                return;
            }

            using (var ctx = new WarehouseContext()) 
            {
                try
                {
                    var authService = new AuthService(ctx);
                    var user = authService.Authenticate(username, password);

                    if (user == null)
                    {
                        _lblError.Text = "Kullanıcı adı veya şifre hatalı.";
                        return;
                    }

                    Session.UserId = user.Id;
                    Session.Username = user.Username;
                    Session.Role = user.Role;
                }
                catch (Exception ex)
                {
                    _lblError.Text = "Veritabanı hatası: " + ex.Message;
                    return;
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LoginForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "LoginForm";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}

