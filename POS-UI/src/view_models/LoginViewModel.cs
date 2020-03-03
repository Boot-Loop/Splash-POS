using Core;
using Core.DB.Access;
using Core.DB.Models;
using System;
using System.ComponentModel;
using System.Windows;
using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private string _password;

        public RelayCommand LoginCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public StaffModel LoggedInUser { get; private set; }
        public MainView MainView { get; set; }
        public string Password {
            get { return _password; }
            set { _password = value; onPropertyRaised("Password"); }
        }
        
        public LoginViewModel(MainView main_view) {
            this.MainView = main_view;
            this.LoginCommand = new RelayCommand(login);
        }

        private void login(object parameter) {
            StaffModel model;
            string password = parameter as string;
            try {
                model = StaffAccess.singleton.login(password);
                this.LoggedInUser = model;
                MainView.Content = new HomeView(LoggedInUser);
            }
            catch (WrongPasswordError) {
                MessageBox.Show("Login failed, Check your password and try again!", "Wrong Password", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception) {
                MessageBox.Show("Something went wrong, Please try again!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }

    }
}
