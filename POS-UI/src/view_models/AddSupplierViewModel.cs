using Core.DB.Access;
using Core.DB.Models;
using System;
using System.ComponentModel;
using UI.ViewModels.Commands;

namespace UI.ViewModels
{
    class AddSupplierViewModel : INotifyPropertyChanged
    {
        private string _first_name;
        private string _last_name;
        private string _address;
        private string _email;
        private string _telephone;
        private string _comments;

        private string _update_or_create;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand CreateOrUpdateCommand { get; private set; }

        public int ID { get; set; }
        public string FirstName {
            get { return _first_name; }
            set { _first_name = value; onPropertyRaised("FirstName"); }
        }
        public string LastName {
            get { return _last_name; }
            set { _last_name = value; onPropertyRaised("LastName"); }
        }
        public string Address {
            get { return _address; }
            set { _address = value; onPropertyRaised("Address"); }
        }
        public string EMail {
            get { return _email; }
            set { _email = value; onPropertyRaised("EMail"); }
        }
        public string Telephone {
            get { return _telephone; }
            set { _telephone = value; onPropertyRaised("Telephone"); }
        }
        public string Comments {
            get { return _comments; }
            set { _comments = value; onPropertyRaised("Comments"); }
        }
        public string UpdateOrCreate {
            get { return _update_or_create; }
            set { _update_or_create = value; onPropertyRaised("UpdateOrCreate"); }
        }

        public AddSupplierViewModel(SupplierModel model) {
            if (model != null) {
                this.UpdateOrCreate = "Update";
                this.ID = Convert.ToInt32(model.ID.value);
                this.FirstName = model.FirstName.value;
                this.LastName = model.LastName.value;
                this.Address = model.Address.value;
                this.EMail = model.EMail.value;
                this.Telephone = model.Telephone.value;
                this.Comments = model.Comments.value;
                this.CreateOrUpdateCommand = new RelayCommand(updateSupplier);
            }
            else
            {
                this.UpdateOrCreate = "Create";
                this.CreateOrUpdateCommand = new RelayCommand(addSupplier);
            }
        }

        public void addSupplier(object parameter) {
            SupplierModel model = new SupplierModel();
            model.FirstName.value = FirstName;
            model.LastName.value = LastName;
            model.Address.value = Address;
            model.EMail.value = EMail;
            model.Telephone.value = Telephone;
            model.Comments.value = Comments;

            SupplierAccess.singleton.addSupplier(model);
        }
        public void updateSupplier(object parameter) {
            SupplierModel model = new SupplierModel();
            model.FirstName.value = FirstName;
            model.LastName.value = LastName;
            model.Address.value = Address;
            model.EMail.value = EMail;
            model.Telephone.value = Telephone;
            model.Comments.value = Comments;

            SupplierAccess.singleton.updateSupplier(model, this.ID);
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
