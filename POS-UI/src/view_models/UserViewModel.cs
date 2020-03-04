using Core.DB.Access;
using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    class UserViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<StaffModel> _staffs;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public StaffModel SelectedStaff { get; set; }

        public ObservableCollection<StaffModel> Staffs {
            get { return _staffs; }
            set { _staffs = value; onPropertyRaised("Staffs"); }
        }

        public UserViewModel() {
            this.AddCommand = new RelayCommand(openAddWindow);
            this.EditCommand = new RelayCommand(openEditWindow, isSelectedStaffNotNull);
            this.DeleteCommand = new RelayCommand(deleteRecord, isSelectedStaffNotNull);
            refresh();
        }

        public void refresh() {
            this.Staffs = new ObservableCollection<StaffModel>(StaffAccess.singleton.getStaffs());
        }

        private void openAddWindow(object parameter) {
            AddUsers new_user = new AddUsers(null);
            new_user.ShowDialog();
            refresh();
        }
        private void openEditWindow(object parameter) {
            AddUsers new_user = new AddUsers(SelectedStaff);
            new_user.ShowDialog();
            refresh();
        }
        private void deleteRecord(object parameter) {
            DialogResult result = MessageBox.Show("Are you sure to delete this user?", "Delete Staff", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes) StaffAccess.singleton.deleteStaff(Convert.ToInt32(SelectedStaff.ID.value));
            refresh();
        }
        private bool isSelectedStaffNotNull(object parameter) {
            return SelectedStaff == null ? false : true;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
