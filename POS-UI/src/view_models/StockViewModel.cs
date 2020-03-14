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
    public class StockViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<StockModel> _stocks;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand ExportPDFCommand { get; private set; }
        public StockModel SelectedStock { get; set; }
        public HomeViewModel HomeViewModel { get; set; }

        public ObservableCollection<StockModel> Stocks {
            get { return _stocks; }
            set { _stocks = value; onPropertyRaised("Stocks"); }
        }

        public StockViewModel(HomeViewModel home_view_model) {
            this.HomeViewModel = home_view_model;
            this.AddCommand = new RelayCommand(openAddWindow);
            this.EditCommand = new RelayCommand(openEditWindow, isSelectedStockNotNull);
            this.DeleteCommand = new RelayCommand(deleteRecord, isSelectedStockNotNull);
            this.ExportPDFCommand = new RelayCommand(exportPDF);
            home_view_model.Title = "Stocks";
            refresh();
        }

        public void refresh() {
            this.Stocks = new ObservableCollection<StockModel>(StockAccess.singleton.getStocks());
        }

        private void openAddWindow(object parameter) {
            AddStock new_stock = new AddStock(null, HomeViewModel);
            new_stock.ShowDialog();
            refresh();
        }
        private void openEditWindow(object parameter) {
            AddStock new_stock = new AddStock(SelectedStock, HomeViewModel);
            new_stock.ShowDialog();
            refresh();
        }
        private void deleteRecord(object parameter) {
            DialogResult result = MessageBox.Show("Are you sure to delete this stock?", "Delete Stock", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes) {
                try {
                    StockAccess.singleton.deleteStock(Convert.ToInt32(SelectedStock.ID.value));
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage("Successfully deleted!", true));
                    thread.Start();
                }
                catch (Exception) {
                    Thread thread = new Thread(() => this.HomeViewModel.setMessage("Failed to delete!", false));
                    thread.Start();
                } 
            }
            refresh();
        }
        private void exportPDF(object parameter) {
            try {
                StocksDocument.singleton.export(new List<StockModel>(Stocks));
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Successfully Exported!", true));
                thread.Start();
            }
            catch (Exception)
            {
                Thread thread = new Thread(() => this.HomeViewModel.setMessage("Export delete!", false));
                thread.Start();
            }
        }
        private bool isSelectedStockNotNull(object parameter) {
            return SelectedStock == null ? false : true;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
