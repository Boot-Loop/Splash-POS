using Core.DB.Access;
using Core.DB.Models;
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
        public NewSaleViewModel NewSaleViewModel {
            get { return _new_sale_view_model; }
            set { _new_sale_view_model = value; }
        }
        private string _name;

        public string NameOfSale {
            get { return _name; }
            set { _name = value; }
        }

        public NewSale(SalesViewModel sales_view_model, HomeViewModel home_view_model) {
            InitializeComponent();
            this.NewSaleViewModel = new NewSaleViewModel(this, sales_view_model, home_view_model);
            this.DataContext = _new_sale_view_model;
            this.search_by_name_txt_box.Action = enterPressedOnBarcodeSearch;
            this.NameOfSale = Convert.ToString(NewSaleViewModel.SaleID);
            this.search_by_barcode_txt_box.Focus();
            
        }

        private void enterPressedOnBarcodeSearch() {
            ProductModel model;
            try { model = ProductAccess.singleton.getProductUsingCode(NewSaleViewModel.PhraseNumber); }
            catch (Exception) { model = null; }
            if (model != null) {
                NewSaleViewModel.addProductToList(model);
            }
            else {
                MessageBox.Show("Error adding this product. Please try again.", "Cannot add product", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            NewSaleViewModel.SubTotal = NewSaleViewModel.calculateTotal()[0].ToString("0.00");
            NewSaleViewModel.Total = NewSaleViewModel.calculateTotal()[2].ToString("0.00");
        }
    }
}
