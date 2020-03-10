using Core.DB.Access;
using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private HomeViewModel _home_view_model;

        public NewSaleViewModel NewSaleViewModel {
            get { return _new_sale_view_model; }
            set { _new_sale_view_model = value; }
        }
        public HomeViewModel HomeViewModel {
            get { return _home_view_model; }
            set { _home_view_model = value; }
        }
        private string _name;

        public string NameOfSale {
            get { return _name; }
            set { _name = value; }
        }

        public NewSale(SalesViewModel sales_view_model, HomeViewModel home_view_model) {
            InitializeComponent();
            this.HomeViewModel = home_view_model;
            this.NewSaleViewModel = new NewSaleViewModel(this, sales_view_model, HomeViewModel);
            this.DataContext = NewSaleViewModel;
            this.search_by_name_txt_box.Action = NewSaleViewModel.searchProductUsingName;
            this.NameOfSale = Convert.ToString(NewSaleViewModel.SaleID);
            Task.Delay(100).ContinueWith(_ => {
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    NewSaleViewModel.selectSearchType("Barcode");
                }));
            });
        }
    }
}
