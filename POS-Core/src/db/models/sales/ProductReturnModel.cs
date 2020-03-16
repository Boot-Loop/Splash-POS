using System;
using System.Data;

namespace Core.DB.Models
{
    public class ProductReturnModel : Model
    {
        public IntergerField    ID              { get; set; } = new IntergerField(  name: "ID");
        public TextField        ReciptID        { get; set; } = new TextField(      name: "ReciptID",       is_required: true);
        public IntergerField    ProductID       { get; set; } = new IntergerField(  name: "ProductID",      is_required: true);
        public IntergerField    Qunatity        { get; set; } = new IntergerField(  name: "Qunatity",       is_required: true);
        public FloatField       RefuntAmount    { get; set; } = new FloatField(     name: "RefuntAmount",   is_required: true);
        public DateTimeField    TransactionTime { get; set; } = new DateTimeField(  name: "TransactionTime");

        public ProductReturnModel() { }

        public ProductReturnModel(DataRow data_row) {
            this.ID.value               = Convert.ToInt32(data_row["ID"]);
            this.ReciptID.value         = Convert.ToString(data_row["Recipt_ID"]);
            this.ProductID.value        = Convert.ToInt32(data_row["Product_ID"]);
            this.Qunatity.value         = Convert.ToInt32(data_row["Quantity"]);
            this.RefuntAmount.value     = Convert.ToDouble(data_row["RefundAmount"]);
            this.TransactionTime.value  = Convert.ToDateTime(data_row["TransactionTime"]);
        }
    }
}