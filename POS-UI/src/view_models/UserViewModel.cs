using Core.DB.Access;
using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    //public class FetchedStaffModel
    //{
    //    public int ID { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string UserName { get; set; }
    //    public string Password { get; set; }
    //    public string EMail { get; set; }
    //    public int AccessLevel { get; set; }

    //    public static List<StaffModel> fetchStaffs()
    //    {
    //        List<StaffModel> fetched_staffs = StaffAccess.singleton.getStaffs();
    //        List<RecentProjectModel> recent_projects = new List<RecentProjectModel>();
    //        foreach (ProgrameData.ProjectViewData recent_project in recent_projects_fetched)
    //        {
    //            string display_name = string.Format("{0} ({1})", recent_project.name, recent_project.client_name);
    //            string path = recent_project.path;
    //            RecentProjectModel recent_project_model = new RecentProjectModel() { DisplayName = display_name, Path = path };
    //            recent_projects.Add(recent_project_model);
    //        }
    //        return recent_projects;
    //    }
    //}

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
            StaffAccess.singleton.deleteStaff(Convert.ToInt32(SelectedStaff.ID.value));
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
