using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DB.Models
{
    public class MeasurementUnitModel : Model
    {
        public IntergerField ID { get; set; } = new IntergerField(name: "ID");
        public TextField Value { get; set; } = new TextField(name: "Value", is_required: true);

        public MeasurementUnitModel(DataRow data_row)
        {
            this.ID.value = Convert.ToInt32(data_row["ID"]);
            this.Value.value = data_row["Value"].ToString();
        }

        public MeasurementUnitModel() { }

        public override object getPK() => ID.value;
        public override ModelType getType() => ModelType.MODEL_BARCODE;
        public override bool matchPK(object pk) { return false; }
        public override void validateRelation() { }
    }
}
