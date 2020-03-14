using System;
using System.ComponentModel;
using System.Windows;

using Core;
using Core.DB.Access;
using Core.DB.Models;
using CoreApp = Core.Application;

using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _password;

        public RelayCommand LoginCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public StaffModel LoggedInUser { get; private set; }
        public MainView MainView { get; set; }
        public LoginView LoginView { get; set; }
        public string Password {
            get { return _password; }
            set { _password = value; onPropertyRaised("Password"); }
        }
        
        public LoginViewModel(MainView main_view, LoginView login_view) {
            this.MainView = main_view;
            this.LoginView = login_view;
            this.Password = "";
            this.LoginCommand = new RelayCommand(login);
            CoreApp.logger.log("LoginViewModel successfully initialized.");
        }

        private void login(object parameter) {
            StaffModel model;
            string password = parameter as string;
            try {
                validate();
                model = StaffAccess.singleton.login(password);
                this.LoggedInUser = model;
                MainView.Content = new HomeView(LoggedInUser, MainView);
                CoreApp.logger.log("Successfully logged in.");
            }
            catch (WrongPasswordError) {
                MessageBox.Show("Login failed, Check your password and try again!", "Wrong Password", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (ValidationError) {
                MessageBox.Show("Password cannot be less than 4 digits!", "Wrong Password", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex) {
                CoreApp.logger.log($"Error occured while login: {ex}");
                MessageBox.Show("Something went wrong, Please try again!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoginView.password_txt_box.Focus();
        }
        private void validate() {
            if (string.IsNullOrEmpty(Password) || Password.Length < 4) throw new ValidationError("Password cannot be less than 4 digits!");
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
