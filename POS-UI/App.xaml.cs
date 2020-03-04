using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UI.Views;

using CoreApp = Core;

namespace POS_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void applicationStartup(object sender, StartupEventArgs e) {
            CoreApp.Application.singleton.initialize();
            MainView main_view = new MainView();
            main_view.Show();
        }
    }
}
