using System;
using System.Data;

namespace Core.DB.Models
{
    public class SaleProductWithNameModel : Model {
        public TextField ProductName    { get; set; } = new TextField(      name: "ProductName");
        public IntergerField Sale_ID    { get; set; } = new IntergerField(  name: "Sale_ID");
        public IntergerField Quantity   { get; set; } = new IntergerField(  name: "Quantity");
        public FloatField Discount      { get; set; } = new FloatField(     name: "Discount");
        public FloatField Price         { get; set; } = new FloatField(     name: "Price");
        public FloatField SubTotal      { get; set; } = new FloatField(     name: "SubTotal");

        public SaleProductWithNameModel() { }

        public SaleProductWithNameModel(DataRow data_row) {
            this.ProductName.value  = Convert.ToString(data_row["ProductName"]);
            this.Sale_ID.value      = Convert.ToInt32(data_row["Sale_ID"]);
            this.Quantity.value     = Convert.ToInt32(data_row["Quantity"]);
            if (!data_row.IsNull("Discount")) this.Discount.value = Convert.ToDouble(data_row["Discount"]); else this.Discount.setToNull();
            this.Price.value        = Convert.ToDouble(data_row["Price"]);
            this.SubTotal.value     = Convert.ToDouble(data_row["SubTotal"]);
        }

    }
}
