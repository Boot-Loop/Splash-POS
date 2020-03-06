using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.Documents
{
    public class StocksDocument : Doc
    {
        private static readonly StocksDocument instance = new StocksDocument();
        private StocksDocument() { }

        public static StocksDocument singleton {
            get { return instance; }
        }

        public void export(List<StockModel> stock_models) {
            try {
                DataTable data_table = makeDataTable(stock_models);
                exportDataTableToPdf(data_table, @"C:\Users\Azeem Muzammil\Desktop\test.pdf", "Stocks List");
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private DataTable makeDataTable(List<StockModel> stock_model) {

            DataTable data_table = new DataTable();

            //Define columns
            data_table.Columns.Add("Stock ID");
            data_table.Columns.Add("Product Name");
            data_table.Columns.Add("Supplier Name");
            data_table.Columns.Add("Quantity");
            data_table.Columns.Add("Unit Price");
            data_table.Columns.Add("Date");

            foreach (StockModel model in stock_model) {
                data_table.Rows.Add(model.ID.value, model.ProductName.value, model.SupplierName.value, model.Quantity.value, model.UnitPrice.value.ToString("0.00"), model.Date.value);
            }

            return data_table;
        }


    }
}
