using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

using Core.DB.Access;
using Core.DB.Models;
using Core.Documents;
using Core.Utils;
using CoreApp = Core.Application;

using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class SupplierViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SupplierModel> _suppliers;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand ExportPDFCommand { get; private set; }
        public SupplierModel SelectedSupplier { get; set; }
        public HomeViewModel HomeViewModel { get; set; }

        public ObservableCollection<SupplierModel> Suppliers {
            get { return _suppliers; }
            set { _suppliers = value; onPropertyRaised("Suppliers"); }
        }

        public SupplierViewModel(HomeViewModel home_view_model) {
            this.HomeViewModel      = home_view_model;
            this.AddCommand         = new RelayCommand(openAddWindow);
            this.EditCommand        = new RelayCommand(openEditWindow, isSelectedSupplierNotNull);
            this.DeleteCommand      = new RelayCommand(deleteRecord, isSelectedSupplierNotNull);
            this.ExportPDFCommand   = new RelayCommand(exportPDF);
            home_view_model.Title   = "Suppliers";
            refresh();
            CoreApp.logger.log("SupplierViewModel successfully initialized.");
        }

        public void refresh() {
            try {
                this.Suppliers = new ObservableCollection<SupplierModel>(SupplierAccess.singleton.getSuppliers());
                CoreApp.logger.log("Suppliers successfully fetched from database(SupplierViewModel)");
            }
            catch (Exception ex) {
                this.Suppliers = new ObservableCollection<SupplierModel>();
                CoreApp.logger.log($"Suppliers cannot be fetched from database(SupplierViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                MessageBox.Show("Failed to fetch supplier data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openAddWindow(object parameter) {
            AddSupplierView new_supplier = new AddSupplierView(null, HomeViewModel);
            new_supplier.ShowDialog();
            refresh();
        }
        private void openEditWindow(object parameter) {
            AddSupplierView old_supplier = new AddSupplierView(SelectedSupplier, HomeViewModel);
            old_supplier.ShowDialog();
            refresh();
        }
        private void deleteRecord(object parameter) {
            DialogResult result = MessageBox.Show("Are you sure to delete this supplier?", "Delete Supplier", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes) {
                try {
                    SupplierAccess.singleton.deleteSupplier(Convert.ToInt32(SelectedSupplier.ID.value));
                    CoreApp.logger.log("Supplier model is successfully deleted(SupplierViewModel)");
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage($"Supplier {SelectedSupplier.FirstName.value} with ID {SelectedSupplier.ID.value} successfully deleted.", true));
                    thread.Start();
                }
                catch (Exception ex) {
                    CoreApp.logger.log($"Failed to delete supplier model(StaffViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage($"Failed to delete the supplier {SelectedSupplier.FirstName.value} with ID {SelectedSupplier.ID.value}.", false));
                    thread.Start();
                }
            }
            refresh();
        }
        private void exportPDF(object parameter) {
            try {
                SuppliersDocument.singleton.export(new List<SupplierModel>(Suppliers));
                CoreApp.logger.log("Supplier models successfully exported as PDF(SupplierViewModel)");
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Supplier details successfully exported!", true));
                thread.Start();
            }
            catch (Exception ex) {
                CoreApp.logger.log($"Failed to export supplier models(SupplierViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Failed to export supplier details. Please make sure output directory is not deleted.", false));
                thread.Start();
            }
        }
        private bool isSelectedSupplierNotNull(object parameter) {
            return SelectedSupplier == null ? false : true;
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
