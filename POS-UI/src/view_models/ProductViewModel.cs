using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

using Core.DB.Access;
using Core.DB.Models;
using Core.Documents;
using Core.Utils;
using CoreApp = Core.Application;

using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ProductModel> _products;
        private string _search_text;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand ExportPDFCommand { get; private set; }
        public ProductModel SelectedProduct { get; set; }
        public HomeViewModel HomeViewModel { get; set; }

        public ObservableCollection<ProductModel> Products {
            get { return _products; }
            set { _products = value; onPropertyRaised("Products"); }
        }
        public string SearchText {
            get { return _search_text; }
            set { _search_text = value; onPropertyRaised("SearchText"); searchProducts(SearchText); }
        }

        public ProductViewModel(HomeViewModel home_view_model) {
            this.HomeViewModel      = home_view_model;
            this.AddCommand         = new RelayCommand(openAddWindow);
            this.EditCommand        = new RelayCommand(openEditWindow, isSelectedProductNotNull);
            this.DeleteCommand      = new RelayCommand(deleteRecord, isSelectedProductNotNull);
            this.ExportPDFCommand   = new RelayCommand(exportPDF);
            home_view_model.Title   = "Products";
            refresh();
            CoreApp.logger.log("ProductViewModel successfully initialized.");
        }

        private void refresh() {
            try {
                this.Products = new ObservableCollection<ProductModel>(ProductAccess.singleton.getProductsWithBarcodes(""));
                CoreApp.logger.log("Products successfully fetched from database(ProductViewModel)");
            }
            catch (Exception ex) {
                this.Products = new ObservableCollection<ProductModel>();
                CoreApp.logger.log($"Products cannot be fetched from database(ProductViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                MessageBox.Show("Failed to fetch product data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void searchProducts(string search_text) {
            try {
                this.Products = new ObservableCollection<ProductModel>(ProductAccess.singleton.getProductsWithBarcodes(search_text));
                CoreApp.logger.log("Products successfully fetched from database on search(ProductViewModel)");
            }
            catch (Exception ex) {
                this.Products = new ObservableCollection<ProductModel>();
                CoreApp.logger.log($"Products cannot be fetched from database on search(ProductViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
            }
        }
        private void openAddWindow(object parameter) {
            AddProductView new_product = new AddProductView(null, HomeViewModel);
            new_product.ShowDialog();
            refresh();
        }
        private void openEditWindow(object parameter) {
            AddProductView old_product = new AddProductView(SelectedProduct, HomeViewModel);
            old_product.ShowDialog();
            refresh();
        }
        private void deleteRecord(object parameter) {
            DialogResult result = MessageBox.Show("Are you sure to delete this product?", "Delete Product", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes) {
                if (result == DialogResult.Yes) {
                    try {
                        ProductAccess.singleton.deleteProduct(Convert.ToInt32(SelectedProduct.ID.value));
                        CoreApp.logger.log("Product model is successfully deleted(ProductViewModel)");
                        this.HomeViewModel.setNotification($"Product {SelectedProduct.Name.value} with ID {SelectedProduct.ID.value} successfully deleted.", true);
                    }
                    catch (Exception ex) {
                        CoreApp.logger.log($"Failed to delete product model(ProductViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                        this.HomeViewModel.setNotification($"Failed to delete the product {SelectedProduct.Name.value} with ID {SelectedProduct.ID.value}.", false);
                    }
                }
            }
            refresh();
        }
        private void exportPDF(object parameter) {
            try {
                ProductsDocument.singleton.export(new List<ProductModel>(Products));
                CoreApp.logger.log("Product models successfully exported as PDF(ProductViewModel)");
                this.HomeViewModel.setNotification("Product details successfully exported!", true);
            }
            catch (Exception ex) {
                CoreApp.logger.log($"Failed to export product models(ProductViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                this.HomeViewModel.setNotification("Failed to export product details. Please make sure output directory is not deleted.", false);
            }
        }
        private bool isSelectedProductNotNull(object parameter) {
            return SelectedProduct == null ? false : true;
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
