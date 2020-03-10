using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UI.ViewModels;
using UI.ViewModels.Commands;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for QuantityView.xaml
    /// </summary>
    public partial class QuantityView : Window, INotifyPropertyChanged
    {
        private string _quantity;

        private static readonly Regex _regex = new Regex("[^0-9]+");

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand EnterCommand { get; private set; }
        public NewSaleViewModel NewSaleViewModel { get; set; }

        public string Quantity {
            get { return _quantity; }
            set { _quantity = value; onPropertyRaised("Quantity"); }
        }

        public QuantityView(NewSaleViewModel new_sale_view_model) {
            InitializeComponent();
            this.EnterCommand = new RelayCommand(enterPressed);
            this.DataContext = this;
            this.Quantity = "1";
            this.NewSaleViewModel = new_sale_view_model;
            inputTextBox.Focus();
        }

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
            } else {
                e.Handled = !isTextAllowed(e.Text);
            }
        }
        private void previewExecuted(object sender, ExecutedRoutedEventArgs e) {
            if (e.Command == ApplicationCommands.Copy || e.Command == ApplicationCommands.Cut ||  e.Command == ApplicationCommands.Paste) {
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
                    } else {
                        text_result = "";
                    }
                    inputTextBox.Text = text_result;
                } else {
                    inputTextBox.Text = "";
                }
            } else if (name == "btnEsc") {
                this.Close();
            } else if (name == "btnEnter") {
                enterPressed(Quantity);
            } else if (name == "btnDot") {
                if (string.IsNullOrEmpty(inputTextBox.SelectedText)) { if (!inputTextBox.Text.Contains(".")) { inputTextBox.AppendText("."); } }
                else { inputTextBox.Text = "."; }
            } else {
                if (string.IsNullOrEmpty(inputTextBox.SelectedText)) { inputTextBox.AppendText(Convert.ToString(button.Tag)); }
                else { inputTextBox.Text = Convert.ToString(button.Tag); }
            }
            inputTextBox.CaretIndex = inputTextBox.Text.Length;
        }

        private void enterPressed(object parameter) {
            Thread.Sleep(100);
            NewSaleViewModel.Quantity = parameter as string;
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
