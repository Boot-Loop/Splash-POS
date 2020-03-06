﻿using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.Documents
{
    public class ProductsDocument : Doc
    {
        private static readonly ProductsDocument instance = new ProductsDocument();
        private ProductsDocument() { }

        public static ProductsDocument singleton {
            get { return instance; }
        }

        public void export(List<ProductModel> product_models) {
            try {
                DataTable data_table = makeDataTable(product_models);
                exportDataTableToPdf(data_table, @"C:\Users\Azeem Muzammil\Desktop\test.pdf", "Product List");
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private DataTable makeDataTable(List<ProductModel> product_models) {

            DataTable data_table = new DataTable();

            //Define columns
            data_table.Columns.Add("Code");
            data_table.Columns.Add("Name");
            data_table.Columns.Add("Barcode");
            data_table.Columns.Add("Description");
            data_table.Columns.Add("Price");

            foreach (ProductModel model in product_models) {
                data_table.Rows.Add(model.Code.value, model.Name.value, model.Barcode.value, model.Description.value, model.Price.value.ToString("0.00"));
            }

            return data_table;
        }


    }
}