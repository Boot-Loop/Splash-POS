﻿using System.Windows;
using System.Windows.Input;

using Core.DB.Models;
using CoreApp = Core.Application;

using UI.ViewModels;
using System.Windows.Controls;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for AddUsers.xaml
    /// </summary>
    public partial class AddSupplierView : Window
    {
        public AddSupplierViewModel AddSupplierViewModel { get; set; }

        public AddSupplierView(SupplierModel model, HomeViewModel home_view_model) {
            InitializeComponent();
            this.AddSupplierViewModel = new AddSupplierViewModel(model, this, home_view_model);
            this.DataContext = AddSupplierViewModel;
            this.Owner = Application.Current.MainWindow;
            first_name_text_box.Focus();
            CoreApp.logger.log("AddSupplierView successfully initialized!");
        }

        private void StackPanel_PreviewKeyDown(object sender, KeyEventArgs e) {
                FrameworkElement s = e.Source as FrameworkElement;
            if (e.Key == Key.Enter) {
                if (s != null && s != comment_text_box) {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                if (s == comment_text_box) {
                    AddSupplierViewModel.addOrUpdateSupplier(null);
                }
                e.Handled = true;
            }

            else if (e.Key == Key.Down)
            {
                if (s != null)
                {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                e.Handled = true;
            }

            else if (e.Key == Key.Up)
            {
                if (s != null)
                {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                }
                e.Handled = true;
            }
        }
        private void textBoxGotFocus(object sender, RoutedEventArgs e) {
            TextBox text_box = (TextBox)sender;
            text_box.SelectAll();
        }
    }
}
