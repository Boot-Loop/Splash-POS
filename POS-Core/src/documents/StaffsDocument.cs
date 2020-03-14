using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using Core.DB.Models;
using CoreApp = Core.Application;

namespace Core.Documents
{
    public class StaffsDocument : Doc
    {
        private static readonly StaffsDocument instance = new StaffsDocument();

        private StaffsDocument() { }

        public static StaffsDocument singleton {
            get { return instance; }
        }

        public void export(List<StaffModel> staff_models) {
            try {
                DataTable data_table = makeDataTable(staff_models);
                exportDataTableToPdf(data_table, Path.Combine(CoreApp.singleton.readDocumentSavePath(), "Staffs.pdf"), "Staffs List");
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private DataTable makeDataTable(List<StaffModel> staff_models) {

            DataTable data_table = new DataTable();

            data_table.Columns.Add("Staff ID");
            data_table.Columns.Add("First Name");
            data_table.Columns.Add("Last Name");
            data_table.Columns.Add("User Name");
            data_table.Columns.Add("EMail");

            foreach (StaffModel model in staff_models) {
                data_table.Rows.Add(model.ID.value, model.FirstName.value, model.LastName.value, model.UserName.value, model.EMail.value);
            }
            return data_table;
        }
    }
}
