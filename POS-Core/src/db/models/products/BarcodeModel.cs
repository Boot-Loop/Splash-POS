using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DB.Models
{
    public class BarcodeModel : Model
    {
        public IntergerField    ID          { get; set; } = new IntergerField(  name: "ID");
        public IntergerField    ProductID   { get; set; } = new IntergerField(  name: "Product_ID", is_required: true);
        public TextField        Value       { get; set; } = new TextField(      name: "Value",      is_required: true);
        
        public BarcodeModel(DataRow data_row) {
            this.ID.value           = Convert.ToInt32(data_row["ID"]);
            this.ProductID.value    = Convert.ToInt32(data_row["Product_ID"]);
            this.Value.value        = data_row["Value"].ToString();
        }

        public BarcodeModel() { }

        public override object getPK() => ID.value;
        public override ModelType getType() => ModelType.MODEL_BARCODE;
        public override bool matchPK(object pk) { return false; }
        public override void validateRelation() { }
    }
}
