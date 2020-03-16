using System;
using System.Data;

namespace Core.DB.Models
{
    public class SaleDetailModel : Model {
        public IntergerField SaleID             { get; set; } = new IntergerField(  name: "SaleID");
        public FloatField SubTotal              { get; set; } = new FloatField(     name: "SubTotal");
        public FloatField Discount              { get; set; } = new FloatField(     name: "Discount");
        public FloatField Total                 { get; set; } = new FloatField(     name: "Total");
        public DateTimeField TransactionTime    { get; set; } = new DateTimeField(  name: "TransactionTime");

        public SaleDetailModel() { }

        public SaleDetailModel(DataRow data_row) {
            this.SaleID.value           = Convert.ToInt32(data_row["SaleID"]);
            this.SubTotal.value         = Convert.ToDouble(data_row["SubTotal"]);
            this.Discount.value         = Convert.ToDouble(data_row["Discount"]);
            this.Total.value            = Convert.ToDouble(data_row["Total"]);
            this.TransactionTime.value  = Convert.ToDateTime(data_row["TransactionTime"]);
        }
    }
}