using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {

        private SettingsViewModel _settings_view_model;

        public SettingsView(HomeViewModel home_view_model)
        {
            InitializeComponent();
            this._settings_view_model = new SettingsViewModel(home_view_model);
            this.DataContext = _settings_view_model;
        }
    }
}
