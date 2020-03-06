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
    /// Interaction logic for NewSale.xaml
    /// </summary>
    public partial class NewSale : UserControl
    {
        private NewSaleViewModel _new_sale_view_model;
        private string _name;

        public string NameOfSale {
            get { return _name; }
            set { _name = value; }
        }

        public NewSale(SalesViewModel sales_view_model, HomeViewModel home_view_model) {
            InitializeComponent();
            this._new_sale_view_model = new NewSaleViewModel(this, sales_view_model, home_view_model);
            this.DataContext = _new_sale_view_model;
            this.NameOfSale = Convert.ToString(_new_sale_view_model.SaleID);
        }
    }
}
