using System;
using System.Windows;
using System.Windows.Input;

namespace Barcoded_Warehouse_Stock_Tracking.Views
{
    public partial class MainView : Window
    {
        private DashboardView _dashboardView;

        public MainView()
        {
            InitializeComponent();
            _dashboardView = new DashboardView();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblUserInfo.Text = $"Hoş Geldiniz, {Session.Username} ({Session.Role})";

            if (!Session.IsAdmin)
            {
                btnReports.IsEnabled = false;
                btnReports.Opacity = 0.5;
            }

            // Load default view
            MainContent.Content = _dashboardView;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Session.UserId = 0;
            Session.Username = null;
            Session.Role = null;

            var login = new LoginView();
            if (login.ShowDialog() == true)
            {
                var mainView = new MainView();
                Application.Current.MainWindow = mainView;
                mainView.Show();
                this.Close();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            // If they click dashboard button (need to add event handler in XAML)
            _dashboardView.RefreshAll();
            MainContent.Content = _dashboardView;
        }

        private void BtnPos_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PosView();
        }

        private void BtnCustomers_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CustomersView();
        }

        private void BtnReturns_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ReturnsView();
        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
            if (!Session.IsAdmin) return;
            MainContent.Content = new ReportsView();
        }
    }
}
