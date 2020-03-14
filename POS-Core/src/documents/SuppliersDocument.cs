using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using Core.DB.Models;
using CoreApp = Core.Application;

namespace Core.Documents
{
    public class SuppliersDocument : Doc
    {
        private static readonly SuppliersDocument instance = new SuppliersDocument();

        private SuppliersDocument() { }

        public static SuppliersDocument singleton {
            get { return instance; }
        }

        public void export(List<SupplierModel> supplier_model) {
            try {
                DataTable data_table = makeDataTable(supplier_model);
                exportDataTableToPdf(data_table, Path.Combine(CoreApp.singleton.readDocumentSavePath(), "Suppliers.pdf"), "Supplier List");
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private DataTable makeDataTable(List<SupplierModel> supplier_model) {

            DataTable data_table = new DataTable();

            data_table.Columns.Add("First Name");
            data_table.Columns.Add("Last Name");
            data_table.Columns.Add("Address");
            data_table.Columns.Add("EMail");
            data_table.Columns.Add("Telephone");
            data_table.Columns.Add("Comments");

            foreach (SupplierModel model in supplier_model) {
                data_table.Rows.Add(model.FirstName.value, model.LastName.value, model.Address.value, model.EMail.value, model.Telephone.value, model.Comments.value);
            }
            return data_table;
        }


    }
}
