using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private string _title;

        public event PropertyChangedEventHandler PropertyChanged;
        public StaffModel LoggedInUser { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }
        public RelayCommand SettingsCommand { get; private set; }
        public MainView MainView { get; set; }
        public HomeView HomeView { get; set; }
        public string Title {
            get { return _title; }
            set { _title = value; onPropertyRaised("Title"); }
        }

        public HomeViewModel(StaffModel user, MainView main_view, HomeView home_view) {
            this.LoggedInUser = user;
            this.MainView = main_view;
            this.HomeView = home_view;
            this.LogoutCommand = new RelayCommand(logout);
            this.SettingsCommand = new RelayCommand(openSettings);
        }
        private void logout(object parameter) {
            MainView.Content = new LoginView(MainView);
        }
        private void openSettings(object parameter) {
            HomeView.home_content_control.Content = new Setting(HomeView);
            Title = "(Settings)";
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
