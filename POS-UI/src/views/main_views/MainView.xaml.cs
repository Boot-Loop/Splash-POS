using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

using UI.ViewModels;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public NewSaleViewModel NewSaleViewModel { get; set; }
        public MainView()
        {
            InitializeComponent();
            this.Content = new LoginView(this);
        }

        private void windowActivated(object sender, EventArgs e) {
            if (NewSaleViewModel != null)
            {
                if (NewSaleViewModel.SearchByBarcode) {
                    NewSaleViewModel.selectSearchType("Barcode");
                }
                else if (NewSaleViewModel.SearchByName) {
                    NewSaleViewModel.selectSearchType("Name");
                }
                else if (NewSaleViewModel.SearchByCode) {
                    NewSaleViewModel.selectSearchType("Code");
                }
            }
        }

        public void bringToFront() {
            //this.Topmost = true;
            //this.Activate();
            //this.Show();
        }
    }
}
