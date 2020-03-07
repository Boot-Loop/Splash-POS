using Core.DB.Access;
using Core.DB.Models;
using System;
using System.ComponentModel;
using System.Threading;
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
        private bool _is_service;

        private string _update_or_create;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand CreateOrUpdateCommand { get; private set; }
        public HomeViewModel HomeViewModel { get; set; }
        public AddProduct AddProduct { get; set; }

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

        public AddProductViewModel(ProductModel model, AddProduct add_product, HomeViewModel home_view_model) {
            this.AddProduct = add_product;
            this.HomeViewModel = home_view_model;
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
                this.CreateOrUpdateCommand = new RelayCommand(updateProduct);
            }
            else {
                this.UpdateOrCreate = "Create";
                this.CreateOrUpdateCommand = new RelayCommand(addProduct);
            }
        }

        public void addProduct(object parameter) {
            ProductModel model = new ProductModel();
            BarcodeModel barcode = new BarcodeModel();
            barcode.Value.value = Barcode;

            model.Name.value = Name;
            model.Code.value = Code;
            model.Barcode.value = Barcode;
            model.Description.value = Description;
            model.Price.value = Price;
            model.IsService.value = IsService;
            model.DateCreated.value = DateTime.Now;
            model.DateUpdated.value = DateTime.Now;

            try {
                ProductAccess.singleton.addProduct(model);
                this.AddProduct.Close();
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Successfully inserted!"));
                thread.Start();
            }
            catch (Exception) {
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Failed to insert!"));
                thread.Start();
            }

            
        }
        public void updateProduct(object parameter) {
            ProductModel model = new ProductModel();
            BarcodeModel barcode = new BarcodeModel();
            barcode.Value.value = Barcode;

            model.Name.value = Name;
            model.Code.value = Code;
            model.Barcode.value = Barcode;
            model.Description.value = Description;
            model.Price.value = Price;
            model.IsService.value = IsService;
            model.DateCreated.value = DateCreated;
            model.DateUpdated.value = DateTime.Now;

            try {
                ProductAccess.singleton.updateProduct(model, this.ID);
                this.AddProduct.Close();
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Successfully updated!"));
                thread.Start();
            }
            catch (Exception) {
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Failed to update!"));
                thread.Start();
            }

            
        }

        private void onPropertyRaised(string property_name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
