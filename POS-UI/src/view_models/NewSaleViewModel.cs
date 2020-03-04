﻿using Core.DB.Access;
using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UI.ViewModels.Commands;

namespace UI.ViewModels
{
    public class NewSaleViewModel : INotifyPropertyChanged
    {
        private string _barcode;
        private string _subtotal;
        private ObservableCollection<SaleProductModel> _sale_products;
        public RelayCommand BarcodeAddCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Barcode {
            get { return _barcode; }
            set { _barcode = value; onPropertyRaised("Barcode"); }
        }
        public string SubTotal {
            get { return _subtotal; }
            set { _subtotal = value; onPropertyRaised("SubTotal"); }
        }
        public ObservableCollection<SaleProductModel> SaleProducts {
            get { return _sale_products; }
            set { _sale_products = value; onPropertyRaised("SaleProducts"); }
        }

        public NewSaleViewModel() {
            this.BarcodeAddCommand = new RelayCommand(enterPressedOnBarcodeSearch);
            this.SaleProducts = new ObservableCollection<SaleProductModel>();
            this.SubTotal = "0.00";
        }

        private void enterPressedOnBarcodeSearch(object parameter) {
            ProductModel model = ProductAccess.singleton.getProductUsingBarcode(Barcode);
            if (model != null) {
                addProductToList(model);
            } else {
                MessageBox.Show("No product found please check the Barcode!", "No product found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.SubTotal = calculateSubTotal().ToString("0.00");
            Console.WriteLine(SaleProducts.Count);
        }
        private void addProductToList(ProductModel model) {
            bool found = false;
            ObservableCollection<SaleProductModel> temp_list = new ObservableCollection<SaleProductModel>();
            foreach (SaleProductModel sp_model in SaleProducts) {
                temp_list.Add(sp_model);
            }
            foreach (SaleProductModel sp_modle in temp_list) {
                if (sp_modle.ProductID.value == model.ID.value) {
                    SaleProductModel temp_model = sp_modle;
                    SaleProducts.Remove(sp_modle);
                    temp_model.Qunatity.value += 3;
                    temp_model.SubTotal.value = temp_model.Price.value * temp_model.Qunatity.value;
                    SaleProducts.Add(temp_model);
                    found = true;
                    break;
                }
            }
            if (!found) {
                SaleProductModel sale_product_model = new SaleProductModel();
                sale_product_model.ProductName.value = model.Name.value;
                sale_product_model.ProductID.value = model.ID.value;
                sale_product_model.Qunatity.value = 3;
                sale_product_model.Price.value = model.Price.value;
                sale_product_model.SubTotal.value = model.Price.value * 3;
                SaleProducts.Add(sale_product_model);
            }
        }

        private double calculateSubTotal() {
            double sub_total = 0;
            foreach (SaleProductModel sale_product in SaleProducts) {
                sub_total += sale_product.SubTotal.value;
            }
            return sub_total;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
