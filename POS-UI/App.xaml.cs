using System.Windows;
using UI.Views;

using CoreApp = Core.Application;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void applicationStartup(object sender, StartupEventArgs e) {
            CoreApp.singleton.initialize();
            MainView main_view = new MainView();
            main_view.Show();
        }
    }
}
