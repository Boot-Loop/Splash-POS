using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using UI.ViewModels;

using CoreApp = Core.Application;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {

        public LoginViewModel LoginViewModel { get; set; }
        public MainView MainView { get; set; }

        public LoginView(MainView main_view) {
            InitializeComponent();
            this.MainView = main_view;
            this.LoginViewModel = new LoginViewModel(main_view, this);
            this.DataContext = LoginViewModel;
            password_txt_box.Focus();
            CoreApp.logger.log("Login view successfully initialized!");
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
