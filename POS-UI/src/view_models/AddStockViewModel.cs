using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

using Core.DB.Access;
using Core.DB.Models;
using Core.Utils;
using Core;
using CoreApp = Core.Application;

using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class AddStockViewModel : INotifyPropertyChanged
    {
        private int _product_id;
        private int _supplier_id;
        private int _quantity;
        private DateTime _date;
        private List<ProductModel> _products;
        private List<SupplierModel> _suppliers;

        private string _update_or_create;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand CreateOrUpdateCommand { get; private set; }
        public HomeViewModel HomeViewModel { get; set; }
        public AddStockView AddStockView { get; set; }

        public int ID { get; set; }
        public int ProductID {
            get { return _product_id; }
            set { _product_id = value; onPropertyRaised("ProductID"); }
        }
        public int SupplierID {
            get { return _supplier_id; }
            set { _supplier_id = value; onPropertyRaised("SupplierID"); }
        }
        public int Quantity {
            get { return _quantity; }
            set { _quantity = value; onPropertyRaised("Quantity"); }
        }
        public DateTime Date {
            get { return _date; }
            set { _date = value; onPropertyRaised("Date"); }
        }
        public List<ProductModel> Products {
            get { return _products; }
            set { _products = value; onPropertyRaised("Products"); }
        }
        public List<SupplierModel> Suppliers {
            get { return _suppliers; }
            set { _suppliers = value; onPropertyRaised("Supppliers"); }
        }
        public ProductModel SelectedProduct { get; set; }
        public SupplierModel SelectedSupplier { get; set; }
        public string UpdateOrCreate {
            get { return _update_or_create; }
            set { _update_or_create = value; onPropertyRaised("UpdateOrCreate"); }
        }

        public AddStockViewModel(StockModel model, AddStockView add_stock_view, HomeViewModel home_view_model) {
            fetchProducts();
            fetchSuppliers();
            this.AddStockView           = add_stock_view;
            this.HomeViewModel          = home_view_model;
            this.CreateOrUpdateCommand  = new RelayCommand(addOrUpdateStock);

            if (model != null) {
                this.UpdateOrCreate = "Update";
                this.ID                 = Convert.ToInt32(model.ID.value);
                this.ProductID          = Convert.ToInt32(model.ProductID.value);
                this.SupplierID         = Convert.ToInt32(model.SupplierID.value);
                foreach (ProductModel product in Products) {
                    if (product.ID.value == ProductID) { this.SelectedProduct = product; break; }
                }
                foreach (SupplierModel supplier in Suppliers) {
                    if (supplier.ID.value == SupplierID) { this.SelectedSupplier = supplier; break; }
                }
                this.Quantity           = Convert.ToInt32(model.Quantity.value);
                this.Date               = model.Date.value;

            }
            else {
                this.UpdateOrCreate = "Create";
            }
            CoreApp.logger.log("AddStockViewModel successfully initialized.");
        }

        public void addOrUpdateStock(object parameter) {
            StockModel model = new StockModel();

            if (this.UpdateOrCreate == "Create") {
                try {
                    validate();

                    model.ProductID.value = SelectedProduct.ID.value;
                    if (SelectedSupplier == null) model.SupplierID.setToNull();
                    else model.SupplierID.value = SelectedSupplier.ID.value;
                    model.Quantity.value = Quantity;
                    model.Date.value = Date;

                    StockAccess.singleton.addStock(model);
                    this.AddStockView.Close();
                    CoreApp.logger.log("Stock model successfully uploaded.(AddStockViewModel)");
                    this.HomeViewModel.setNotification("Stock details added successfully!", true);
                }
                catch (EmptyFieldException) {
                    this.HomeViewModel.setNotification("Required fields cannot be empty.", false);
                }
                catch (Exception ex) {
                    CoreApp.logger.log($"Unexpected error while adding stock details.(AddStockViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                    this.HomeViewModel.setNotification("Some unexpected error occured while adding stock details.", false);
                }
            }
            else {
                try {
                    validate();

                    model.ProductID.value = SelectedProduct.ID.value;
                    if (SelectedSupplier == null) model.SupplierID.setToNull();
                    else model.SupplierID.value = SelectedSupplier.ID.value;
                    model.Quantity.value = Quantity;
                    model.Date.value = Date;

                    StockAccess.singleton.updateStock(model, this.ID);
                    this.AddStockView.Close();
                    CoreApp.logger.log("Stock model successfully updated.(AddStockViewModel)");
                    this.HomeViewModel.setNotification("Stock details updated successfully!", true);
                }
                catch (EmptyFieldException) {
                    this.HomeViewModel.setNotification("Required fields cannot be empty.", false);
                }
                catch (Exception ex) {
                    CoreApp.logger.log($"Unexpected error while updating stock details.(AddStockViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                    this.HomeViewModel.setNotification("Some unexpected error occured while updating stock details.", false);
                }
            }
        }

        private void fetchProducts() {
            try {
                this.Products = ProductAccess.singleton.getProducts("");
                CoreApp.logger.log("Products successfully fetched from database(AddStockViewModel)");
            }
            catch (Exception ex) {
                this.Products = new List<ProductModel>();
                CoreApp.logger.log($"Products cannot be fetched from database(AddStockViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
            }
        }
        private void fetchSuppliers() {
            try {
                this.Suppliers = SupplierAccess.singleton.getSuppliers();
                CoreApp.logger.log("Suppliers successfully fetched from database(AddStockViewModel)");
            }
            catch (Exception ex) {
                this.Suppliers = new List<SupplierModel>();
                CoreApp.logger.log($"Suppliers cannot be fetched from database(AddStockViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
            }
        }
        private void validate() {
            if (SelectedProduct == null) throw new EmptyFieldException("Required fields cannot be empty!");
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
