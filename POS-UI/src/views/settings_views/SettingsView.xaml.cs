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
            One.Background = Brushes.MediumPurple;
        }

        private void SettingButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (Convert.ToInt16(button.Tag))
            {
                case 0:
                    setting_tab_control.SelectedIndex = Convert.ToInt16(button.Tag);
                    ButtonSelectedEffect();
                    One.Background = Brushes.MediumPurple;
                    break;
                case 1:
                    setting_tab_control.SelectedIndex = Convert.ToInt16(button.Tag);
                    ButtonSelectedEffect();
                    Two.Background = Brushes.MediumPurple;
                    break;
                case 2:
                    setting_tab_control.SelectedIndex = Convert.ToInt16(button.Tag);
                    ButtonSelectedEffect();
                    Three.Background = Brushes.MediumPurple;
                    break;
                case 3:
                    setting_tab_control.SelectedIndex = Convert.ToInt16(button.Tag);
                    ButtonSelectedEffect();
                    Four.Background = Brushes.MediumPurple;
                    break;
                case 4:
                    setting_tab_control.SelectedIndex = Convert.ToInt16(button.Tag);
                    ButtonSelectedEffect();
                    Five.Background = Brushes.MediumPurple;
                    break;
            }
        }


        private void ButtonSelectedEffect()
        {
            One.Background   = Brushes.Transparent;
            Two.Background   = Brushes.Transparent;
            Three.Background = Brushes.Transparent;
            Four.Background  = Brushes.Transparent;
            Five.Background  = Brushes.Transparent;
        }

    }
}
