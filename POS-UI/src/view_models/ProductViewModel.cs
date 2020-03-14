using Core.DB.Access;
using Core.DB.Models;
using Core.Documents;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ProductModel> _products;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand ExportPDFCommand { get; private set; }
        public ProductModel SelectedProduct { get; set; }
        public HomeViewModel HomeViewModel { get; set; }

        public ObservableCollection<ProductModel> Products
        {
            get { return _products; }
            set { _products = value; onPropertyRaised("Products"); }
        }

        public ProductViewModel(HomeViewModel home_view_model) {
            this.HomeViewModel = home_view_model;
            this.AddCommand = new RelayCommand(openAddWindow);
            this.EditCommand = new RelayCommand(openEditWindow, isSelectedProductNotNull);
            this.DeleteCommand = new RelayCommand(deleteRecord, isSelectedProductNotNull);
            this.ExportPDFCommand = new RelayCommand(exportPDF);
            home_view_model.Title = "Products";
            refresh();
        }

        public void refresh() {
            this.Products = new ObservableCollection<ProductModel>(ProductAccess.singleton.getProductsWithBarcodes());
        }

        private void openAddWindow(object parameter) {
            AddProduct new_product = new AddProduct(null, HomeViewModel);
            new_product.ShowDialog();
            refresh();
        }
        private void openEditWindow(object parameter) {
            AddProduct new_product = new AddProduct(SelectedProduct, HomeViewModel);
            new_product.ShowDialog();
            refresh();
        }
        private void deleteRecord(object parameter) {
            DialogResult result = MessageBox.Show("Are you sure to delete this product?", "Delete Product", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes) {
                if (result == DialogResult.Yes) {
                    try {
                        ProductAccess.singleton.deleteProduct(Convert.ToInt32(SelectedProduct.ID.value));
                        Thread thread = new Thread(() => this.HomeViewModel.setMessage("Successfully deleted!", true));
                        thread.Start();
                    }
                    catch (Exception) {
                        Thread thread = new Thread(() => this.HomeViewModel.setMessage("Failed to delete!", false));
                        thread.Start();
                    }

                }
            }
            refresh();
        }
        private void exportPDF(object parameter) {
            try {
                ProductsDocument.singleton.export(new List<ProductModel>(Products));
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Successfully Exported!", true));
                thread.Start();
            }
            catch (Exception) {
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Export delete!", false));
                thread.Start();
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
