using Core.DB;
using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DB.Models
{
    public class SaleProductModel : Model
    {
        public IntergerField SaleID { get; set; } = new IntergerField(name: "SaleID");
        public IntergerField ProductID { get; set; } = new IntergerField(name: "ProductID", is_required: true);
        public IntergerField Qunatity { get; set; } = new IntergerField(name: "Qunatity", is_required: true);
        public FloatField Discount { get; set; } = new FloatField(name: "Discount");
        public FloatField Price { get; set; } = new FloatField(name: "Price", is_required: true);
        public TextField ProductName { get; set; } = new TextField(name: "ProductName");
        public FloatField SubTotal { get; set; } = new FloatField(name: "SubTotal");

        public SaleProductModel(DataRow data_row) {
            this.SaleID.value = Convert.ToInt32(data_row["Sale_ID"]);
            this.ProductID.value = Convert.ToInt32(data_row["Product_ID"]);
            this.Qunatity.value = Convert.ToInt32(data_row["Qunatity"]);
            if (!data_row.IsNull("Discount")) this.Discount.value = Convert.ToDouble(data_row["Discount"]); else this.Discount.setToNull();
            this.Price.value = Convert.ToInt32(data_row["Price"]);
        }

        public SaleProductModel() { }

        public override object getPK() => null;
        public override ModelType getType() => ModelType.MODEL_SALE_PRODUCT;
        public override bool matchPK(object pk) { return false; }
        public override void validateRelation() { }
    }
}
