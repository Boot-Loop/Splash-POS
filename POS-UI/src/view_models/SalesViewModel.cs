using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class SalesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<NewSale> _new_sales = new ObservableCollection<NewSale>();
        private ObservableCollection<string> _sale_descriptions = new ObservableCollection<string>();
        private int _selected_index;
        

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand AddNewSaleCommand { get; set; }
        public HomeViewModel HomeViewModel { get; set; }
        public Sales Sales { get; set; }

        public ObservableCollection<NewSale> NewSales  {
            get { return _new_sales; }
            set { _new_sales = value;  onPropertyRaised("NewSales"); }
        } 
        public ObservableCollection<string> SaleDescriptions {
            get { return _sale_descriptions; }
            set { _sale_descriptions = value; }
        }
        public int SelectedIndex {
            get { return _selected_index; }
            set { _selected_index = value; bringSaleToFront(_selected_index); onPropertyRaised("SelectedIndex"); }
        }

        public SalesViewModel(HomeViewModel home_view_model, Sales sales) {
            this.HomeViewModel = home_view_model;
            this.AddNewSaleCommand = new RelayCommand(addNewSale);
            this.Sales = sales;
            home_view_model.Title = "Sales";
            NewSales.Add(new NewSale(this, HomeViewModel));
            SaleDescriptions.Add("New Sale");
            this.SelectedIndex = 0;
        }

        public void removeSale(NewSale selected_sale) {
            if (selected_sale.NewSaleViewModel.SaleProducts.Count != 0) {
                DialogResult result = MessageBox.Show("Are you sure you want to void this order.", "Void Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes) {
                    removeSaleWithoutAlert(selected_sale);
                }
            } else {
                removeSaleWithoutAlert(selected_sale);
            }
        }

        private void removeSaleWithoutAlert(NewSale selected_sale) {
            if (NewSales.Count == 1) {
                SaleDescriptions.RemoveAt(0);
                NewSales.Remove(selected_sale);
                NewSales.Add(new NewSale(this, HomeViewModel));
                SaleDescriptions.Add("New Sale");
                this.SelectedIndex = 0;
            } 
            else {
                List<NewSale> temp_list = new List<NewSale>();
                foreach (NewSale new_sale in NewSales) {
                    temp_list.Add(new_sale);
                }
                for (int i = 0; i < temp_list.Count; i++) {
                    if (temp_list[i] == selected_sale) {
                        NewSales.Remove(selected_sale);
                        SaleDescriptions.RemoveAt(i);
                        this.SelectedIndex = NewSales.Count - 1;
                        break;
                    }
                }
            }
        }

        private void addNewSale(object parameter) {
            NewSales.Add(new NewSale(this, HomeViewModel));
            SaleDescriptions.Add("New Sale");
            this.SelectedIndex = NewSales.Count - 1;
        }
        private void bringSaleToFront(int selected_index) {
            if (selected_index >= 0) {
                Sales.sales_content_control.Content = NewSales[selected_index];
                HomeViewModel.MainView.NewSaleViewModel = NewSales[selected_index].NewSaleViewModel;
            }
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
