using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using CoreApp = Core.Application;

using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class SupplierView : UserControl
    {
        public SupplierViewModel SupplierViewModel { get; set; }

        public SupplierView(HomeViewModel home_view_model) {
            InitializeComponent();
            this.SupplierViewModel = new SupplierViewModel(home_view_model);
            this.DataContext = SupplierViewModel;
            CoreApp.logger.log("Supplier view successfully initialized!");
        }

        private void edit_button_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if (edit_button.IsEnabled) edit_button_image.Source = new BitmapImage(new Uri("/res/icons/edit.png", UriKind.Relative));
            else edit_button_image.Source = new BitmapImage(new Uri("/res/icons/editdisable.png", UriKind.Relative));
        }
        private void delete_button_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if (delete_button.IsEnabled) delete_button_image.Source = new BitmapImage(new Uri("/res/icons/bin.png", UriKind.Relative));
            else delete_button_image.Source = new BitmapImage(new Uri("/res/icons/deletedisable.png", UriKind.Relative));
        }
    }
}
