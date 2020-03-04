using Core.DB.Access;
using Core.DB.Models;
using System;
using System.ComponentModel;
using UI.ViewModels.Commands;

namespace UI.ViewModels
{
    class AddStockViewModel : INotifyPropertyChanged
    {
        private int _product_id;
        private int _supplier_id;
        private int _quantity;
        private DateTime _date;

        private string _update_or_create;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand CreateOrUpdateCommand { get; private set; }

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
        public string UpdateOrCreate {
            get { return _update_or_create; }
            set { _update_or_create = value; onPropertyRaised("UpdateOrCreate"); }
        }

        public AddStockViewModel(StockModel model) {
            if (model != null) {
                this.UpdateOrCreate = "Update";
                this.ID = Convert.ToInt32(model.ID.value);
                this.ProductID = Convert.ToInt32(model.ProductID.value);
                this.SupplierID = Convert.ToInt32(model.SupplierID.value);
                this.Quantity = Convert.ToInt32(model.Quantity.value);
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
            model.ProductID.value = ProductID;
            model.SupplierID.value = SupplierID;
            model.Quantity.value = Quantity;
            model.Date.value = Date;

            StockAccess.singleton.addStock(model);
        }
        public void updateStock(object parameter) {
            StockModel model = new StockModel();
            model.ProductID.value = ProductID;
            model.SupplierID.value = SupplierID;
            model.Quantity.value = Quantity;
            model.Date.value = Date;

            StockAccess.singleton.updateStock(model, this.ID);
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
