using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for DiscountView.xaml
    /// </summary>
    public partial class DiscountView : Window
    {
        public DiscountView()
        {
            InitializeComponent();
        }

        private void windowDeactivated(object sender, EventArgs e) {
            this.Close();
        }

        private void btnClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string name = button.Name;
            if (name == "btnBS")
            {
                string text_result = inputTextBox.Text;
                if (string.IsNullOrEmpty(inputTextBox.SelectedText))
                {
                    if (text_result.Length > 1)
                    {
                        text_result = text_result.Substring(0, text_result.Length - 1);
                    }
                    else
                    {
                        text_result = "";
                    }
                    inputTextBox.Text = text_result;
                }
                else
                {
                    inputTextBox.Text = "";
                }
            }
            else if (name == "btnEsc")
            {
                this.Close();
            }
            else if (name == "btnEnter")
            {
                //enterPressed(Quantity);
            }
            else if (name == "btnDot")
            {
                if (string.IsNullOrEmpty(inputTextBox.SelectedText)) { if (!inputTextBox.Text.Contains(".")) { inputTextBox.AppendText("."); } }
                else { inputTextBox.Text = "."; }
            }
            else
            {
                if (string.IsNullOrEmpty(inputTextBox.SelectedText)) { inputTextBox.AppendText(Convert.ToString(button.Tag)); }
                else { inputTextBox.Text = Convert.ToString(button.Tag); }
            }
            inputTextBox.CaretIndex = inputTextBox.Text.Length;
        }

    }
}
