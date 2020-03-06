using Core.DB.Access;
using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class AddStockViewModel : INotifyPropertyChanged
    {
        private int _product_id;
        private int _supplier_id;
        private int _quantity;
        private double _unit_price;
        private DateTime _date;
        private List<ProductModel> _products;
        private List<SupplierModel> _suppliers;

        private string _update_or_create;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand CreateOrUpdateCommand { get; private set; }
        public HomeViewModel HomeViewModel { get; set; }
        public AddStock AddStock { get; set; }

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
        public double UnitPrice {
            get { return _unit_price; }
            set { _unit_price = value; onPropertyRaised("UnitPrice"); }
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

        public AddStockViewModel(StockModel model, AddStock add_stock, HomeViewModel home_view_model) {
            this.Products = ProductAccess.singleton.getProducts();
            this.AddStock = add_stock;
            this.HomeViewModel = home_view_model;
            
            this.Suppliers = SupplierAccess.singleton.getSuppliers();
            if (model != null) {
                this.UpdateOrCreate = "Update";
                this.ID = Convert.ToInt32(model.ID.value);
                this.ProductID = Convert.ToInt32(model.ProductID.value);
                this.SupplierID = Convert.ToInt32(model.SupplierID.value);
                foreach (ProductModel product in Products) {
                    if (product.ID.value == ProductID) { this.SelectedProduct = product; break; }
                }
                foreach (SupplierModel supplier in Suppliers) {
                    if (supplier.ID.value == SupplierID) { this.SelectedSupplier = supplier; break; }
                }
                this.Quantity = Convert.ToInt32(model.Quantity.value);
                this.UnitPrice = model.UnitPrice.value;
                this.Date = model.Date.value;

                this.CreateOrUpdateCommand = new RelayCommand(updateStock);
            }
            else
            {
                this.UpdateOrCreate = "Create";
                this.CreateOrUpdateCommand = new RelayCommand(addStock);
            }
        }

        public void addStock(object parameter) {
            StockModel model = new StockModel();
            model.ProductID.value = SelectedProduct.ID.value;
            if (SelectedSupplier == null) model.SupplierID.setToNull();
            else model.SupplierID.value = SelectedSupplier.ID.value;
            model.Quantity.value = Quantity;
            model.UnitPrice.value = UnitPrice;
            model.Date.value = Date;

            try {
                StockAccess.singleton.addStock(model);
                this.AddStock.Close();
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Successfully inserted!"));
                thread.Start();
            }
            catch (Exception) {
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Failed to insert!"));
                thread.Start();
            }
        }
        public void updateStock(object parameter) {
            StockModel model = new StockModel();
            model.ProductID.value = SelectedProduct.ID.value;
            if (SelectedSupplier == null) model.SupplierID.setToNull();
            else model.SupplierID.value = SelectedSupplier.ID.value;
            model.Quantity.value = Quantity;
            model.UnitPrice.value = UnitPrice;
            model.Date.value = Date;

            try {
                StockAccess.singleton.updateStock(model, this.ID);
                this.AddStock.Close();
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Successfully updated!"));
                thread.Start();
            }
            catch (Exception) {
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Failed to update!"));
                thread.Start();
            } 
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
