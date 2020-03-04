using Core.DB.Models;
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

        public HomeView(StaffModel user)
        {
            InitializeComponent();
            this.HomeViewModel = new HomeViewModel(user);
            this.Content = new NewSale();
        }
    }
}
