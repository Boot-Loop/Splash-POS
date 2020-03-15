using Core.DB.Access;
using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.ViewModels;
using UI.ViewModels.Commands;

using Forms = System.Windows.Forms;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for QuantityView.xaml
    /// </summary>
    public partial class ReturnView : Window, INotifyPropertyChanged
    {
        private string _receipt_no;
        private string _refund_amount;

        private static readonly Regex _regex = new Regex("[^0-9]+");

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand EnterCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }
        public NewSaleViewModel NewSaleViewModel { get; set; }

        public string ReceiptNo {
            get { return _receipt_no; }
            set { _receipt_no = value; onPropertyRaised("ReceiptNo"); }
        }
        public string RefundAmount {
            get { return _refund_amount; }
            set { _refund_amount = value; onPropertyRaised("RefundAmount"); }
        }

        public ReturnView(NewSaleViewModel new_sale_view_model) {
            InitializeComponent();
            this.EnterCommand = new RelayCommand(enterPressed);
            this.CloseCommand = new RelayCommand(closeCommand);
            this.DataContext = this;
            this.NewSaleViewModel = new_sale_view_model;
            recipt_text_box.Focus();
        }

        private void closeCommand(object parameter) { this.Close(); }
        private static bool isTextAllowed(string text) {
            return !_regex.IsMatch(text);
        }
        private void previewTextInput(object sender, TextCompositionEventArgs e) {
            if (e.Text == ".") {
                if (refund_text_box.Text.Contains(".")) { e.Handled = true; }
                else { e.Handled = false; }
            }
            else {
                e.Handled = !isTextAllowed(e.Text);
            }
        }
        private void previewExecuted(object sender, ExecutedRoutedEventArgs e) {
            if (e.Command == ApplicationCommands.Copy || e.Command == ApplicationCommands.Cut || e.Command == ApplicationCommands.Paste) {
                e.Handled = true;
            }
        }
        private void enterPressed(object parameter) {
            Thread.Sleep(100);
            ReciptModel model;
            try { model = SaleAccess.singleton.getReceiptUsingID(ReceiptNo); }
            catch (Exception) { model = null; }
            if (model == null) {
                Forms.DialogResult result = Forms.MessageBox.Show("There is no recipt found. Do you want to still refund?", "Recipt Not Found", Forms.MessageBoxButtons.YesNo, Forms.MessageBoxIcon.Warning);
                if (result == Forms.DialogResult.Yes) {
                    foreach (SaleProductModel sale_product_model in NewSaleViewModel.SaleProducts) {
                        ProductReturnModel return_model = new ProductReturnModel();
                        return_model.ReciptID.value = "no-receipt";
                        return_model.ProductID.value = sale_product_model.ProductID.value;
                        return_model.Qunatity.value = sale_product_model.Qunatity.value;
                        return_model.RefuntAmount.value = Convert.ToDouble(RefundAmount);
                        SaleAccess.singleton.addReturnProduct(return_model);
                    }
                }
            }
            else {
                Forms.DialogResult result = Forms.MessageBox.Show("Are you sure to refund?", "Refund", Forms.MessageBoxButtons.YesNo, Forms.MessageBoxIcon.Information);
                if (result == Forms.DialogResult.Yes) {
                    foreach (SaleProductModel sale_product_model in NewSaleViewModel.SaleProducts) {
                        ProductReturnModel return_model = new ProductReturnModel();
                        return_model.ReciptID.value = model.ID.value;
                        return_model.ProductID.value = sale_product_model.ProductID.value;
                        return_model.Qunatity.value = sale_product_model.Qunatity.value;
                        return_model.RefuntAmount.value = Convert.ToDouble(RefundAmount);
                        SaleAccess.singleton.addReturnProduct(return_model);
                    }
                }
            }
            if (NewSaleViewModel.SalesViewModel.NewSales.Count == 1) {
                NewSaleViewModel.SalesViewModel.SaleDescriptions.RemoveAt(0);
                NewSaleViewModel.SalesViewModel.NewSales.Remove(NewSaleViewModel.NewSale);
                NewSaleViewModel.SalesViewModel.NewSales.Add(new NewSale(NewSaleViewModel.SalesViewModel, NewSaleViewModel.SalesViewModel.HomeViewModel));
                NewSaleViewModel.SalesViewModel.SaleDescriptions.Add("New Sale");
                NewSaleViewModel.SalesViewModel.SelectedIndex = 0;
            }
            else {
                List<NewSale> temp_list = new List<NewSale>();
                foreach (NewSale new_sale in NewSaleViewModel.SalesViewModel.NewSales) {
                    temp_list.Add(new_sale);
                }
                for (int i = 0; i < temp_list.Count; i++) {
                    if (temp_list[i] == NewSaleViewModel.NewSale) {
                        NewSaleViewModel.SalesViewModel.NewSales.Remove(NewSaleViewModel.NewSale);
                        NewSaleViewModel.SalesViewModel.SaleDescriptions.RemoveAt(i);
                        NewSaleViewModel.SalesViewModel.SelectedIndex = NewSaleViewModel.SalesViewModel.NewSales.Count - 1;
                        break;
                    }
                }
            }
            this.Close();
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
