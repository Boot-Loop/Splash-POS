using Core.DB.Access;
using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class NewSaleViewModel : INotifyPropertyChanged
    {
        private string _barcode;
        private string _subtotal;
        private string _discount;
        private string _total;
        private int _sale_id;
        private bool _search_by_name;
        private ObservableCollection<SaleProductModel> _sale_products;
        private ObservableCollection<ProductModel> _search_products;
        public RelayCommand BarcodeAddCommand { get; private set; }
        public RelayCommand DeleteItemCommand { get; private set; }
        public RelayCommand VoidSaleCommand { get; private set; }
        public RelayCommand DoPaymentCommand { get; private set; }
        public SaleProductModel SelectedItem { get; set; }
        public SalesViewModel SalesViewModel { get; set; }
        public HomeViewModel HomeViewModel { get; set; }
        public NewSale NewSale { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Barcode {
            get { return _barcode; }
            set { _barcode = value; onPropertyRaised("Barcode"); searchByName(Barcode); }
        }
        public string SubTotal {
            get { return _subtotal; }
            set { _subtotal = value; onPropertyRaised("SubTotal"); }
        }
        public string Discount {
            get { return _discount; }
            set { _discount = value; onPropertyRaised("Discount"); }
        }
        public string Total {
            get { return _total; }
            set { _total = value; onPropertyRaised("Total"); }
        }
        public int SaleID {
            get { return _sale_id; }
            set { _sale_id = value; onPropertyRaised("SaleID"); }
        }
        public bool SearchByName {
            get { return _search_by_name; }
            set { _search_by_name = value; onPropertyRaised("SearchByName"); }
        }
        public ObservableCollection<SaleProductModel> SaleProducts {
            get { return _sale_products; }
            set { _sale_products = value; onPropertyRaised("SaleProducts"); }
        }
        public ObservableCollection<ProductModel> SearchProducts {
            get { return _search_products; }
            set { _search_products = value; onPropertyRaised("SearchProducts"); }
        }

        public NewSaleViewModel(NewSale new_sale, SalesViewModel sales_view_model, HomeViewModel home_view_model) {
            this.NewSale = new_sale;
            this.SalesViewModel = sales_view_model;
            this.HomeViewModel = home_view_model;
            this.BarcodeAddCommand = new RelayCommand(enterPressedOnBarcodeSearch);
            this.DeleteItemCommand = new RelayCommand(deleteItem, isSelectedItemNotNull);
            this.VoidSaleCommand = new RelayCommand(searchByName); /////////
            this.DoPaymentCommand = new RelayCommand(doPayment);
            this.SaleProducts = new ObservableCollection<SaleProductModel>();
            this.SearchProducts = new ObservableCollection<ProductModel>();
            this.SubTotal = "0.00";
            this.Discount = "0.00";
            this.Total = "0.00";
            this.SaleID = 123;
        }

        private void enterPressedOnBarcodeSearch(object parameter) {
            ProductModel model = ProductAccess.singleton.getProductUsingBarcode(Barcode);
            if (model != null) {
                addProductToList(model);
            } else {
                MessageBox.Show("No product found please check the Barcode!", "No product found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.SubTotal = calculateTotal()[0].ToString("0.00");
            this.Total = calculateTotal()[2].ToString("0.00");
            this.Barcode = null;
        }
        private void addProductToList(ProductModel model) {
            bool found = false;
            ObservableCollection<SaleProductModel> temp_list = new ObservableCollection<SaleProductModel>();
            foreach (SaleProductModel sp_model in SaleProducts) {
                temp_list.Add(sp_model);
            }
            foreach (SaleProductModel sp_modle in temp_list) {
                if (sp_modle.ProductID.value == model.ID.value) {
                    SaleProductModel temp_model = sp_modle;
                    SaleProducts.Remove(sp_modle);
                    temp_model.Qunatity.value += 3;
                    temp_model.SubTotal.value = temp_model.Price.value * temp_model.Qunatity.value;
                    SaleProducts.Add(temp_model);
                    found = true;
                    break;
                }
            }
            if (!found) {
                SaleProductModel sale_product_model = new SaleProductModel();
                sale_product_model.ProductName.value = model.Name.value;
                sale_product_model.ProductID.value = model.ID.value;
                sale_product_model.Qunatity.value = 3;
                sale_product_model.Price.value = model.Price.value;
                sale_product_model.SubTotal.value = model.Price.value * 3;
                SaleProducts.Add(sale_product_model);
            }
        }
        private void voidButtonPressed(object parameter) {
            this.SalesViewModel.removeSale(NewSale);
        }

        private List<double> calculateTotal() {
            double sub_total = 0;
            double discount = 0;
            foreach (SaleProductModel sale_product in SaleProducts) {
                sub_total += sale_product.SubTotal.value;
                discount += sale_product.Discount.value;
            }
            double total = sub_total - discount;
            return new List<double>() { sub_total, discount, total };
        }
        private void deleteItem(object parameter) {
            SaleProducts.Remove(SelectedItem);
            this.SubTotal = calculateTotal()[0].ToString("0.00");
            this.Total = calculateTotal()[2].ToString("0.00");
        }
        private void doPayment(object parameter) {
            if (SaleProducts.Count == 0) { }
            else {
                PaymentModel payment_model = new PaymentModel();
                payment_model.Amount.value = Convert.ToDouble(Total);
                payment_model.TransactionTime.value = DateTime.Now;
                int payment_id = SaleAccess.singleton.addPayment(payment_model);

                SaleModel sale_model = new SaleModel();
                sale_model.UserID.value = this.HomeViewModel.LoggedInUser.ID.value;
                sale_model.PaymentID.value = payment_id;
                int sale_id = SaleAccess.singleton.addSale(sale_model);

                foreach (SaleProductModel sale_product in SaleProducts) {
                    SaleProductModel sale_product_model = new SaleProductModel();
                    sale_product_model = sale_product;
                    sale_product_model.SaleID.value = sale_id;
                    SaleAccess.singleton.addSaleProduct(sale_product_model);
                }
            }
        }
        private void searchByName(object parameter) {
            if (string.IsNullOrEmpty(Barcode)) { SearchProducts = new ObservableCollection<ProductModel>(); }
            else { SearchProducts = new ObservableCollection<ProductModel>(ProductAccess.singleton.searchProducts(Barcode)); }
            Console.WriteLine(SearchProducts.Count);
        }
        private bool isSelectedItemNotNull(object parameter) {
            return SelectedItem == null ? false : true;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
