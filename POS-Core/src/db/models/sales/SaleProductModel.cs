using System;
using System.Data;

namespace Core.DB.Models
{
    public class SaleProductModel : Model
    {
        public IntergerField    ID          { get; set; } = new IntergerField(  name: "ID");
        public IntergerField    SaleID      { get; set; } = new IntergerField(  name: "SaleID");
        public IntergerField    ProductID   { get; set; } = new IntergerField(  name: "ProductID",  is_required: true);
        public IntergerField    Qunatity    { get; set; } = new IntergerField(  name: "Qunatity",   is_required: true);
        public FloatField       Discount    { get; set; } = new FloatField(     name: "Discount");
        public FloatField       Price       { get; set; } = new FloatField(     name: "Price",      is_required: true);
        public TextField        ProductName { get; set; } = new TextField(      name: "ProductName");
        public FloatField       SubTotal    { get; set; } = new FloatField(     name: "SubTotal");

        public SaleProductModel() { }

        public SaleProductModel(DataRow data_row) {
            this.ID.value           = Convert.ToInt32(data_row["ID"]);
            this.SaleID.value       = Convert.ToInt32(data_row["Sale_ID"]);
            this.ProductID.value    = Convert.ToInt32(data_row["Product_ID"]);
            this.Qunatity.value     = Convert.ToInt32(data_row["Qunatity"]);
            if (!data_row.IsNull("Discount")) this.Discount.value = Convert.ToDouble(data_row["Discount"]); else this.Discount.setToNull();
            this.Price.value        = Convert.ToInt32(data_row["Price"]);
        }

    }
}
