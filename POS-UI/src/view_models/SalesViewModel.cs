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
        private NewSale _new_sale;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand AddNewSaleCommand { get; set; }
        public HomeViewModel HomeViewModel { get; set; }
        public NewSale SelectedSale {
            get { return _new_sale; }
            set { _new_sale = value; bringSaleToFront(_new_sale); onPropertyRaised("SelectedSale"); }
        }
        public Sales Sales { get; set; }

        public ObservableCollection<NewSale> NewSales  {
            get { return _new_sales; }
            set { _new_sales = value;  onPropertyRaised("NewSales"); }
        } 

        public SalesViewModel(HomeViewModel home_view_model, Sales sales) {
            this.HomeViewModel = home_view_model;
            this.AddNewSaleCommand = new RelayCommand(addNewSale);
            this.Sales = sales;
            home_view_model.Title = "Sales";
            NewSales.Add(new NewSale(this));
            this.SelectedSale = NewSales[0];
        }

        public void removeSale(NewSale selected_sale) {
            DialogResult result = MessageBox.Show("Are you sure you want to void this order.", "Void Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes) {
                if (NewSales.Count == 1) {
                    NewSales.Remove(selected_sale);
                    NewSales.Add(new NewSale(this));
                    this.SelectedSale = NewSales[0];
                }
                else {
                    List<NewSale> temp_list = new List<NewSale>();
                    foreach (NewSale new_sale in NewSales) {
                        temp_list.Add(new_sale);
                    }
                    foreach (NewSale temp_new_sale in temp_list) {
                        if (temp_new_sale == selected_sale) {
                            NewSales.Remove(selected_sale);
                            this.SelectedSale = NewSales[NewSales.Count - 1];
                            break;
                        }
                    }
                }
            }
        }

        private void addNewSale(object parameter) {
            NewSales.Add(new NewSale(this));
            this.SelectedSale = NewSales[NewSales.Count - 1];
        }
        private void bringSaleToFront(NewSale selected_sale) {
            foreach (NewSale new_sale in NewSales) {
                if (new_sale == selected_sale) {
                    Sales.sales_content_control.Content = new_sale;
                    break;
                }
            }
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
