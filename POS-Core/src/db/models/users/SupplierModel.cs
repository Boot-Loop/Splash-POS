using System;
using System.Data;

namespace Core.DB.Models
{
    public class SupplierModel : Model
    {
        public IntergerField    ID          { get; set; } = new IntergerField(  name: "ID"          );
        public TextField        FirstName   { get; set; } = new TextField(      name: "FirstName", is_required: true);
        public TextField        LastName    { get; set; } = new TextField(      name: "LastName"    );
        public IntergerField    CompanyID   { get; set; } = new IntergerField(  name: "Company_ID"  );
        public TextField        Address     { get; set; } = new TextField(      name: "Address"     );
        public EmailField       EMail       { get; set; } = new EmailField(     name: "EMail"       );
        public TextField        Telephone   { get; set; } = new TextField(      name: "Telephone"   );
        public TextField        Comments    { get; set; } = new TextField(      name: "Comments"    );

        public SupplierModel() { }

        public SupplierModel(DataRow data_row) {
            this.ID.value           = Convert.ToInt32(data_row["ID"]);
            this.FirstName.value    = data_row["FirstName"].ToString();
            this.LastName.value     = !data_row.IsNull("LastName")      ? data_row["LastName"].ToString()           : null;
            if (!data_row.IsNull("Company_ID")) this.CompanyID.value = Convert.ToInt32(data_row["Company_ID"]); else this.CompanyID.setToNull();
            this.Address.value      = !data_row.IsNull("Address")       ? data_row["Address"].ToString()            : null;
            this.EMail.value        = !data_row.IsNull("EMail")         ? data_row["EMail"].ToString()              : null;
            this.Telephone.value    = !data_row.IsNull("Telephone")     ? data_row["Telephone"].ToString()          : null;
            this.Comments.value     = !data_row.IsNull("Comments")      ? data_row["Comments"].ToString()           : null;
        }
    }
}