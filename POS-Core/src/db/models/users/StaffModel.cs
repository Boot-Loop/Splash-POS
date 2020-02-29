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
    public class StaffModel : Model
    {
        public IntergerField    ID              { get; set; } = new IntergerField(  name: "ID"          );
        public TextField        FirstName       { get; set; } = new TextField(      name: "FirstName",      is_required: true);
        public TextField        LastName        { get; set; } = new TextField(      name: "LastName"    );
        public TextField        UserName        { get; set; } = new TextField(      name: "UserName",       is_required: true);
        public TextField        Password        { get; set; } = new TextField(      name: "Password",       is_required: true);
        public EmailField       EMail           { get; set; } = new EmailField(     name: "EMail"       );
        public IntergerField    AccessLevel     { get; set; } = new IntergerField(  name: "AccessLevel",    is_required: true);

        public StaffModel(DataRow data_row) {
            this.ID.value           = Convert.ToInt32(data_row["ID"]);
            this.FirstName.value    = data_row["FirstName"].ToString();
            this.LastName.value     = !data_row.IsNull("LastName")  ? data_row["LastName"].ToString()   : null;
            this.UserName.value     = data_row["UserName"].ToString();
            this.Password.value     = data_row["Password"].ToString();
            this.EMail.value        = !data_row.IsNull("EMail")     ? data_row["EMail"].ToString()      : null;
            this.AccessLevel.value  = Convert.ToInt16(data_row["AccessLevel"]);
        }

        public StaffModel() { }

        public override object getPK()          => ID.value;
        public override ModelType getType()     => ModelType.MODEL_STAFF;
        public override bool matchPK(object pk) { return false; }
        public override void validateRelation() {}
    }
}
