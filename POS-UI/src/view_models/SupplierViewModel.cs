using Core.DB.Access;
using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using a = System.Windows;
using System.Windows.Forms;
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
        public SupplierModel SelectedSupplier { get; set; }

        public ObservableCollection<SupplierModel> Suppliers {
            get { return _suppliers; }
            set { _suppliers = value; onPropertyRaised("Suppliers"); }
        }

        public SupplierViewModel() {
            this.AddCommand = new RelayCommand(openAddWindow);
            this.EditCommand = new RelayCommand(openEditWindow, isSelectedSupplierNotNull);
            this.DeleteCommand = new RelayCommand(deleteRecord, isSelectedSupplierNotNull);
            refresh();
        }

        public void refresh() {
            this.Suppliers = new ObservableCollection<SupplierModel>(SupplierAccess.singleton.getSuppliers());
        }

        private void openAddWindow(object parameter) {
            AddSupplier new_supplier = new AddSupplier(null);
            new_supplier.ShowDialog();
            refresh();
        }
        private void openEditWindow(object parameter) {
            AddSupplier new_supplier = new AddSupplier(SelectedSupplier);
            new_supplier.ShowDialog();
            refresh();
        }
        private void deleteRecord(object parameter) {
            DialogResult result = MessageBox.Show("Are you sure to delete this supplier?", "Delete Supplier", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes) SupplierAccess.singleton.deleteSupplier(Convert.ToInt32(SelectedSupplier.ID.value));
            refresh();
        }
        private bool isSelectedSupplierNotNull(object parameter) {
            return SelectedSupplier == null ? false : true;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
