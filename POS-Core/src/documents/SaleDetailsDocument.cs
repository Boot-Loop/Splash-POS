using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using Core.DB.Models;
using CoreApp = Core.Application;

namespace Core.Documents
{
    public class SaleDetailDocument : ReportDoc
    {
        private static readonly SaleDetailDocument instance = new SaleDetailDocument();

        private SaleDetailDocument() { }

        public static SaleDetailDocument singleton {
            get { return instance; }
        }

        public void export(List<SaleDetailModel> sale_details, string from, string to, string footer_text) {
            try {
                DataTable data_table = makeDataTable(sale_details);
                exportDataTableToPdf(data_table, Path.Combine(CoreApp.singleton.readDocumentSavePath(), "SaleDetails.pdf"), "Sales Report", from, to, footer_text);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void exportToPrint(List<SaleDetailModel> sale_details, string from, string to, string footer_text) {
            try {
                DataTable data_table = makeDataTable(sale_details);
                exportDataTableToPdf(data_table, Path.Combine(Paths.TEMP_FILE), "Sales Report", from, to, footer_text);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private DataTable makeDataTable(List<SaleDetailModel> sale_models) {

            DataTable data_table = new DataTable();

            data_table.Columns.Add("SaleID");
            data_table.Columns.Add("SubTotal");
            data_table.Columns.Add("Discount");
            data_table.Columns.Add("Total");
            data_table.Columns.Add("Transaction Time");

            foreach (SaleDetailModel model in sale_models) {
                data_table.Rows.Add(model.SaleID.value, model.SubTotal.value, model.Discount.value, model.Total.value, model.TransactionTime.value);
            }
            return data_table;
        }
    }
}
