using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Input;

namespace UI.ViewModels.Commands
{
    public class ReciptPrintCommand : ICommand
    {
        public List<SaleProductModel> SaleProductModels{ get; set; }

        public ReciptPrintCommand(List<SaleProductModel> sale_product_models) {
            this.SaleProductModels = new List<SaleProductModel>();
            this.SaleProductModels = sale_product_models;
            Console.WriteLine(this.SaleProductModels.Count + "  command");
        }
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            PrintDocument document = new PrintDocument();
            PaperSize paperSize = new PaperSize("Custom", 520, 820);
            document.DefaultPageSettings.PaperSize = paperSize;
            document.PrintPage += new PrintPageEventHandler(ProvideContent);
            document.Print();
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
            foreach (SaleProductModel sale_product_model in SaleProductModels) {
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
