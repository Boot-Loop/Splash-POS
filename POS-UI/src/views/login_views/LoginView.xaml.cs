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
            Console.WriteLine("Loaded");
            password_txt_box.Focus();
        }

        private void eyeButtonClick(object sender, RoutedEventArgs e)
        {
            var fontstyle1 = FindResource("PASSWORD_FONT") as FontFamily;
            var fontstyle2 = FindResource("FONT_ROBOTO_REGULAR") as FontFamily;

            if (password_txt_box.FontFamily == fontstyle1)
            { password_txt_box.FontFamily = fontstyle2; password_txt_box.FontSize = 16; }

            else { password_txt_box.FontFamily = fontstyle1; password_txt_box.FontSize = 18; }

            password_txt_box.Focus();
        }
    }
}
