using Core.DB.Access;
using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UI.ViewModels.Commands;
using UI.Views;

namespace UI.ViewModels
{
    public class NewSaleViewModel : INotifyPropertyChanged
    {
        private string _barcode;
        private string _subtotal;
        private string _discount;
        private string _total;
        private int _sale_id;
        private bool _search_by_name;
        private ObservableCollection<SaleProductModel> _sale_products;
        private ObservableCollection<ProductModel> _search_products;
        public RelayCommand BarcodeAddCommand { get; private set; }
        public RelayCommand DeleteItemCommand { get; private set; }
        public RelayCommand VoidSaleCommand { get; private set; }
        public RelayCommand DoPaymentCommand { get; private set; }
        public RelayCommand ReciptPrintCommand { get; private set; }
        public SaleProductModel SelectedItem { get; set; }
        public SalesViewModel SalesViewModel { get; set; }
        public HomeViewModel HomeViewModel { get; set; }
        public NewSale NewSale { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Barcode {
            get { return _barcode; }
            set { _barcode = value; onPropertyRaised("Barcode"); searchByName(Barcode); }
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
        public int SaleID {
            get { return _sale_id; }
            set { _sale_id = value; onPropertyRaised("SaleID"); }
        }
        public bool SearchByName {
            get { return _search_by_name; }
            set { _search_by_name = value; onPropertyRaised("SearchByName"); }
        }
        public ObservableCollection<SaleProductModel> SaleProducts {
            get { return _sale_products; }
            set { _sale_products = value; onPropertyRaised("SaleProducts"); }
        }
        public ObservableCollection<ProductModel> SearchProducts {
            get { return _search_products; }
            set { _search_products = value; onPropertyRaised("SearchProducts"); }
        }

        public NewSaleViewModel(NewSale new_sale, SalesViewModel sales_view_model, HomeViewModel home_view_model) {
            this.NewSale = new_sale;
            this.SalesViewModel = sales_view_model;
            this.HomeViewModel = home_view_model;
            this.BarcodeAddCommand = new RelayCommand(enterPressedOnBarcodeSearch);
            this.DeleteItemCommand = new RelayCommand(deleteItem, isSelectedItemNotNull);
            this.VoidSaleCommand = new RelayCommand(voidButtonPressed);
            this.DoPaymentCommand = new RelayCommand(doPayment);
            this.SaleProducts = new ObservableCollection<SaleProductModel>();
            this.SearchProducts = new ObservableCollection<ProductModel>();
            this.ReciptPrintCommand = new RelayCommand(print);
            this.SubTotal = "0.00";
            this.Discount = "0.00";
            this.Total = "0.00";
            this.SaleID = 123;
        }

        private void enterPressedOnBarcodeSearch(object parameter) {
            ProductModel model = ProductAccess.singleton.getProductUsingBarcode(Barcode);
            if (model != null) {
                addProductToList(model);
            } else {
                MessageBox.Show("No product found please check the Barcode!", "No product found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.SubTotal = calculateTotal()[0].ToString("0.00");
            this.Total = calculateTotal()[2].ToString("0.00");
            this.Barcode = null;
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
        private void voidButtonPressed(object parameter) {
            this.SalesViewModel.removeSale(NewSale);
        }

        private List<double> calculateTotal() {
            double sub_total = 0;
            double discount = 0;
            foreach (SaleProductModel sale_product in SaleProducts) {
                sub_total += sale_product.SubTotal.value;
                discount += sale_product.Discount.value;
            }
            double total = sub_total - discount;
            return new List<double>() { sub_total, discount, total };
        }
        private void deleteItem(object parameter) {
            SaleProducts.Remove(SelectedItem);
            this.SubTotal = calculateTotal()[0].ToString("0.00");
            this.Total = calculateTotal()[2].ToString("0.00");
        }
        private void doPayment(object parameter) {
            if (SaleProducts.Count == 0) { }
            else {
                PaymentModel payment_model = new PaymentModel();
                payment_model.Amount.value = Convert.ToDouble(Total);
                payment_model.TransactionTime.value = DateTime.Now;
                int payment_id = SaleAccess.singleton.addPayment(payment_model);

                SaleModel sale_model = new SaleModel();
                sale_model.UserID.value = this.HomeViewModel.LoggedInUser.ID.value;
                sale_model.PaymentID.value = payment_id;
                int sale_id = SaleAccess.singleton.addSale(sale_model);

                foreach (SaleProductModel sale_product in SaleProducts) {
                    SaleProductModel sale_product_model = new SaleProductModel();
                    sale_product_model = sale_product;
                    sale_product_model.SaleID.value = sale_id;
                    SaleAccess.singleton.addSaleProduct(sale_product_model);
                }
            }
        }
        private void searchByName(object parameter) {
            if (string.IsNullOrEmpty(Barcode)) { SearchProducts = new ObservableCollection<ProductModel>(); }
            else { SearchProducts = new ObservableCollection<ProductModel>(ProductAccess.singleton.searchProducts(Barcode)); }
            Console.WriteLine(SearchProducts.Count);
        }
        private bool isSelectedItemNotNull(object parameter) {
            return SelectedItem == null ? false : true;
        }

        private void onPropertyRaised(string property_name) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }


        private void print(object parameter) {
            foreach (string printer in PrinterSettings.InstalledPrinters) {
                Console.WriteLine(printer);
            }
            //PrintDocument document = new PrintDocument();
            //PaperSize paperSize = new PaperSize("Custom", 520, 820);
            //document.DefaultPageSettings.PaperSize = paperSize;
            //document.PrintPage += new PrintPageEventHandler(ProvideContent);
            //document.PrinterSettings.PrinterName = "";
            //document.Print();
        }

        private void ProvideContent(object sender, PrintPageEventArgs e) {
            const float WIDTH = 260;
            const float START_Y = 20;
            const float START_X = 4;
            const float TITLE_HEIGHT = 24.5F;
            const float TEXT_HEIGHT = 16;
            float OFFSET = 0;
            float LINE_SPACE = 12;
            float PRODUCT_SPACE = 6;

            const string TITLE = "SPLASH SHOES";
            const string LINE_BREAK = "--------------------------------------------------------------------------------";


            Graphics graphics = e.Graphics;
            Font title_font = new Font(System.Drawing.FontFamily.GenericSansSerif, 14, System.Drawing.FontStyle.Bold);
            Font text_font = new Font(System.Drawing.FontFamily.GenericSansSerif, 9, System.Drawing.FontStyle.Bold);

            SizeF measurements = measurement(e, TITLE, title_font);
            graphics.DrawString(TITLE, title_font, new SolidBrush(System.Drawing.Color.Black), (WIDTH / 2) - (measurements.Width / 2), START_Y + OFFSET);
            OFFSET += (TITLE_HEIGHT + LINE_SPACE);
            graphics.DrawString("Splash Shoes, Chillaw Road,", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            graphics.DrawString("Mahawewa.", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            graphics.DrawString("TEL : (+94) 77 995 6868", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + LINE_SPACE);
            graphics.DrawString("Recipt No : 101", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + LINE_SPACE);
            graphics.DrawString("DATE : " + DateTime.Now, text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            graphics.DrawString("CASHIER : Azeem Muzammil", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            graphics.DrawString(LINE_BREAK, text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            foreach (SaleProductModel sale_product_model in SaleProducts)
            {
                graphics.DrawString(sale_product_model.ProductName.value, text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
                OFFSET += (TEXT_HEIGHT);
                graphics.DrawString(sale_product_model.Qunatity.value + " x " + sale_product_model.Price.value.ToString("0.00"), text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
                string item_amount = sale_product_model.SubTotal.value.ToString("0.00");
                SizeF item_amoutn_size = measurement(e, item_amount, text_font);
                graphics.DrawString(item_amount, text_font, new SolidBrush(System.Drawing.Color.Black), WIDTH - item_amoutn_size.Width - START_X, START_Y + OFFSET);
                OFFSET += (TEXT_HEIGHT + PRODUCT_SPACE);
            }
            graphics.DrawString(LINE_BREAK, text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT);
            graphics.DrawString("Total Discount: ", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            string discount_amount = "0.00";
            SizeF discount_amount_size = measurement(e, discount_amount, text_font);
            graphics.DrawString(discount_amount, text_font, new SolidBrush(System.Drawing.Color.Black), WIDTH - discount_amount_size.Width - START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + PRODUCT_SPACE);
            graphics.DrawString("Sub Total: ", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            string sub_total = "1245.00";
            SizeF sub_total_size = measurement(e, sub_total, text_font);
            graphics.DrawString(sub_total, text_font, new SolidBrush(System.Drawing.Color.Black), WIDTH - sub_total_size.Width - START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + PRODUCT_SPACE);
            graphics.DrawString("Total: ", title_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            string total = "3000.40";
            SizeF total_size = measurement(e, total, title_font);
            graphics.DrawString(total, title_font, new SolidBrush(System.Drawing.Color.Black), WIDTH - total_size.Width - START_X, START_Y + OFFSET);
            OFFSET += (TITLE_HEIGHT + PRODUCT_SPACE);
            graphics.DrawString(LINE_BREAK, text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + PRODUCT_SPACE);
            graphics.DrawString("Paid: ", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            string paid = "1245.00";
            SizeF paid_size = measurement(e, paid, text_font);
            graphics.DrawString(paid, text_font, new SolidBrush(System.Drawing.Color.Black), WIDTH - paid_size.Width - START_X, START_Y + OFFSET);
            OFFSET += (TEXT_HEIGHT + PRODUCT_SPACE);
            graphics.DrawString("Change: ", text_font, new SolidBrush(System.Drawing.Color.Black), START_X, START_Y + OFFSET);
            string change = "220.00";
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

    }
}
