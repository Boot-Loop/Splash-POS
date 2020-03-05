using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class SettingsViewModel
    {
        public RelayCommand UserButtonCommand { get; private set; }
        public HomeView HomeView { get; set; }

        public SettingsViewModel(HomeView home_view) {
            this.UserButtonCommand = new RelayCommand(userButtonPressed);
            this.HomeView = home_view;
        }

        private void userButtonPressed(object parameter) {
            HomeView.home_content_control.Content = new Users();
        }
    }
}
