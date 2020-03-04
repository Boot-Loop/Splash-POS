using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class HomeViewModel
    {
        public StaffModel LoggedInUser { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }
        public MainView MainView { get; set; }

        public HomeViewModel(StaffModel user, MainView main_view) {
            this.LoggedInUser = user;
            this.MainView = main_view;
            this.LogoutCommand = new RelayCommand(logout);

        }
        private void logout(object parameter) {
            MainView.Content = new LoginView(MainView);
        }
    }
}
