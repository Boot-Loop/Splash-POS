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
    public partial class AddStockView : Window
    {
        private AddStockViewModel AddStockViewModel { get; set; }

        public AddStockView(StockModel model, HomeViewModel home_view_model) {
            InitializeComponent();
            this.AddStockViewModel = new AddStockViewModel(model, this, home_view_model);
            this.DataContext = AddStockViewModel;
            this.Owner = Application.Current.MainWindow;
            CoreApp.logger.log("AddStockView successfully initialized!");
        }

        private void StackPanel_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                FrameworkElement s = e.Source as FrameworkElement;
                if (s != null && s != date_text_box) {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                if (s == date_text_box) {
                    AddStockViewModel.addOrUpdateStock(null);
                }
                e.Handled = true;
            }
        }
    }
}
