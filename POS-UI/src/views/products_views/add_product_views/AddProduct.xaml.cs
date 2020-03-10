using Core.DB.Models;
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
using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for AddUsers.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private AddProductViewModel _add_product_view_model;
        public AddProduct(ProductModel model, HomeViewModel home_view_model)
        {
            InitializeComponent();
            this._add_product_view_model = new AddProductViewModel(model, this, home_view_model);
            this.DataContext = _add_product_view_model;
            this.Owner = Application.Current.MainWindow;
            name_text_box.Focus();
        }

        private void StackPanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FrameworkElement s = e.Source as FrameworkElement;
                if (s != null && s != description_text_box)
                {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }

                if (s == description_text_box)
                {
                    _add_product_view_model.addProduct(null);
                }

                e.Handled = true;
            }
        }
    }
}
