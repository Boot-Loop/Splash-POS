using System.Windows;
using System.Windows.Input;

using Core.DB.Models;
using CoreApp = Core.Application;

using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for AddUsers.xaml
    /// </summary>
    public partial class AddProductView : Window
    {
        public AddProductViewModel AddProductViewModel { get; set; }

        public AddProductView(ProductModel model, HomeViewModel home_view_model) {
            InitializeComponent();
            this.AddProductViewModel = new AddProductViewModel(model, this, home_view_model);
            this.DataContext = AddProductViewModel;
            this.Owner = Application.Current.MainWindow;
            name_text_box.Focus();
            CoreApp.logger.log("AddProductView successfully initialized!");
        }

        private void StackPanel_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                FrameworkElement s = e.Source as FrameworkElement;
                if (s != null && s != description_text_box) {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                if (s == description_text_box) {
                    AddProductViewModel.addOrUpdateProduct(null);
                }
                e.Handled = true;
            }
        }
    }
}
