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
using Core;
using PdfiumViewer;
using System.Drawing.Printing;

namespace UI.ViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        private string _display_date;

        public event PropertyChangedEventHandler PropertyChanged;
        public HomeViewModel HomeViewModel { get; set; }
        public ReportView ReportView { get; set; }
        public RelayCommand ReturnProductDocCommand { get; set; }
        public RelayCommand SaleReportDocCommand { get; set; }
        public RelayCommand DetailedSaleDocCommand { get; set; }

        public string DisplayDate {
            get { return _display_date; }
            set { _display_date = value; onPropertyRaised("DisplayDate"); }
        }

        public ReportViewModel(HomeViewModel home_view_model, ReportView report_view) {
            this.HomeViewModel = home_view_model;
            this.ReportView = report_view;
            home_view_model.Title = "Reports";
            this.ReturnProductDocCommand = new RelayCommand(generateReturnProductsButtonPressed);
            this.SaleReportDocCommand = new RelayCommand(generateSaleReportButtonPressed);
            this.DetailedSaleDocCommand = new RelayCommand(generateDetailedSalesReportButtonPressed);
            CoreApp.logger.log("ReportViewModel successfully initialized.");
        }

        private void generateReturnProductsButtonPressed(object parameter) {
            string from = ReportView.startCalender.SelectedDate?.ToString("MM/dd/yyyy");
            string to = ReportView.endCalender.SelectedDate?.AddDays(1).ToString("MM/dd/yyyy");
            try {
                List<ProductReturnWithNameModel> return_products = SaleAccess.singleton.getReturnProductDetails(from, to);
                double total = 0;
                foreach (ProductReturnWithNameModel model in return_products) {
                    total += model.RefundAmount.value;
                }
                ReturnProductDocument.singleton.export(return_products, ReportView.startCalender.SelectedDate?.ToString("dd/MM/yyyy"), ReportView.endCalender.SelectedDate?.ToString("dd/MM/yyyy"), $"\nTotal Amount: {total.ToString("0.00")}");
                CoreApp.logger.log("Return product models successfully exported as PDF(ReportViewModel)");
                this.HomeViewModel.setNotification("Product Return Report successfully exported!", true);
            }
            catch (Exception ex) {
                CoreApp.logger.log($"Failed to export return product models(ReportViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                this.HomeViewModel.setNotification("Failed to export Product Return Report. Please make sure output directory is not deleted.", false);
            }
        }
        private void generateSaleReportButtonPressed(object parameter) {
            string from = ReportView.startCalender.SelectedDate?.ToString("MM/dd/yyyy");
            string to = ReportView.endCalender.SelectedDate?.AddDays(1).ToString("MM/dd/yyyy");
            try {
                List<SaleDetailModel> sale_models = SaleAccess.singleton.getSaleDetails(from, to);
                double total = 0;
                double discunt = 0;
                double subtotal = 0;
                foreach (SaleDetailModel model in sale_models) {
                    subtotal += model.SubTotal.value;
                    discunt += model.Discount.value;
                    total += model.Total.value;
                }
                string footer = $"\nSub Total : {subtotal.ToString("0.00")}\nDiscount : {discunt.ToString("0.00")}\nTotal : {total.ToString("0.00")}";
                SaleDetailDocument.singleton.export(sale_models, ReportView.startCalender.SelectedDate?.ToString("dd/MM/yyyy"), ReportView.endCalender.SelectedDate?.ToString("dd/MM/yyyy"), footer);
                CoreApp.logger.log("Sales models successfully exported as PDF(ReportViewModel)");
                this.HomeViewModel.setNotification("Sales Report successfully exported!", true);
            }
            catch (Exception ex) {
                CoreApp.logger.log($"Failed to export sales models(ReportViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                this.HomeViewModel.setNotification("Failed to export Sales Report. Please make sure output directory is not deleted.", false);
            }
        }
        private void generateDetailedSalesReportButtonPressed(object parameter) {
            string from = ReportView.startCalender.SelectedDate?.ToString("MM/dd/yyyy");
            string to = ReportView.endCalender.SelectedDate?.AddDays(1).ToString("MM/dd/yyyy");
            try {
                List<SaleDetailModel> sale_models = SaleAccess.singleton.getSaleDetails(from, to);
                DetailedSalesDocument.singleton.export(sale_models, ReportView.startCalender.SelectedDate?.ToString("dd/MM/yyyy"), ReportView.endCalender.SelectedDate?.ToString("dd/MM/yyyy"));
                CoreApp.logger.log("Detailed sales models successfully exported as PDF(ReportViewModel)");
                this.HomeViewModel.setNotification("Detailed Sales Report successfully exported!", true);
            }
            catch (Exception ex)
            {
                CoreApp.logger.log($"Failed to export detailed sales models(ReportViewModel): {ex}", Logger.LogLevel.LEVEL_ERROR);
                this.HomeViewModel.setNotification("Failed to export Detailed Sales Report. Please make sure output directory is not deleted.", false);
            }
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
