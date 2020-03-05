﻿using Core.DB.Access;
using Core.DB.Models;
using System;
using System.ComponentModel;
using System.Threading;
using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class AddUserViewModel : INotifyPropertyChanged
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
        public AddUsers AddUsers { get; set; }

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

        public AddUserViewModel(StaffModel model, AddUsers add_users, HomeViewModel home_view_model) {
            this.HomeViewModel = home_view_model;
            this.AddUsers = add_users;
            if (model != null) {
                this.UpdateOrCreate = "Update";
                this.ID = Convert.ToInt32(model.ID.value);
                this.FirstName = model.FirstName.value;
                this.LastName = model.LastName.value;
                this.UserName = model.UserName.value;
                this.Password = model.Password.value;
                this.EMail = model.EMail.value;
                this.AccessLevel = Convert.ToInt32(model.AccessLevel.value);
                this.CreateOrUpdateCommand = new RelayCommand(updateStaff);
            }
            else {
                this.UpdateOrCreate = "Create";
                this.CreateOrUpdateCommand = new RelayCommand(addStaff);
            }
        }

        public void addStaff(object parameter) {
            StaffModel model = new StaffModel();
            model.FirstName.value = FirstName;
            model.LastName.value = LastName;
            model.UserName.value = UserName;
            model.Password.value = Password;
            model.EMail.value = EMail;

            try {
                StaffAccess.singleton.addStaff(model);
                this.AddUsers.Close();
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Successfully inserted!"));
                thread.Start();
            }
            catch (Exception) {
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Failed to insert!"));
                thread.Start();
            }
        }
        public void updateStaff(object parameter) {
            StaffModel model = new StaffModel();
            model.FirstName.value = FirstName;
            model.LastName.value = LastName;
            model.UserName.value = UserName;
            model.Password.value = Password;
            model.EMail.value = EMail;
            model.AccessLevel.value = AccessLevel;

            try {
                StaffAccess.singleton.updateStaff(model, this.ID);
                this.AddUsers.Close();
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
