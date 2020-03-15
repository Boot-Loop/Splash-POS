using Core.DB.Models;
using System.Windows.Controls;
using System.Windows.Media;
using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeViewModel HomeViewModel { get; set; }

        public HomeView(StaffModel user, MainView main_view) {
            InitializeComponent();
            this.HomeViewModel = new HomeViewModel(user, main_view, this);
            this.DataContext = HomeViewModel;
        }
    }
}
