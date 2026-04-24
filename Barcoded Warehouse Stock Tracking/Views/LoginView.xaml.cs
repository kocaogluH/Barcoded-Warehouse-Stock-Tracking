using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Barcoded_Warehouse_Stock_Tracking.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            LoadLogo();
        }

        private void LoadLogo()
        {
            try
            {
                string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string[] logoPaths = new[]
                {
                    Path.Combine(exeDir, "Resources", "poseidon_logo.png"),
                    Path.Combine(exeDir, "poseidon_logo.png"),
                    @"C:\Users\Halil Kocaoğlu\.gemini\antigravity\brain\24d4abda-eeb1-454e-b8a7-49b150518e71\poseidon_emblem_clean_1776967179886.png",
                    @"C:\Users\Halil Kocaoğlu\OneDrive\Masaüstü\iş\Poseidon Otomasyon&Yazılım\Logo-2-.png"
                };

                foreach (var path in logoPaths)
                {
                    if (File.Exists(path))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(path, UriKind.Absolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();

                        elLogo.Fill = new ImageBrush(bitmap) { Stretch = Stretch.UniformToFill };
                        break;
                    }
                }
            }
            catch { }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TxtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            lblPassPlaceholder.Visibility = string.IsNullOrEmpty(txtPass.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TxtUser_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            lblUserPlaceholder.Visibility = string.IsNullOrEmpty(txtUser.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var user = txtUser.Text.Trim();
            var pass = txtPass.Password;

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                lblError.Text = "Lütfen tüm alanları doldurun.";
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

                    DialogResult = true;
                    Close();
                }
                else
                {
                    lblError.Text = "Kullanıcı adı veya şifre hatalı!";
                    txtPass.Clear();
                    txtPass.Focus();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Giriş sırasında hata oluştu.";
                MessageBox.Show(ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
