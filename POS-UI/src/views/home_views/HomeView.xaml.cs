using Core.DB.Models;
using POS_UI.src.views.setting_views;
using System.Windows.Controls;
using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeViewModel HomeViewModel { get; set; }

        public HomeView(StaffModel user, MainView main_view)
        {
            InitializeComponent();
            this.HomeViewModel = new HomeViewModel(user, main_view);
            this.DataContext = HomeViewModel;
        }

        
    }
}
