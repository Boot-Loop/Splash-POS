﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

using Core.DB.Access;
using Core.DB.Models;
using CoreApp = Core.Application;

using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class NewSaleViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SaleProductModel> _sale_products;
        private ObservableCollection<ProductModel> _search_products;
        private SearchDataProvider _search_data_provider;
        private int _phraseNumber = 0;
        private string _barcode_or_code;
        private string _subtotal;
        private string _discount;
        private string _total;
        private string _paid;
        private string _balance;
        private string _quantity;
        private string _recipt_id;
        private double _cart_discount;
        private int _sale_id;
        private bool _search_by_barcode;
        private bool _search_by_code;
        private bool _search_by_name;
        private bool _is_search_by_name_visible;
        private bool _is_search_by_barcode_visible;

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand BarcodeSearchCommand { get; private set; }
        public RelayCommand DeleteItemCommand { get; private set; }
        public RelayCommand VoidSaleCommand { get; private set; }
        public RelayCommand SearchSelectionCommand { get; private set; }
        public RelayCommand DiscountCommand { get; set; }
        public RelayCommand QuantityCommand { get; set; }
        public RelayCommand PayCommand { get; set; }
        public RelayCommand ReturnCommand { get; set; }
        public SaleProductModel SelectedItem { get; set; }
        public ProductModel SearchedModel { get; set; } = null;
        public SalesViewModel SalesViewModel { get; set; }
        public HomeViewModel HomeViewModel { get; set; }
        public NewSale NewSale { get; set; }

        public int PhraseNumber {
            get { return _phraseNumber; }
            set { _phraseNumber = value; onPropertyRaised("PhraseNumber"); }
        }
        public string BarcodeOrCode {
            get { return _barcode_or_code; }
            set { _barcode_or_code = value; onPropertyRaised("BarcodeOrCode"); }
        }
        public string SubTotal {
            get { return _subtotal; }
            set { _subtotal = value; onPropertyRaised("SubTotal"); }
        }
        public string Discount {
            get { return _discount; }
            set { _discount = value; onPropertyRaised("Discount"); }
        }
        public string Total {
            get { return _total; }
            set { _total = value; onPropertyRaised("Total"); }
        }
        public string Paid {
            get { return _paid; }
            set { _paid = value; onPropertyRaised("Paid"); }
        }
        public string Balance {
            get { return _balance; }
            set { _balance = value; onPropertyRaised("Balance"); }
        }
        public string Quantity {
            get { return _quantity; }
            set { _quantity = value; onPropertyRaised("Quantity"); addProductToList(SearchedModel, Quantity); }
        }
        public string ReciptID {
            get { return _recipt_id; }
            set { _recipt_id = value; onPropertyRaised("ReciptID"); }
        }
        public double CartDiscount {
            get { return _cart_discount; }
            set { _cart_discount = value; onPropertyRaised("CartDiscount"); }
        }
        public int SaleID {
            get { return _sale_id; }
            set { _sale_id = value; onPropertyRaised("SaleID"); }
        }
        public bool SearchByBarcode {
            get { return _search_by_barcode; }
            set { _search_by_barcode = value; onPropertyRaised("SearchByBarcode"); }
        }
        public bool SearchByCode {
            get { return _search_by_code; }
            set { _search_by_code = value; onPropertyRaised("SearchByCode"); }
        }
        public bool SearchByName {
            get { return _search_by_name; }
            set { _search_by_name = value; onPropertyRaised("SearchByName"); }
        }
        public bool IsSearchByNameVisible {
            get { return _is_search_by_name_visible; }
            set { _is_search_by_name_visible = value; onPropertyRaised("IsSearchByNameVisible"); }
        }
        public bool IsSearchByBarcodeVisible {
            get { return _is_search_by_barcode_visible; }
            set { _is_search_by_barcode_visible = value; onPropertyRaised("IsSearchByBarcodeVisible"); }
        }
        public ObservableCollection<SaleProductModel> SaleProducts {
            get { return _sale_products; }
            set { _sale_products = value; onPropertyRaised("SaleProducts"); }
        }
        public ObservableCollection<ProductModel> SearchProducts {
            get { return _search_products; }
            set { _search_products = value; onPropertyRaised("SearchProducts"); }
        }
        public SearchDataProvider SearchDataProvider {
            get { return _search_data_provider; }
            set { _search_data_provider = value; onPropertyRaised("SearchDataProvider"); } 
        }

        public NewSaleViewModel(NewSale new_sale, SalesViewModel sales_view_model, HomeViewModel home_view_model) {
            this.NewSale                    = new_sale;
            this.SalesViewModel             = sales_view_model;
            this.HomeViewModel              = home_view_model;
            this.BarcodeSearchCommand       = new RelayCommand(searchProductUsingBarcode);
            this.DeleteItemCommand          = new RelayCommand(deleteItem, isSelectedItemNotNull);
            this.VoidSaleCommand            = new RelayCommand(voidButtonPressed);
            this.SearchSelectionCommand     = new RelayCommand(selectSearchType);
            this.DiscountCommand            = new RelayCommand(discountButtonPressed);
            this.QuantityCommand            = new RelayCommand(quantityButtonPressed, isSelectedItemNotNull);
            this.PayCommand                 = new RelayCommand(payButtonPressed);
            this.ReturnCommand              = new RelayCommand(returnButtonPressed);
            this.SaleProducts               = new ObservableCollection<SaleProductModel>();
            this.SearchProducts             = new ObservableCollection<ProductModel>();
            this.SearchDataProvider         = new SearchDataProvider();
            this.SubTotal                   = "0.00";
            this.Discount                   = "0.00";
            this.Total                      = "0.00";
            this.SearchByBarcode            = true;
            this.SearchByCode               = false;
            this.SearchByName               = false;
            this.IsSearchByBarcodeVisible   = true;
            this.IsSearchByNameVisible      = false;
            this.selectSearchType("Barcode");
        }

        #region AddProduct

        private void openQuantityView() {
            bool found = false;
            QuantityView quantity_view = new QuantityView(this);
            quantity_view.Left = HomeViewModel.MainView.Left + (HomeViewModel.MainView.ActualWidth / 2) - quantity_view.ActualWidth;
            quantity_view.Top = HomeViewModel.MainView.Top + 160;
            foreach (SaleProductModel sp_modle in SaleProducts) {
                if (sp_modle.ProductID.value == SearchedModel.ID.value && sp_modle.Discount.value == 0) {
                    quantity_view.Quantity = sp_modle.Qunatity.value.ToString();
                    found = true;
                    break;
                }
            }
            if (!found) {
                quantity_view.Quantity = "1";
            }
            quantity_view.Show();
        }
        private void searchProductUsingBarcode(object parameter) {
            ProductModel model;
            if (SearchByBarcode) {
                try { model = ProductAccess.singleton.getProductUsingBarcode(BarcodeOrCode); }
                catch (Exception) { model = null; }
                if (model != null) {
                    SearchedModel = model;
                    openQuantityView();
                }
                else {
                    SearchedModel = null;
                    MessageBox.Show("No product found please check the Barcode!", "No product found", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else if (SearchByCode) {
                try { model = ProductAccess.singleton.getProductUsingCode(Convert.ToInt32(BarcodeOrCode)); }
                catch (Exception) { model = null; }
                if (model != null) {
                    SearchedModel = model;
                    openQuantityView();
                }
                else {
                    SearchedModel = null;
                    MessageBox.Show("No product found please check the Code!", "No product found", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            this.BarcodeOrCode = null;
        }
        public void searchProductUsingName() {
            ProductModel model;
            try { model = ProductAccess.singleton.getProductUsingCode(PhraseNumber); }
            catch (Exception) { model = null; }
            if (model != null) {
                SearchedModel = model;
                openQuantityView();
            }
            else {
                MessageBox.Show("Error adding this product. Please try again.", "Cannot add product", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void addProductToList(ProductModel model, string quantity) {
            bool found = false;
            int int_quantity;
            try { int_quantity = Convert.ToInt32(quantity); }
            catch (Exception) { int_quantity = 1; }
            ObservableCollection<SaleProductModel> temp_list = new ObservableCollection<SaleProductModel>();
            foreach (SaleProductModel sp_model in SaleProducts) {
                temp_list.Add(sp_model);
            }
            for (int i = 0; i < temp_list.Count; i++) {
                if (temp_list[i].ProductID.value == model.ID.value && temp_list[i].Discount.value == 0) {
                    SaleProductModel temp_model = temp_list[i];
                    SaleProducts.Remove(temp_list[i]);
                    temp_model.Qunatity.value = int_quantity;
                    temp_model.SubTotal.value = temp_model.Price.value * temp_model.Qunatity.value;
                    SaleProducts.Insert(i, temp_model);
                    found = true;
                    break;
                }
            }
            if (!found) {
                SaleProductModel sale_product_model = new SaleProductModel();
                sale_product_model.ProductName.value = model.Name.value;
                sale_product_model.ProductID.value = model.ID.value;
                sale_product_model.Qunatity.value = int_quantity;
                sale_product_model.Price.value = model.Price.value;
                sale_product_model.SubTotal.value = model.Price.value * sale_product_model.Qunatity.value;
                SaleProducts.Add(sale_product_model);
            }
            SearchedModel = null;
            calculateTotal();
        }

        #endregion


        private void voidButtonPressed(object parameter) {
            this.SalesViewModel.removeSale(NewSale);
        }

        public void calculateTotal() {
            double sub_total = 0;
            double discount = 0;
            double total = 0;
            foreach (SaleProductModel sale_product in SaleProducts) {
                sub_total += sale_product.Price.value * sale_product.Qunatity.value;
                discount += sale_product.Discount.value;
                total += sale_product.SubTotal.value;
            }
            discount -= CartDiscount;
            total -= CartDiscount;
            this.SubTotal = sub_total.ToString("0.00");
            this.Discount = discount.ToString("0.00");
            this.Total = total.ToString("0.00");
        }
        private void deleteItem(object parameter) {
            SaleProducts.Remove(SelectedItem);
            calculateTotal();
        }
        public void recordSale() {
            if (SaleProducts.Count == 0) { }
            else {
                PaymentModel payment_model = new PaymentModel();
                payment_model.PaymentMethodID.value = 1;
                payment_model.SubTotal.value        = Convert.ToDouble(SubTotal);
                payment_model.Discount.value        = Convert.ToDouble(Discount);
                payment_model.Total.value           = Convert.ToDouble(Total);
                payment_model.Paid.value            = Convert.ToDouble(Paid);
                payment_model.Balance.value         = Convert.ToDouble(Balance);
                payment_model.TransactionTime.value = DateTime.Now;
                int payment_id = SaleAccess.singleton.addPayment(payment_model);

                SaleModel sale_model = new SaleModel();
                sale_model.UserID.value             = this.HomeViewModel.LoggedInUser.ID.value;
                sale_model.PaymentID.value          = payment_id;
                sale_model.CartDiscount.value       = CartDiscount;
                int sale_id = SaleAccess.singleton.addSale(sale_model);

                foreach (SaleProductModel sale_product in SaleProducts) {
                    SaleProductModel sale_product_model = new SaleProductModel();
                    sale_product_model = sale_product;
                    sale_product_model.SaleID.value = sale_id;
                    SaleAccess.singleton.addSaleProduct(sale_product_model);
                    StockModel model = StockAccess.singleton.getStockOfProduct(Convert.ToInt32(sale_product.ProductID.value));
                    if (model != null) {
                        if (model.Quantity.value > sale_product.Qunatity.value) {
                            model.Quantity.value -= sale_product.Qunatity.value;
                            StockAccess.singleton.updateStock(model, Convert.ToInt32(model.ID.value));
                        }else {
                            model.Quantity.value = 0;
                            StockAccess.singleton.updateStock(model, Convert.ToInt32(model.ID.value));
                        }
                    }
                }
                ReciptModel recipt_model = new ReciptModel();
                string recipt_id = string.Format("rpt-{0}", ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds());
                recipt_model.ID.value = recipt_id;
                recipt_model.SaleID.value = sale_id;
                this.ReciptID = recipt_id;
                SaleAccess.singleton.addRecipt(recipt_model);
            }
        }
        public void selectSearchType(object parameter) {
            string para = parameter as string;
            if (para == "Barcode") {
                this.SearchByBarcode = true;
                this.SearchByCode = false;
                this.SearchByName = false;
                this.IsSearchByBarcodeVisible = true;
                this.IsSearchByNameVisible = false;
                NewSale.search_by_barcode_txt_box.Text = "";
                NewSale.search_by_barcode_txt_box.Focus();
                NewSale.barcode_image.Source = new BitmapImage(new Uri("/res/icons/barcode_primary.png", UriKind.Relative));
                NewSale.code_image.Source = new BitmapImage(new Uri("/res/icons/code_secondary.png", UriKind.Relative));
                NewSale.name_image.Source = new BitmapImage(new Uri("/res/icons/name_secondary.png", UriKind.Relative));
            } else if (para == "Code") {
                this.SearchByBarcode = false;
                this.SearchByCode = true;
                this.SearchByName = false;
                this.IsSearchByBarcodeVisible = true;
                this.IsSearchByNameVisible = false;
                NewSale.search_by_barcode_txt_box.Text = "";
                NewSale.search_by_barcode_txt_box.Focus();
                NewSale.barcode_image.Source = new BitmapImage(new Uri("/res/icons/barcode_secondary.png", UriKind.Relative));
                NewSale.code_image.Source = new BitmapImage(new Uri("/res/icons/code_primary.png", UriKind.Relative));
                NewSale.name_image.Source = new BitmapImage(new Uri("/res/icons/name_secondary.png", UriKind.Relative));
            } else {
                this.SearchByBarcode = false;
                this.SearchByCode = false;
                this.SearchByName = true;
                this.IsSearchByBarcodeVisible = false;
                this.IsSearchByNameVisible = true;
                NewSale.barcode_image.Source = new BitmapImage(new Uri("/res/icons/barcode_secondary.png", UriKind.Relative));
                NewSale.code_image.Source = new BitmapImage(new Uri("/res/icons/code_secondary.png", UriKind.Relative));
                NewSale.name_image.Source = new BitmapImage(new Uri("/res/icons/name_primary.png", UriKind.Relative));
                Task.Delay(100).ContinueWith(_ => {
                    Application.Current.Dispatcher.Invoke(new Action(() => {
                        NewSale.search_by_name_txt_box.Focus();
                    }));
                });
            }
        }
        private void discountButtonPressed(object parameter) {
            DiscountView discount_view = new DiscountView(this);
            discount_view.Left = HomeViewModel.MainView.Left + (HomeViewModel.MainView.ActualWidth / 2) - discount_view.ActualWidth;
            discount_view.Top = HomeViewModel.MainView.Top + 160;
            if (SelectedItem != null) {
                discount_view.CartSelected = false;
                discount_view.Show();
            } else {
                discount_view.product_button.IsEnabled = false;
                discount_view.Show();
            }
        }
        private void quantityButtonPressed(object parameter) {
            if (SelectedItem != null) {
                try {
                    ProductModel model = ProductAccess.singleton.getProductUsingProductID(Convert.ToInt32(SelectedItem.ProductID.value));
                    QuantityView quantity_view = new QuantityView(this);
                    quantity_view.Left = HomeViewModel.MainView.Left + (HomeViewModel.MainView.ActualWidth / 2) - quantity_view.ActualWidth;
                    quantity_view.Top = HomeViewModel.MainView.Top + 160;
                    quantity_view.Quantity = SelectedItem.Qunatity.value.ToString();
                    this.SearchedModel = model;
                    quantity_view.Show();
                }
                catch (Exception) { }
            }
        }
        private void payButtonPressed(object parameter) {
            if (this.SaleProducts.Count != 0) {
                PaymentView payment_view = new PaymentView(this);
                payment_view.Left = HomeViewModel.MainView.Left + (HomeViewModel.MainView.ActualWidth / 2) - payment_view.ActualWidth;
                payment_view.Top = HomeViewModel.MainView.Top + 160;
                payment_view.Total = this.Total;
                payment_view.ShowDialog();
            }
        }
        private void returnButtonPressed(object parameter) {
            if (this.SaleProducts.Count != 0) {
                ReturnView return_view = new ReturnView(this);
                return_view.Left = HomeViewModel.MainView.Left + (HomeViewModel.MainView.ActualWidth / 2) - return_view.ActualWidth;
                return_view.Top = HomeViewModel.MainView.Top + 160;
                return_view.RefundAmount = this.Total;
                return_view.ShowDialog();
            }
        }

        #region ReciptPrint

        public void print() {
            PrintDocument document = new PrintDocument();
            PaperSize paperSize = new PaperSize("Custom", 260, 820);
            document.DefaultPageSettings.PaperSize = paperSize;
            document.PrintPage += new PrintPageEventHandler(provideContent);
            try { document.PrinterSettings.PrinterName = CoreApp.singleton.readReciptPrinterName(); document.Print(); }
            catch (Exception) { }
        }

        private void provideContent(object sender, PrintPageEventArgs e) {
            const float WIDTH = 260;
            const float START_Y = 20;
            const float START_X = 4;
            const float TITLE_HEIGHT = 24.5F;
            const float TEXT_HEIGHT = 16;
            float OFFSET = 0;
            float LINE_SPACE = 12;
            float PRODUCT_SPACE = 6;

            const string LINE_BREAK = "--------------------------------------------------------------------------------";


            Graphics graphics = e.Graphics;
            Font title_font = new Font(System.Drawing.FontFamily.GenericSansSerif, 14, System.Drawing.FontStyle.Bold);
            Font text_font = new Font(System.Drawing.FontFamily.GenericSansSerif, 9, System.Drawing.FontStyle.Bold);
            Font item_font = new Font(System.Drawing.FontFamily.GenericSansSerif, 9, System.Drawing.FontStyle.Regular);

            string strAppPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string strFilePath = Path.Combine(strAppPath, "Resources");
            string strFullFilename = Path.Combine(strFilePath, "header_image.png");
            Image image = Image.FromFile(strFullFilename);
            Bitmap bitmap = new Bitmap(image);
            Rectangle page_bounds = e.PageBounds;
            page_bounds.Width = 260;
            page_bounds.Height = 100;
            e.Graphics.DrawImage(bitmap, page_bounds);
            OFFSET += 100;

            graphics.DrawString("Splash Shoes, Chillaw Road,", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            graphics.DrawString("Mahawewa.", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            graphics.DrawString("TEL : (+94) 77 995 6868", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + LINE_SPACE);
            graphics.DrawString("Recipt No : " + ReciptID, text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + LINE_SPACE);
            graphics.DrawString("DATE : " + DateTime.Now, text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            string cashier_first_name = this.HomeViewModel.LoggedInUser.FirstName.value;
            string cashier_last_name = this.HomeViewModel.LoggedInUser.LastName.value;
            graphics.DrawString("CASHIER : " + cashier_first_name + " " + cashier_last_name, text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            graphics.DrawString(LINE_BREAK, text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            foreach (SaleProductModel sale_product_model in SaleProducts) {
                graphics.DrawString(sale_product_model.ProductName.value, item_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
                OFFSET += (TEXT_HEIGHT);
                if (sale_product_model.Discount.value == 0) { graphics.DrawString(sale_product_model.Qunatity.value + " x " + sale_product_model.Price.value.ToString("0.00"), item_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET); }
                else { graphics.DrawString(sale_product_model.Qunatity.value + " x " + sale_product_model.Price.value.ToString("0.00") + " (" + sale_product_model.Discount.value.ToString("0.00") + ")", item_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET); }
                string item_amount = sale_product_model.SubTotal.value.ToString("0.00");
                SizeF item_amoutn_size = measurement(e, item_amount, item_font);
                graphics.DrawString(item_amount, item_font, new SolidBrush(System.Drawing.Color.Black), WIDTH - item_amoutn_size.Width - START_X, START_Y + OFFSET);
                OFFSET += (TEXT_HEIGHT + PRODUCT_SPACE);
            }
            graphics.DrawString(LINE_BREAK, text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            graphics.DrawString("Total Discount: ", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            string discount_amount = this.Discount;
            SizeF discount_amount_size = measurement(e, discount_amount, text_font);
            graphics.DrawString(discount_amount, text_font, new SolidBrush(System.Drawing.Color.Black), WIDTH - discount_amount_size.Width - START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + PRODUCT_SPACE);
            graphics.DrawString("Sub Total: ", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            string sub_total = this.SubTotal;
            SizeF sub_total_size = measurement(e, sub_total, text_font);
            graphics.DrawString(sub_total, text_font, new SolidBrush(System.Drawing.Color.Black), WIDTH - sub_total_size.Width - START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + PRODUCT_SPACE);
            graphics.DrawString("Total: ", title_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            string total = this.Total;
            SizeF total_size = measurement(e, total, title_font);
            graphics.DrawString(total, title_font, new SolidBrush(System.Drawing.Color.Black), WIDTH - total_size.Width - START_X, START_Y + OFFSET);
            OFFSET += (TITLE_HEIGHT + PRODUCT_SPACE);
            graphics.DrawString(LINE_BREAK, text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + PRODUCT_SPACE);
            graphics.DrawString("Paid: ", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            string paid = this.Paid;
            SizeF paid_size = measurement(e, paid, text_font);
            graphics.DrawString(paid, text_font, new SolidBrush(System.Drawing.Color.Black), WIDTH - paid_size.Width - START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + PRODUCT_SPACE);
            graphics.DrawString("Change: ", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            string change = this.Balance;
            SizeF change_size = measurement(e, change, text_font);
            graphics.DrawString(change, text_font, new SolidBrush(System.Drawing.Color.Black), WIDTH - change_size.Width - START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + PRODUCT_SPACE);
            graphics.DrawString(LINE_BREAK, text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + PRODUCT_SPACE);
            string thank_you = "Thank you, Come again!";
            SizeF thank_you_size = measurement(e, thank_you, text_font);
            graphics.DrawString(thank_you, text_font, new SolidBrush(System.Drawing.Color.Black), (WIDTH / 2) - (thank_you_size.Width / 2), START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + PRODUCT_SPACE);
            string credits_one = "Developed by:";
            SizeF credits_one_size = measurement(e, credits_one, text_font);
            graphics.DrawString(credits_one, text_font, new SolidBrush(System.Drawing.Color.Black), (WIDTH / 2) - (credits_one_size.Width / 2), START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            string credits_two = "BOOT LOOP TECHNOLOGIES";
            SizeF credits_two_size = measurement(e, credits_two, text_font);
            graphics.DrawString(credits_two, text_font, new SolidBrush(System.Drawing.Color.Black), (WIDTH / 2) - (credits_two_size.Width / 2), START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            string credits_three = "Contact: (+94) 77 371 1120";
            SizeF credits_three_size = measurement(e, credits_three, text_font);
            graphics.DrawString(credits_three, text_font, new SolidBrush(System.Drawing.Color.Black), (WIDTH / 2) - (credits_three_size.Width / 2), START_Y + OFFSET);
        }
        private SizeF measurement(PrintPageEventArgs e, string text, Font font) {
            Graphics graphics = e.Graphics;
            SizeF measurements = graphics.MeasureString(text, font);
            return measurements;
        }

        #endregion

        private bool isSelectedItemNotNull(object parameter) {
            return SelectedItem == null ? false : true;
        }
        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }


    public class SearchDataProvider : WpfAutoComplete.ISearchDataProvider {

        public SearchDataProvider() {
            List<ProductModel> products = ProductAccess.singleton.getProducts("");
            foreach(ProductModel product in products) {
                dict.Add(Convert.ToInt32(product.Code.value), product.Name.value);
            }
        }

        public WpfAutoComplete.SearchResult DoSearch(string searchTerm) {
            return new WpfAutoComplete.SearchResult {
                SearchTerm = searchTerm,
                Results = dict.Where(item => item.Value.ToUpperInvariant().Contains(searchTerm.ToUpperInvariant())).ToDictionary(v => v.Key, v => v.Value)
            };
        }
        public WpfAutoComplete.SearchResult SearchByKey(object Key) {
            return new WpfAutoComplete.SearchResult {
                SearchTerm = null,
                Results = dict.Where(item => item.Key.ToString() == Key.ToString()).ToDictionary(v => v.Key, v => v.Value)
            };
        }

        private Dictionary<object, string> dict = new Dictionary<object, string> {
            { 0, ""},
        };
    }
}
