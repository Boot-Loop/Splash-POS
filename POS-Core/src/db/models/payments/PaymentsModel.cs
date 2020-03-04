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
    public class PaymentModel : Model
    {
        public IntergerField ID { get; set; } = new IntergerField(name: "ID");
        public IntergerField PaymentMethodID { get; set; } = new IntergerField(name: "PaymentMethodID");
        public FloatField Amount { get; set; } = new FloatField(name: "Amount", is_required: true);
        public DateTimeField TransactionTime { get; set; } = new DateTimeField(name: "TransactionTime");

        public PaymentModel(DataRow data_row) {
            this.ID.value = Convert.ToInt32(data_row["ID"]);
            if (!data_row.IsNull("PaymentMethod_ID")) this.PaymentMethodID.value = Convert.ToInt32(data_row["PaymentMethod_ID"]); else this.PaymentMethodID.setToNull();
            this.Amount.value = Convert.ToDouble(data_row["Amount"]);
            if (!data_row.IsNull("TransactionTime")) this.TransactionTime.value = Convert.ToDateTime(data_row["TransactionTime"]); else this.TransactionTime.setToNull();
        }

        public PaymentModel() { }

        public override object getPK() => null;
        public override ModelType getType() => ModelType.MODEL_PAYMENT;
        public override bool matchPK(object pk) { return false; }
        public override void validateRelation() { }
    }
}
