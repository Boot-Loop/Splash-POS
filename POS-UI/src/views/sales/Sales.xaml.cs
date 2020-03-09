using System.Windows.Controls;
using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : UserControl
    {
        public SalesViewModel SalesViewModel { get; set; }
        public Sales(HomeViewModel home_view_model)
        {
            InitializeComponent();
            this.SalesViewModel = new SalesViewModel(home_view_model, this);
            this.DataContext = SalesViewModel;
        }
    }
}
