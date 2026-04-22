using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Barcoded_Warehouse_Stock_Tracking
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                var appDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "BarcodedWarehouse");
                Directory.CreateDirectory(appDir);
                AppDomain.CurrentDomain.SetData("DataDirectory", appDir);

                Database.EnsureDatabase();
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Uygulama başlatılırken bir hata oluştu.\n\n" +
                    ex.Message +
                    "\n\nDetay:\n" + ex,
                    "Başlatma Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
