using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

        public event PropertyChangedEventHandler PropertyChanged;
        public bool CartSelected {
            get { return _cart_selected; }
            set { _cart_selected = value; onPropertyRaised("CartSelected"); }
        }
        public bool PercentSelected {
            get { return _percent_selected; }
            set { _percent_selected = value; onPropertyRaised("PercentSelected"); }
        }


        public DiscountView() {
            InitializeComponent();
            this.CartSelected = true;
            this.PercentSelected = false;
            if (CartSelected) { cart_button.Background = selected_bg; product_button.Background = not_selected_bg; }
            else { cart_button.Background = not_selected_bg; product_button.Background = selected_bg; }
            if (PercentSelected) { percent_button.Background = selected_bg; rs_button.Background = not_selected_bg; }
            else { percent_button.Background = not_selected_bg; rs_button.Background = selected_bg; }
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
                cart_button.Background = selected_bg; product_button.Background = not_selected_bg;
            }
            else if (name == "product_button") {
                cart_button.Background = not_selected_bg; product_button.Background = selected_bg;
            }
            else if (name == "percent_button") {
                percent_button.Background = selected_bg; rs_button.Background = not_selected_bg;
            }
            else if (name == "rs_button") {
                percent_button.Background = not_selected_bg; rs_button.Background = selected_bg;
            }
        }

        private void enterPressed(object parameter) {
            Thread.Sleep(100);
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
