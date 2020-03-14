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
        public IntergerField ID                 { get; set; } = new IntergerField(  name: "ID");
        public IntergerField PaymentMethodID    { get; set; } = new IntergerField(  name: "PaymentMethodID",    is_required: true);
        public FloatField SubTotal              { get; set; } = new FloatField(     name: "SubTotal",           is_required: true);
        public FloatField Discount              { get; set; } = new FloatField(     name: "Discount",           is_required: true);
        public FloatField Total                 { get; set; } = new FloatField(     name: "Total",              is_required: true);
        public FloatField Paid                  { get; set; } = new FloatField(     name: "Paid",               is_required: true);
        public FloatField Balance               { get; set; } = new FloatField(     name: "Balance",            is_required: true);
        public DateTimeField TransactionTime    { get; set; } = new DateTimeField(  name: "TransactionTime",    is_required: true);

        public PaymentModel() { }

        public PaymentModel(DataRow data_row) {
            this.ID.value               = Convert.ToInt32(data_row["ID"]);
            this.PaymentMethodID.value  = Convert.ToInt32(data_row["PaymentMethod_ID"]);
            this.SubTotal.value         = Convert.ToDouble(data_row["SubTotal"]);
            this.Discount.value         = Convert.ToDouble(data_row["Discount"]);
            this.Total.value            = Convert.ToDouble(data_row["Total"]);
            this.Paid.value             = Convert.ToDouble(data_row["Paid"]);
            this.Balance.value          = Convert.ToDouble(data_row["Balance"]);
            this.TransactionTime.value  = Convert.ToDateTime(data_row["TransactionTime"]);
        }

    }
}
