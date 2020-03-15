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
    public class AddSupplierViewModel : INotifyPropertyChanged
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
        public HomeViewModel HomeViewModel { get; set; }
        public AddSupplierView AddSupplierView { get; set; }

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

        public AddSupplierViewModel(SupplierModel model, AddSupplierView add_supplier_view, HomeViewModel home_view_model) {
            this.AddSupplierView        = add_supplier_view;
            this.HomeViewModel          = home_view_model;
            this.CreateOrUpdateCommand  = new RelayCommand(addOrUpdateSupplier);

            if (model != null) {
                this.UpdateOrCreate     = "Update";
                this.ID                 = Convert.ToInt32(model.ID.value);
                this.FirstName          = model.FirstName.value;
                this.LastName           = model.LastName.value;
                this.Address            = model.Address.value;
                this.EMail              = model.EMail.value;
                this.Telephone          = model.Telephone.value;
                this.Comments           = model.Comments.value;
            }
            else {
                this.UpdateOrCreate     = "Create";
            }
            CoreApp.logger.log("AddSupplierViewModel successfully initialized.");
        }

        public void addOrUpdateSupplier(object parameter) {
            SupplierModel model     = new SupplierModel();
            model.FirstName.value   = FirstName;
            model.LastName.value    = LastName;
            model.Address.value     = Address;
            model.EMail.value       = EMail;
            model.Telephone.value   = Telephone;
            model.Comments.value    = Comments;

            if (this.UpdateOrCreate == "Create") {
                try {
                    validate();
                    SupplierAccess.singleton.addSupplier(model);
                    this.AddSupplierView.Close();
                    CoreApp.logger.log("Supplier model successfully uploaded.(AddSupplierViewModel)");
                    this.HomeViewModel.setNotification("Supplier details added successfully!", true);
                }
                catch (EmptyFieldException) {
                    this.HomeViewModel.setNotification("Required fields cannot be empty.", false);
                }
                catch (Exception ex) {
                    CoreApp.logger.log($"Unexpected error while adding supplier details.(AddSupplierViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                    this.HomeViewModel.setNotification("Some unexpected error occured while adding supplier details.", false);
                }
            }
            else {
                try {
                    validate();
                    SupplierAccess.singleton.updateSupplier(model, this.ID);
                    this.AddSupplierView.Close();
                    CoreApp.logger.log("Supplier model successfully updated.(AddSupplierViewModel)");
                    this.HomeViewModel.setNotification("Supplier details updated successfully!", true);
                }
                catch (EmptyFieldException) {
                    this.HomeViewModel.setNotification("Required fields cannot be empty.", false);
                }
                catch (Exception ex) {
                    CoreApp.logger.log($"Unexpected error while updating supplier details.(AddSupplierViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                    this.HomeViewModel.setNotification("Some unexpected error occured while updating supplier details.", false);
                }
            }
        }
        private void validate() {
            if (string.IsNullOrEmpty(FirstName)) throw new EmptyFieldException("Required fields cannot be empty!");
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
