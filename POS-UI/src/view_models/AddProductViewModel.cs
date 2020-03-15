using System;
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
    public class AddProductViewModel : INotifyPropertyChanged
    {
        private string _name;
        private int _code;
        private string _barcode;
        private string _description;
        private double _price;
        private double _cost;
        private bool _is_service;

        private string _update_or_create;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand CreateOrUpdateCommand { get; private set; }
        public HomeViewModel HomeViewModel { get; set; }
        public AddProductView AddProductView { get; set; }

        public int ID { get; set; }
        public string Name {
            get { return _name; }
            set { _name = value; onPropertyRaised("Name"); }
        }
        public int Code {
            get { return _code; }
            set { _code = value; onPropertyRaised("Code"); }
        }
        public string Barcode {
            get { return _barcode; }
            set { _barcode = value; onPropertyRaised("Barcode"); }
        }
        public string Description {
            get { return _description; }
            set { _description = value; onPropertyRaised("Description"); }
        }
        public double Price {
            get { return _price; }
            set { _price = value; onPropertyRaised("Price"); }
        }
        public double Cost {
            get { return _cost; }
            set { _cost = value; onPropertyRaised("Cost"); }
        }
        public bool IsService {
            get { return _is_service; }
            set { _is_service = value; onPropertyRaised("IsService"); }
        }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        
        public string UpdateOrCreate {
            get { return _update_or_create; }
            set { _update_or_create = value; onPropertyRaised("UpdateOrCreate"); }
        }

        public AddProductViewModel(ProductModel model, AddProductView add_product_view, HomeViewModel home_view_model) {
            this.AddProductView = add_product_view;
            this.HomeViewModel = home_view_model;
            this.CreateOrUpdateCommand = new RelayCommand(addOrUpdateProduct);

            if (model != null) {
                this.UpdateOrCreate = "Update";
                this.ID = Convert.ToInt32(model.ID.value);
                this.Name = model.Name.value;
                this.Code = Convert.ToInt32(model.Code.value);
                this.Description = model.Description.value;
                this.Barcode = model.Barcode.value;
                this.Price = model.Price.value;
                this.IsService = model.IsService.value;
                this.DateCreated = model.DateCreated.value;
                this.DateUpdated = model.DateUpdated.value;
            }
            else {
                this.UpdateOrCreate = "Create";
                this.Code = ProductAccess.singleton.getLastProductCode() + 1;
            }
            CoreApp.logger.log("AddProductViewModel successfully initialized.");
        }

        public void addOrUpdateProduct(object parameter) {
            ProductModel model = new ProductModel();
            BarcodeModel barcode = new BarcodeModel();
            barcode.Value.value = Barcode;

            model.Name.value = Name;
            model.ProductGroupID.value = 1;
            model.Code.value = Code;
            model.Barcode.value = Barcode;
            model.Description.value = Description;
            model.Price.value = Price;
            model.Cost.value = Cost;
            model.IsService.value = IsService;
            model.DateCreated.value = DateTime.Now;
            model.DateUpdated.value = DateTime.Now;

            if (this.UpdateOrCreate == "Create") {
                try {
                    validate();

                    ProductAccess.singleton.addProduct(model);
                    HomeViewModel.ProductsUpdated = true;
                    this.AddProductView.Close();
                    CoreApp.logger.log("Product model successfully uploaded.(AddProductViewModel)");
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage("Product details added successfully!", true));
                    thread.Start();
                }
                catch (EmptyFieldException) {
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage("Required fields cannot be empty.", false));
                    thread.Start();
                }
                catch (Exception ex) {
                    CoreApp.logger.log($"Unexpected error while adding product details.(AddProductViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage("Some unexpected error occured while adding product details.", false));
                    thread.Start();
                }
            }
            else {
                try {
                    validate();

                    ProductAccess.singleton.updateProduct(model, this.ID);
                    HomeViewModel.ProductsUpdated = true;
                    this.AddProductView.Close();
                    CoreApp.logger.log("Product model successfully updated.(AddProductViewModel)");
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage("Product details updated successfully!", true));
                    thread.Start();
                }
                catch (EmptyFieldException) {
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage("Required fields cannot be empty.", false));
                    thread.Start();
                }
                catch (Exception ex) {
                    CoreApp.logger.log($"Unexpected error while updating product details.(AddProductViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage("Some unexpected error occured while updating product details.", false));
                    thread.Start();
                }
            } 
        }
        private void validate() {
            if (string.IsNullOrEmpty(Name)) throw new EmptyFieldException("Required fields cannot be empty!");
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
