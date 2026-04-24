using System;
using System.IO;
using System.Windows;
using Barcoded_Warehouse_Stock_Tracking.Views;

namespace Barcoded_Warehouse_Stock_Tracking
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                var appDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "BarcodedWarehouse");
                Directory.CreateDirectory(appDir);
                AppDomain.CurrentDomain.SetData("DataDirectory", appDir);

                Database.EnsureDatabase();

                // Start with LoginView
                var loginView = new LoginView();
                if (loginView.ShowDialog() == true)
                {
                    var mainView = new MainView();
                    Application.Current.MainWindow = mainView;
                    mainView.Show();
                }
                else
                {
                    Shutdown();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Uygulama başlatılırken bir hata oluştu.\n\n" +
                    ex.Message +
                    "\n\nDetay:\n" + ex,
                    "Başlatma Hatası",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}
