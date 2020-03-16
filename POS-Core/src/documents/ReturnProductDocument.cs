using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using Core.DB.Models;
using CoreApp = Core.Application;

namespace Core.Documents
{
    public class ReturnProductDocument : ReportDoc
    {
        private static readonly ReturnProductDocument instance = new ReturnProductDocument();

        private ReturnProductDocument() { }

        public static ReturnProductDocument singleton {
            get { return instance; }
        }

        public void export(List<ProductReturnWithNameModel> return_models, string from, string to, string footer_text) {
            try {
                DataTable data_table = makeDataTable(return_models);
                exportDataTableToPdf(data_table, Path.Combine(CoreApp.singleton.readDocumentSavePath(), "ProductReturnReport.pdf"), "Product Return Report", from, to, footer_text);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void exportToPrint(List<ProductReturnWithNameModel> return_models, string from, string to, string footer_text) {
            try {
                DataTable data_table = makeDataTable(return_models);
                exportDataTableToPdf(data_table, Path.Combine(Paths.TEMP_FILE), "Product Return Report", from, to, footer_text);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private DataTable makeDataTable(List<ProductReturnWithNameModel> return_models) {

            DataTable data_table = new DataTable();

            data_table.Columns.Add("Product Name");
            data_table.Columns.Add("Quantity");
            data_table.Columns.Add("Refund Amount");
            data_table.Columns.Add("Transaction Time");

            foreach (ProductReturnWithNameModel model in return_models) {
                data_table.Rows.Add(model.ProductName.value, model.Quantity.value, model.RefundAmount.value, model.TransactionTime.value);
            }
            return data_table;
        }
    }
}
