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
            this.HomeViewModel      = home_view_model;
            this.AddCommand         = new RelayCommand(openAddWindow);
            this.EditCommand        = new RelayCommand(openEditWindow, isSelectedStockNotNull);
            this.DeleteCommand      = new RelayCommand(deleteRecord, isSelectedStockNotNull);
            this.ExportPDFCommand   = new RelayCommand(exportPDF);
            home_view_model.Title   = "Stocks";
            refresh();
            CoreApp.logger.log("StockViewModel successfully initialized.");
        }

        private void refresh() {
            try {
                this.Stocks = new ObservableCollection<StockModel>(StockAccess.singleton.getStocks());
                CoreApp.logger.log("Stocks successfully fetched from database(StockViewModel)");
            }
            catch (Exception ex) {
                this.Stocks = new ObservableCollection<StockModel>();
                CoreApp.logger.log($"Stocks cannot be fetched from database(StockViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                MessageBox.Show("Failed to fetch stock data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void openAddWindow(object parameter) {
            AddStockView new_stock = new AddStockView(null, HomeViewModel);
            new_stock.ShowDialog();
            refresh();
        }
        private void openEditWindow(object parameter) {
            AddStockView old_stock = new AddStockView(SelectedStock, HomeViewModel);
            old_stock.ShowDialog();
            refresh();
        }
        private void deleteRecord(object parameter) {
            DialogResult result = MessageBox.Show("Are you sure to delete this stock?", "Delete Stock", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes) {
                try {
                    StockAccess.singleton.deleteStock(Convert.ToInt32(SelectedStock.ID.value));
                    CoreApp.logger.log("Stock model is successfully deleted(StockViewModel)");
                    this.HomeViewModel.setNotification($"Supplier {SelectedStock.ProductName.value} with ID {SelectedStock.ID.value} successfully deleted.", true);
                }
                catch (Exception ex) {
                    CoreApp.logger.log($"Failed to delete stock model(StockViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                    this.HomeViewModel.setNotification($"Failed to delete the supplier {SelectedStock.ProductName.value} with ID {SelectedStock.ID.value}.", false);
                } 
            }
            refresh();
        }
        private void exportPDF(object parameter) {
            try {
                StocksDocument.singleton.export(new List<StockModel>(Stocks));
                CoreApp.logger.log("Stock models successfully exported as PDF(StockViewModel)");
                this.HomeViewModel.setNotification("Stock details successfully exported!", true);
            }
            catch (Exception ex) {
                CoreApp.logger.log($"Failed to export stock models(StockViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                this.HomeViewModel.setNotification("Failed to export stock details. Please make sure output directory is not deleted.", false);
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
