using System.Windows.Controls;
using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : UserControl
    {
        private SalesViewModel _sales_view_model;
        public Sales(HomeViewModel home_view_model)
        {
            InitializeComponent();
            this._sales_view_model = new SalesViewModel(home_view_model, this);
            this.DataContext = _sales_view_model;
        }
    }
}
