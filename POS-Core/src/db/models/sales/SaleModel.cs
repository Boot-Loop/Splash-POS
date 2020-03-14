using System;
using System.Data;

namespace Core.DB.Models
{
    public class SaleModel : Model
    {
        public IntergerField ID         { get; set; } = new IntergerField(  name: "ID");
        public IntergerField UserID     { get; set; } = new IntergerField(  name: "UserID");
        public IntergerField CustomerID { get; set; } = new IntergerField(  name: "CustomerID");
        public IntergerField PaymentID  { get; set; } = new IntergerField(  name: "PaymentID",      is_required: true);
        public FloatField CartDiscount  { get; set; } = new FloatField(     name: "CartDiscount",   is_required: true);

        public SaleModel() { }

        public SaleModel(DataRow data_row) {
            this.ID.value           = Convert.ToInt32(data_row["ID"]);
            if (!data_row.IsNull("User_ID")) this.UserID.value          = Convert.ToInt32(data_row["User_ID"]); else this.UserID.setToNull();
            if (!data_row.IsNull("Customer_ID")) this.CustomerID.value  = Convert.ToInt32(data_row["Customer_ID"]); else this.CustomerID.setToNull();
            this.PaymentID.value    = Convert.ToInt32(data_row["Payment_ID"]);
            this.CartDiscount.value = Convert.ToDouble(data_row["CartDiscount"]);
        }
    }
}
