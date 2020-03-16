using System;
using System.Data;

namespace Core.DB.Models {
    public class ProductReturnWithNameModel : Model {
        public TextField        ProductName     { get; set; } = new TextField(      name: "ProductName");
        public IntergerField    Quantity        { get; set; } = new IntergerField(  name: "Quantity");
        public FloatField       RefundAmount    { get; set; } = new FloatField(     name: "RefundAmount");
        public DateTimeField    TransactionTime { get; set; } = new DateTimeField(  name: "TransactionTime");

        public ProductReturnWithNameModel() { }

        public ProductReturnWithNameModel(DataRow data_row) {
            this.ProductName.value      = Convert.ToString(data_row["ProductName"]);
            this.Quantity.value         = Convert.ToInt32(data_row["Quantity"]);
            this.RefundAmount.value     = Convert.ToDouble(data_row["RefundAmount"]);
            this.TransactionTime.value  = Convert.ToDateTime(data_row["TransactionTime"]);
        }

    }
}
