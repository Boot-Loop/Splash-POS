using System;
using System.Data;

namespace Core.DB.Models
{
    public class ReciptModel : Model
    {
        public TextField        ID      { get; set; } = new TextField(      name: "ID",     is_required: true);
        public IntergerField    SaleID  { get; set; } = new IntergerField(  name: "SaleID", is_required: true);

        public ReciptModel() { }

        public ReciptModel(DataRow data_row) {
            this.ID.value       = Convert.ToString(data_row["ID"]);
            this.SaleID.value   = Convert.ToInt32(data_row["Sale_ID"]);
        }

    }
}
