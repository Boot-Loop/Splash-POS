using System;
using System.Data;

namespace Core.DB.Models
{
    public class MeasurementUnitModel : Model
    {
        public IntergerField ID { get; set; } = new IntergerField(name: "ID");
        public TextField Value { get; set; } = new TextField(name: "Value", is_required: true);

        public MeasurementUnitModel() { }

        public MeasurementUnitModel(DataRow data_row) {
            this.ID.value = Convert.ToInt32(data_row["ID"]);
            this.Value.value = data_row["Value"].ToString();
        }
    }
}
