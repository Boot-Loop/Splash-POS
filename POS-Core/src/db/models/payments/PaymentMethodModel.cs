using System;
using System.Data;

namespace Core.DB.Models
{
    public class PaymentMethodModel : Model
    {
        public IntergerField ID { get; set; } = new IntergerField(  name: "ID");
        public TextField Type   { get; set; } = new TextField(      name: "Type", is_required: true);

        public PaymentMethodModel() { }

        public PaymentMethodModel(DataRow data_row) {
            this.ID.value   = Convert.ToInt32(data_row["ID"]);
            this.Type.value = data_row["Type"].ToString();
        }
    }
}
