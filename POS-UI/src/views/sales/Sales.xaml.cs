using Core.DB.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using UI.ViewModels;
using SysForms = System.Windows.Forms;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : UserControl
    {
        public SalesViewModel SalesViewModel { get; set; }
        public HomeViewModel HomeViewModel { get; set; }

        public Sales(HomeViewModel home_view_model) {
            InitializeComponent();
            this.HomeViewModel = home_view_model;
            this.SalesViewModel = new SalesViewModel(HomeViewModel, this);
            this.DataContext = SalesViewModel;
        }

        private void salesViewLoaded(object sender, System.Windows.RoutedEventArgs e) {
            if (HomeViewModel.ProductsUpdated) {
                SysForms.DialogResult result = SysForms.MessageBox.Show("Product detailes were updated. To make the changes affective please logout and login. (Changes will be automatically added to the products that are not in these sales.)?", "Products Updated", SysForms.MessageBoxButtons.OK, SysForms.MessageBoxIcon.Warning);
                HomeViewModel.ProductsUpdated = false;
            }
        }
    }
}
