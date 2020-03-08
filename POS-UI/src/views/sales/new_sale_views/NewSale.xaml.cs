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
        private string _name;

        public string NameOfSale {
            get { return _name; }
            set { _name = value; }
        }

        public NewSale(SalesViewModel sales_view_model, HomeViewModel home_view_model) {
            InitializeComponent();
            this._new_sale_view_model = new NewSaleViewModel(this, sales_view_model, home_view_model);
            this.DataContext = _new_sale_view_model;
            this.search_by_name_txt_box.Action = enterPressedOnBarcodeSearch;
            this.NameOfSale = Convert.ToString(_new_sale_view_model.SaleID);
            this.search_by_barcode_txt_box.Focus();
            
        }

        private void enterPressedOnBarcodeSearch() {
            ProductModel model;
            try { model = ProductAccess.singleton.getProductUsingCode(_new_sale_view_model.PhraseNumber); }
            catch (Exception) { model = null; }
            if (model != null) {
                _new_sale_view_model.addProductToList(model);
            }
            else {
                MessageBox.Show("Error adding this product. Please try again.", "Cannot add product", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            _new_sale_view_model.SubTotal = _new_sale_view_model.calculateTotal()[0].ToString("0.00");
            _new_sale_view_model.Total = _new_sale_view_model.calculateTotal()[2].ToString("0.00");
        }
    }
}
