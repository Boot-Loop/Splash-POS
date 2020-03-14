using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

using Core.DB.Access;
using Core.DB.Models;
using Core.Documents;
using Core.Utils;
using CoreApp = Core.Application;

using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class StaffViewModel : INotifyPropertyChanged
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

        public StaffViewModel(HomeViewModel home_view_model) {
            this.HomeViewModel      = home_view_model;
            this.AddCommand         = new RelayCommand(openAddWindow);
            this.EditCommand        = new RelayCommand(openEditWindow, isSelectedStaffNotNull);
            this.DeleteCommand      = new RelayCommand(deleteRecord, isSelectedStaffNotAdmin);
            this.ExportPDFCommand   = new RelayCommand(exportPDF);
            home_view_model.Title   = "Staffs";
            refresh();
            CoreApp.logger.log("StaffViewModel successfully initialized.");
        }

        public void refresh() {
            try { 
                this.Staffs = new ObservableCollection<StaffModel>(StaffAccess.singleton.getStaffs());
                CoreApp.logger.log("Staffs successfully fetched from database(StaffViewModel)");
            }
            catch (Exception ex) {
                this.Staffs = new ObservableCollection<StaffModel>();
                CoreApp.logger.log($"Staffs cannot be fetched from database(StaffViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                MessageBox.Show("Failed to fetch staff data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openAddWindow(object parameter) {
            AddStaffView new_staff = new AddStaffView(null, HomeViewModel);
            new_staff.ShowDialog();
            refresh();
        }
        private void openEditWindow(object parameter) {
            AddStaffView old_staff = new AddStaffView(SelectedStaff, HomeViewModel);
            old_staff.ShowDialog();
            refresh();
        }
        private void deleteRecord(object parameter) {
            DialogResult result = MessageBox.Show("Are you sure to delete this user?", "Delete Staff", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes) {
                try {
                    StaffAccess.singleton.deleteStaff(Convert.ToInt32(SelectedStaff.ID.value));
                    CoreApp.logger.log("Staff model is successfully deleted(StaffViewModel)");
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage($"Staff {SelectedStaff.FirstName.value} with ID {SelectedStaff.ID.value} successfully deleted.", true));
                    thread.Start();
                }
                catch (Exception ex) {
                    CoreApp.logger.log($"Failed to delete staff model(StaffViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage($"Failed to delete the staff {SelectedStaff.FirstName.value} with ID {SelectedStaff.ID.value}.", false));
                    thread.Start();
                }
                
            }
            refresh();
        }
        private void exportPDF(object parameter) {
            try {
                StaffsDocument.singleton.export(new List<StaffModel>(Staffs));
                CoreApp.logger.log("Staff models successfully exported as PDF(StaffViewModel)");
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Staff details successfully exported!", true));
                thread.Start();
            }
            catch (Exception ex) {
                CoreApp.logger.log($"Failed to export staff models(StaffViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Failed to export staff details. Please make sure output directory is not deleted.", false));
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
