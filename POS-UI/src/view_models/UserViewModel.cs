using Core.DB.Access;
using Core.DB.Models;
using Core.Documents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<StaffModel> _staffs;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand ExportPDFCommand { get; private set; }
        public StaffModel SelectedStaff { get; set; }
        public HomeViewModel HomeViewModel { get; set; }

        public ObservableCollection<StaffModel> Staffs {
            get { return _staffs; }
            set { _staffs = value; onPropertyRaised("Staffs"); }
        }

        public UserViewModel(HomeViewModel home_view_model) {
            this.HomeViewModel = home_view_model;
            this.AddCommand = new RelayCommand(openAddWindow);
            this.EditCommand = new RelayCommand(openEditWindow, isSelectedStaffNotNull);
            this.DeleteCommand = new RelayCommand(deleteRecord, isSelectedStaffNotAdmin);
            this.ExportPDFCommand = new RelayCommand(exportPDF);
            home_view_model.Title = "Users";
            refresh();
        }

        public void refresh() {
            this.Staffs = new ObservableCollection<StaffModel>(StaffAccess.singleton.getStaffs());
        }

        private void openAddWindow(object parameter) {
            AddUsers new_user = new AddUsers(null, HomeViewModel);
            new_user.ShowDialog();
            refresh();
        }
        private void openEditWindow(object parameter) {
            AddUsers new_user = new AddUsers(SelectedStaff, HomeViewModel);
            new_user.ShowDialog();
            refresh();
        }
        private void deleteRecord(object parameter) {
            DialogResult result = MessageBox.Show("Are you sure to delete this user?", "Delete Staff", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes) {
                try {
                    StaffAccess.singleton.deleteStaff(Convert.ToInt32(SelectedStaff.ID.value));
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage("Successfully deleted!"));
                    thread.Start();
                }
                catch (Exception) {
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage("Failed to delete!"));
                    thread.Start();
                }
                
            }
            refresh();
        }
        private void exportPDF(object parameter) {
            try {
                StaffsDocument.singleton.export(new List<StaffModel>(Staffs));
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Successfully Exported!"));
                thread.Start();
            }
            catch (Exception) {
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Export delete!"));
                thread.Start();
            }
        }
        private bool isSelectedStaffNotNull(object parameter) {
            return (SelectedStaff == null) ? false : true;
        }
        private bool isSelectedStaffNotAdmin(object parameter) {
            return (SelectedStaff == null) || (SelectedStaff.AccessLevel.value == 10) ? false : true;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
