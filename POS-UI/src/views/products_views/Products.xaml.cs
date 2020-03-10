using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Products : UserControl
    {
        private ProductViewModel _product_view_model;
        public Products(HomeViewModel home_view_model) {
            InitializeComponent();
            this._product_view_model = new ProductViewModel(home_view_model);
            this.DataContext = _product_view_model;
        }



        private void edit_button_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (edit_button.IsEnabled)
                edit_button_image.Source = new BitmapImage(new Uri("/res/icons/edit.png", UriKind.Relative));
            else edit_button_image.Source = new BitmapImage(new Uri("/res/icons/editdisable.png", UriKind.Relative));
            
        }

        private void delete_button_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (delete_button.IsEnabled)
                delete_button_image.Source = new BitmapImage(new Uri("/res/icons/bin.png", UriKind.Relative));
            else delete_button_image.Source = new BitmapImage(new Uri("/res/icons/deletedisable.png", UriKind.Relative));
        }
    }
}
