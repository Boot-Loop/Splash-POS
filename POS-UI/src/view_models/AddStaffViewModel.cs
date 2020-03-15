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
    public class AddStaffViewModel : INotifyPropertyChanged
    {
        private string _first_name;
        private string _last_name;
        private string _user_name;
        private string _password;
        private string _email;
        private string _update_or_create;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand CreateOrUpdateCommand { get; private set; }
        public HomeViewModel HomeViewModel { get; set; }
        public AddStaffView AddStaffView { get; set; }

        public int ID { get; set; }
        public string FirstName {
            get { return _first_name; }
            set { _first_name = value; onPropertyRaised("FirstName"); }
        }
        public string LastName {
            get { return _last_name; }
            set { _last_name = value; onPropertyRaised("LastName"); }
        }
        public string UserName {
            get { return _user_name; }
            set { _user_name = value; onPropertyRaised("UserName"); }
        }
        public string Password {
            get { return _password; }
            set { _password = value; onPropertyRaised("Password"); }
        }
        public string EMail {
            get { return _email; }
            set { _email = value; onPropertyRaised("EMail"); }
        }
        public string UpdateOrCreate {
            get { return _update_or_create; }
            set { _update_or_create = value; onPropertyRaised("UpdateOrCreate"); }
        }
        public int AccessLevel { get; set; }

        public AddStaffViewModel(StaffModel model, AddStaffView add_staff_view, HomeViewModel home_view_model) {
            this.HomeViewModel              = home_view_model;
            this.AddStaffView               = add_staff_view;
            this.CreateOrUpdateCommand      = new RelayCommand(addOrUpdateStaff);

            if (model != null) {
                this.UpdateOrCreate         = "Update";
                this.ID                     = Convert.ToInt32(model.ID.value);
                this.FirstName              = model.FirstName.value;
                this.LastName               = model.LastName.value;
                this.UserName               = model.UserName.value;
                this.Password               = model.Password.value;
                this.EMail                  = model.EMail.value;
                this.AccessLevel            = Convert.ToInt32(model.AccessLevel.value);
            }
            else {
                this.UpdateOrCreate         = "Create";
            }
            CoreApp.logger.log("AddStaffViewModel successfully initialized.");
        }

        public void addOrUpdateStaff(object parameter) {
            StaffModel model = new StaffModel();
            model.FirstName.value = FirstName;
            model.LastName.value = LastName;
            model.UserName.value = UserName;
            model.Password.value = Password;
            model.EMail.value = EMail;
            if (this.UpdateOrCreate == "Create") {
                try {
                    validate();
                    StaffAccess.singleton.addStaff(model);
                    this.AddStaffView.Close();
                    CoreApp.logger.log("Staff model successfully uploaded.(AddStaffViewModel)");
                    this.HomeViewModel.setNotification("Staff details added successfully!", true);
                }
                catch (EmptyFieldException) {
                    this.HomeViewModel.setNotification("Required fields cannot be empty.", false);
                }
                catch (ValidationError) {
                    this.HomeViewModel.setNotification("Password cannot be less than 4 digits.", false);
                }
                catch (Exception ex) {
                    CoreApp.logger.log($"Unexpected error while adding staff details.(AddStaffViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                    this.HomeViewModel.setNotification("Some unexpected error occured while adding staff details.", false);
                }
            }
            else {
                model.AccessLevel.value = AccessLevel;
                try {
                    validate();
                    StaffAccess.singleton.updateStaff(model, this.ID);
                    this.AddStaffView.Close();
                    CoreApp.logger.log("Staff model successfully updated.(AddStaffViewModel)");
                    this.HomeViewModel.setNotification("Staff details updated successfully!", true);
                }
                catch (EmptyFieldException) {
                    this.HomeViewModel.setNotification("Required fields cannot be empty.", false);
                }
                catch (ValidationError) {
                    this.HomeViewModel.setNotification("Password cannot be less than 4 digits.", false);
                }
                catch (Exception ex) {
                    CoreApp.logger.log($"Unexpected error while updating staff details.(AddStaffViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                    this.HomeViewModel.setNotification("Some unexpected error occured while updating staff details.", false);
                }
            }
        }

        private void validate() {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(UserName)) throw new EmptyFieldException("Required fields cannot be empty!");
            if (string.IsNullOrEmpty(Password) || Password.Length < 4) throw new ValidationError("Password cannot be less than 4 digits!");
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
