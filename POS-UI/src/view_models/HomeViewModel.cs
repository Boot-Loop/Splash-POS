﻿using Core.DB.Models;

using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;

using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private string _title;
        private string _message;
        private string _message_height;

        public event PropertyChangedEventHandler PropertyChanged;
        public StaffModel LoggedInUser { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }
        public RelayCommand SettingsCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }
        public MainView MainView { get; set; }
        public HomeView HomeView { get; set; }
        public Sales Sales { get; set; }
        public bool ProductsUpdated { get; set; }
        public string Title {
            get { return _title; }
            set { _title = value; onPropertyRaised("Title"); }
        }
        public string Message {
            get { return _message; }
            set { _message = value; onPropertyRaised("Message"); }
        }
        public string Height {
            get { return _message_height; }
            set { _message_height = value; onPropertyRaised("Height"); }
        }

        public HomeViewModel(StaffModel user, MainView main_view, HomeView home_view) {
            this.LoggedInUser = user;
            this.MainView = main_view;
            this.HomeView = home_view;
            this.ProductsUpdated = false;
            this.Sales = new Sales(this);
            this.HomeView.home_content_control.Content = Sales;
            this.LogoutCommand = new RelayCommand(logout);
            this.SettingsCommand = new RelayCommand(openSettings);
            this.CloseCommand = new RelayCommand(closeNotification);
            this.Height = "0";
        }

        public void setNotification(string message, bool success) {
            if (success) this.HomeView.notification_view.Background = new SolidColorBrush(Color.FromRgb(2, 115, 32));
            else this.HomeView.notification_view.Background = new SolidColorBrush(Color.FromRgb(166, 3, 3));
            Thread thread = new Thread(() => setMessage(message));
            thread.Start();
        }

        private void setMessage(string message) {
            this.Message = message;
            this.Height = "50";
            Thread.Sleep(5000);
            this.Height = "0";
        }
        private void closeNotification(object parameter) {
            this.Height = "0";
        }
        private void logout(object parameter) {
            bool found = false;
            for (int i = 0; i < this.Sales.SalesViewModel.NewSales.Count; i++) {
                if (this.Sales.SalesViewModel.NewSales[i].NewSaleViewModel.SaleProducts.Count != 0) { found = true; this.Sales.SalesViewModel.SelectedIndex = i; break; }
            }
            if (found) {
                DialogResult result = MessageBox.Show("Are you sure you want to logout.The sales which are not completed will not be saved.", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes) { MainView.Content = new LoginView(MainView); }
            } else {
                MainView.Content = new LoginView(MainView);
            }
        }
        private void openSettings(object parameter) {
            if (this.LoggedInUser.AccessLevel.value == 10) { HomeView.home_content_control.Content = new Management(HomeView, this); }
            else { MessageBox.Show("To access management administrative privilages required.", "Access Limited", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
