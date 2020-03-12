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
    public partial class PaymentView : Window, INotifyPropertyChanged
    {
        private string _paid;
        private string _total;
        private string _change;

        private static readonly Regex _regex = new Regex("[^0-9]+");

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand EnterCommand { get; private set; }
        public NewSaleViewModel NewSaleViewModel { get; set; }

        public string Paid {
            get { return _paid; }
            set { _paid = value; onPropertyRaised("Paid"); onPropertyRaised("Change"); }
        }
        public string Total {
            get { return _total; }
            set { _total = value; onPropertyRaised("Total"); onPropertyRaised("Change"); }
        }
        public string Change {
            get {
                double paid;
                double total;
                try { paid = Convert.ToDouble(_paid); }
                catch (Exception) { paid = 0; }
                try { total = Convert.ToDouble(_total); }
                catch (Exception) { total = 0; }
                return (paid - total).ToString("0.00");
            }
            set { _change = value; onPropertyRaised("Change"); }
        }

        public PaymentView(NewSaleViewModel new_sale_view_model) {
            InitializeComponent();
            this.EnterCommand = new RelayCommand(enterPressed);
            this.DataContext = this;
            this.NewSaleViewModel = new_sale_view_model;
            inputTextBox.Focus();
        }
        private static bool isTextAllowed(string text) {
            return !_regex.IsMatch(text);
        }
        private void previewTextInput(object sender, TextCompositionEventArgs e) {
            if (e.Text == ".") {
                if (inputTextBox.Text.Contains(".")) { e.Handled = true; }
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
        private void btnClick(object sender, RoutedEventArgs e) {
            Button button = (Button)sender;
            string name = button.Name;
            if (name == "btnBS") {
                string text_result = inputTextBox.Text;
                if (string.IsNullOrEmpty(inputTextBox.SelectedText)) {
                    if (text_result.Length > 1) {
                        text_result = text_result.Substring(0, text_result.Length - 1);
                    }
                    else {
                        text_result = "";
                    }
                    inputTextBox.Text = text_result;
                }
                else {
                    inputTextBox.Text = "";
                }
            }
            else if (name == "btnEsc") {
                this.Close();
            }
            else if (name == "btnEnter") {
                enterPressed(null);
            }
            else if (name == "btnDot") {
                if (string.IsNullOrEmpty(inputTextBox.SelectedText)) { if (!inputTextBox.Text.Contains(".")) { inputTextBox.AppendText("."); } }
                else { inputTextBox.Text = "."; }
            }
            else {
                if (string.IsNullOrEmpty(inputTextBox.SelectedText)) { inputTextBox.AppendText(Convert.ToString(button.Tag)); }
                else { inputTextBox.Text = Convert.ToString(button.Tag); }
            }
            inputTextBox.CaretIndex = inputTextBox.Text.Length;
        }

        private void enterPressed(object parameter) {
            Thread.Sleep(100);
            string paid;
            try {paid = Convert.ToDouble(this.Paid).ToString("0.00"); }
            catch (Exception) { paid = "0.00"; }
            NewSaleViewModel.Paid = paid;
            NewSaleViewModel.Balance = this.Change;
            Forms.DialogResult result = Forms.MessageBox.Show("You want to print Recipt?", "Print Recipt", Forms.MessageBoxButtons.YesNo, Forms.MessageBoxIcon.Information);
            if (result == Forms.DialogResult.Yes) {
                NewSaleViewModel.print();
            }
            if (NewSaleViewModel.SalesViewModel.NewSales.Count == 1)  {
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

        private void windowLoaded(object sender, RoutedEventArgs e) {
            inputTextBox.CaretIndex = inputTextBox.Text.Length;
            inputTextBox.SelectAll();
        }
    }
}
