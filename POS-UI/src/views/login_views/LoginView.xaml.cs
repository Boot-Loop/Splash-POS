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
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {

        private LoginViewModel _login_view_model;
        public MainView MainView { get; set; }

        public LoginView(MainView main_view)
        {
            InitializeComponent();
            this.MainView = main_view;
            this._login_view_model = new LoginViewModel(main_view, this);
            this.DataContext = _login_view_model;
            password_txt_box.Focus();
        }

        private void eyeButtonMouseDown(object sender, MouseButtonEventArgs e) {
            FontFamily regular_font = FindResource("FONT_ROBOTO_REGULAR") as FontFamily;
            password_txt_box.FontFamily = regular_font; password_txt_box.FontSize = 16;
        }

        private void eyeButtonMouseUp(object sender, MouseButtonEventArgs e) {
            FontFamily password_font = FindResource("PASSWORD_FONT") as FontFamily;
            password_txt_box.FontFamily = password_font; password_txt_box.FontSize = 18;
            password_txt_box.Focus();
        }
    }
}
