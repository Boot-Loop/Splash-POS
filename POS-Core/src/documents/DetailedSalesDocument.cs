using iTextSharp.text;
using iTextSharp.text.pdf;

using System;
using System.Data;
using System.IO;
using System.Collections.Generic;

using Core.DB.Models;
using CoreApp = Core.Application;
using Core.DB.Access;

namespace Core.Documents
{
    public class DetailedSalesDocument
    {
        private static readonly DetailedSalesDocument instance = new DetailedSalesDocument();

        private DetailedSalesDocument() { }

        public static DetailedSalesDocument singleton {
            get { return instance; }
        }

        public void export(List<SaleDetailModel> sales, string from, string to) {
            List<DataTable> data_tables = new List<DataTable>();
            foreach(SaleDetailModel sale in sales) {
                try {
                    List<SaleProductWithNameModel> sale_products = SaleAccess.singleton.getSaleProductsWithName(Convert.ToInt32(sale.SaleID.value));
                    DataTable data_table = makeDataTable(sale_products);
                    data_tables.Add(data_table);
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
            try {
                exportDataTableToPdf(data_tables, sales, Path.Combine(CoreApp.singleton.readDocumentSavePath(), "DetailedSalesReport.pdf"), "Detailed Sales Report", from, to);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        //public void exportToPrint(List<ProductReturnWithNameModel> return_models, string from, string to, string footer_text) {
        //    try {
        //        DataTable data_table = makeDataTable(return_models);
        //        exportDataTableToPdf(data_table, Path.Combine(Paths.TEMP_FILE), "Product Return Report", from, to, footer_text);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        private DataTable makeDataTable(List<SaleProductWithNameModel> sale_products) {

            DataTable data_table = new DataTable();

            data_table.Columns.Add("Product Name");
            data_table.Columns.Add("Quantity");
            data_table.Columns.Add("Discount");
            data_table.Columns.Add("Price");
            data_table.Columns.Add("SubTotal");

            foreach (SaleProductWithNameModel model in sale_products) {
                data_table.Rows.Add(model.ProductName.value, model.Quantity.value, model.Discount.value, model.Price.value, model.SubTotal.value);
            }
            return data_table;
        }

        public void exportDataTableToPdf(List<DataTable> list_data_table, List<SaleDetailModel> sales, String pdf_path, string header, string from, string to) {
            FileStream file_stream = new FileStream(pdf_path, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document();
            document.SetPageSize(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, file_stream);
            document.Open();

            //Report Header
            BaseFont base_font_header = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font_head = new Font(base_font_header, 16, 1, Color.GRAY);
            Paragraph paragraph_heading = new Paragraph();
            paragraph_heading.Alignment = Element.ALIGN_CENTER;
            paragraph_heading.Add(new Chunk(header.ToUpper(), font_head));
            document.Add(paragraph_heading);

            //Author
            Paragraph paragraph_author = new Paragraph();
            BaseFont base_font_author = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font_author = new Font(base_font_author, 8, 2, Color.GRAY);
            paragraph_author.Alignment = Element.ALIGN_RIGHT;
            paragraph_author.Add(new Chunk("Splash Shoe", font_author));
            paragraph_author.Add(new Chunk($"\nDate : {from} - {to}", font_author));
            document.Add(paragraph_author);

            //Add a line seperation
            Paragraph paragraph = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, Color.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(paragraph);

            //Add line break
            document.Add(new Chunk("\n", font_head));

            for(int i = 0; i < sales.Count; i++) {
                Paragraph sale_header = new Paragraph();
                BaseFont base_font_sale_header = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font font_sale_header = new Font(base_font_sale_header, 12, 2, Color.BLACK);
                sale_header.Alignment = Element.ALIGN_LEFT;
                sale_header.Add(new Chunk($"        Sale ID : {sales[i].SaleID}    /    Transaction Time : {sales[i].TransactionTime}\n\n", font_sale_header));
                document.Add(sale_header);

                PdfPTable table = new PdfPTable(list_data_table[i].Columns.Count);

                BaseFont base_font_coulum_header = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fntColumnHeader = new Font(base_font_coulum_header, 10, 1, Color.WHITE);

                for (int j = 0; j < list_data_table[i].Columns.Count; j++) {
                    PdfPCell cell = new PdfPCell();
                    cell.BackgroundColor = Color.GRAY;
                    cell.AddElement(new Chunk(list_data_table[i].Columns[j].ColumnName.ToUpper(), fntColumnHeader));
                    table.AddCell(cell);
                }

                for (int j = 0; j < list_data_table[i].Rows.Count; j++) {
                    for (int k = 0; k < list_data_table[i].Columns.Count; k++) {
                        table.AddCell(list_data_table[i].Rows[j][k].ToString());
                    }
                }

                document.Add(table);

                double total = sales[i].Total.value;
                double discunt = sales[i].Discount.value;
                double subtotal = sales[i].SubTotal.value;
                string footer = $"\nSub Total : {subtotal.ToString("0.00")}\nDiscount : {discunt.ToString("0.00")}\nTotal : {total.ToString("0.00")}";

                Paragraph paragraph_footer = new Paragraph();
                BaseFont base_font_footer = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font font_footer = new Font(base_font_footer, 12, 2, Color.BLACK);
                paragraph_footer.Alignment = Element.ALIGN_RIGHT;
                paragraph_footer.Add(new Chunk(footer, font_footer));
                document.Add(paragraph_footer);
            }

            double entire_total = 0;
            double entire_discunt = 0;
            double entire_subtotal = 0;
            foreach (SaleDetailModel model in sales) {
                entire_discunt += model.Discount.value;
                entire_subtotal += model.SubTotal.value;
                entire_total += model.Total.value;
            }
            string entire_footer = $"\n\nSub Total for the period : {entire_subtotal.ToString("0.00")}\nDiscount for the period : {entire_discunt.ToString("0.00")}\nTotal for the period : {entire_total.ToString("0.00")}";

            Paragraph paragraph_entire_footer = new Paragraph();
            BaseFont base_font_entire_footer = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font_entire_footer = new Font(base_font_entire_footer, 14, 2, Color.BLACK);
            paragraph_entire_footer.Alignment = Element.ALIGN_RIGHT;
            paragraph_entire_footer.Add(new Chunk(entire_footer, font_entire_footer));
            document.Add(paragraph_entire_footer);

            document.Close();
            writer.Close();
            file_stream.Close();
        }
    }
}