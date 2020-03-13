using Core.DB.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UI.ViewModels;
using UI.ViewModels.Commands;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for DiscountView.xaml
    /// </summary>
    public partial class DiscountView : Window, INotifyPropertyChanged
    {
        private static readonly Regex _regex = new Regex("[^0-9]+");
        private static readonly BrushConverter converter = new BrushConverter();
        private static readonly Brush selected_bg = (Brush)converter.ConvertFromString("#FFDCD6F7");
        private static readonly Brush not_selected_bg = (Brush)converter.ConvertFromString("#FFFFFFFF");

        private bool _cart_selected;
        private bool _percent_selected;
        private string _discount;

        public event PropertyChangedEventHandler PropertyChanged;
        public NewSaleViewModel NewSaleViewModel { get; set; }
        public RelayCommand EnterCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }

        public bool CartSelected {
            get { return _cart_selected; }
            set { _cart_selected = value;
                onPropertyRaised("CartSelected");
                if (CartSelected) { cart_button.Background = selected_bg; product_button.Background = not_selected_bg; }
                else { cart_button.Background = not_selected_bg; product_button.Background = selected_bg; }
            }
        }
        public bool PercentSelected {
            get { return _percent_selected; }
            set { _percent_selected = value;
                onPropertyRaised("PercentSelected");
                if (PercentSelected) { percent_button.Background = selected_bg; rs_button.Background = not_selected_bg; }
                else { percent_button.Background = not_selected_bg; rs_button.Background = selected_bg; }
            }
        }
        public string Discount {
            get { return _discount; }
            set { _discount = value; onPropertyRaised("Discount"); }
        }

        public DiscountView(NewSaleViewModel new_sale_view_model) {
            InitializeComponent();
            this.DataContext = this;
            this.NewSaleViewModel = new_sale_view_model;
            this.EnterCommand = new RelayCommand(enterPressed);
            this.CloseCommand = new RelayCommand(closeCommand);
            this.CartSelected = true;
            this.PercentSelected = false;
            this.Discount = "0";
            inputTextBox.Focus();
        }

        private void closeCommand(object parameter) { this.Close(); }

        private void windowDeactivated(object sender, EventArgs e) {
            try { this.Close(); }
            catch (Exception ex) { Console.WriteLine(ex.Message + ": Exception is correctly handled!"); }

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
                enterPressed(Discount);
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
        private void btnClickD(object sender, RoutedEventArgs e) {
            Button button = (Button)sender;
            string name = button.Name;
            if (name == "cart_button") {
                CartSelected = true;
            }
            else if (name == "product_button") {
                CartSelected = false;
            }
            else if (name == "percent_button") {
                PercentSelected = true;
            }
            else if (name == "rs_button") {
                PercentSelected = false;
            }
        }

        private void enterPressed(object parameter) {
            Thread.Sleep(100);
            if (!_cart_selected) {
                ObservableCollection<SaleProductModel> temp_list = new ObservableCollection<SaleProductModel>();
                double discount;
                try { discount = Convert.ToDouble(Discount); }
                catch (Exception) { discount = 0; }
                foreach (SaleProductModel sp_model in NewSaleViewModel.SaleProducts) {
                    temp_list.Add(sp_model);
                }
                for (int i = 0; i < temp_list.Count; i++) {
                    if (temp_list[i] == NewSaleViewModel.SelectedItem) {
                        SaleProductModel temp_model = temp_list[i];
                        NewSaleViewModel.SaleProducts.Remove(temp_list[i]);
                        if (_percent_selected) { temp_model.Discount.value = -(temp_model.Price.value * temp_model.Qunatity.value) * discount * 0.01; }
                        else { temp_model.Discount.value = -discount; }
                        temp_model.SubTotal.value = (temp_model.Price.value * temp_model.Qunatity.value) + temp_model.Discount.value;
                        NewSaleViewModel.SaleProducts.Insert(i, temp_model);
                        break;
                    }
                }
            } else {
                double discount;
                double sub_total = 0;
                try { discount = Convert.ToDouble(Discount); }
                catch (Exception) { discount = 0; }
                foreach (SaleProductModel sale_product in NewSaleViewModel.SaleProducts) {
                    sub_total += sale_product.Price.value * sale_product.Qunatity.value;
                }
                if (_percent_selected) { NewSaleViewModel.CartDiscount = sub_total * discount * 0.01; }
                else { NewSaleViewModel.CartDiscount = discount; }
               
            }
            NewSaleViewModel.calculateTotal();
            this.Close();
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }

        private void windowLoaded(object sender, RoutedEventArgs e) {
            inputTextBox.CaretIndex = inputTextBox.Text.Length;
            inputTextBox.SelectAll();
        }

        private void windowClosed(object sender, EventArgs e) {
            NewSaleViewModel.HomeViewModel.MainView.bringToFront();
        }
    }
}
