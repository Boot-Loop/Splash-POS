using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class ManagementViewModel
    {
        public RelayCommand ProductButtonCommand { get; private set; }
        public RelayCommand SupplierButtonCommand { get; private set; }
        public RelayCommand StockButtonCommand { get; private set; }
        public RelayCommand StaffButtonCommand { get; private set; }
        public RelayCommand ReportsButtonCommand { get; private set; }
        public RelayCommand SettingsButtonCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }
        public HomeView HomeView { get; set; }
        public HomeViewModel HomeViewModel { get; set; }

        public ManagementViewModel(HomeView home_view, HomeViewModel home_view_model) {
            this.ProductButtonCommand = new RelayCommand(productButtonPressed);
            this.SupplierButtonCommand = new RelayCommand(supplierButtonPressed);
            this.StockButtonCommand = new RelayCommand(stockButtonPressed);
            this.StaffButtonCommand = new RelayCommand(staffButtonPressed);
            this.ReportsButtonCommand = new RelayCommand(reportsButtonPressed);
            this.SettingsButtonCommand = new RelayCommand(settingsButtonPressed);
            this.CloseCommand = new RelayCommand(closeButtonPressed);
            home_view_model.Title = "Settings";
            this.HomeView = home_view;
            this.HomeViewModel = home_view_model;
        }

        private void productButtonPressed(object parameter) {
            HomeView.home_content_control.Content = new ProductView(HomeViewModel);
        }
        private void supplierButtonPressed(object parameter) {
            HomeView.home_content_control.Content = new SupplierView(HomeViewModel);
        }
        private void stockButtonPressed(object parameter) {
            HomeView.home_content_control.Content = new StockView(HomeViewModel);
        }
        private void staffButtonPressed(object parameter) {
            HomeView.home_content_control.Content = new StaffView(HomeViewModel);
        }
        private void reportsButtonPressed(object parameter) {
            HomeView.home_content_control.Content = new ReportView(HomeViewModel);
        }
        private void settingsButtonPressed(object parameter) {
            HomeView.home_content_control.Content = new SettingsView(HomeViewModel);
        }
        private void closeButtonPressed(object parameter) {
            HomeView.home_content_control.Content = HomeViewModel.Sales;
            HomeViewModel.Title = "Sales";
        }
    }
}
