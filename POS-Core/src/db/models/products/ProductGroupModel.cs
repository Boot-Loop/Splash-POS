using System;
using System.Data;

namespace Core.DB.Models
{
    public class ProductGroupModel : Model
    {
        public IntergerField ID             { get; set; } = new IntergerField(  name: "ID");
        public TextField Name               { get; set; } = new TextField(      name: "Name", is_required: true);
        public IntergerField ParentGroup_ID { get; set; } = new IntergerField(  name: "ParentGroup_ID");
        public TextField Color              { get; set; } = new TextField(      name: "Color"); 

        public ProductGroupModel() { }

        public ProductGroupModel(DataRow data_row) {
            this.ID.value       = Convert.ToInt32(data_row["ID"]);
            this.Name.value     = Convert.ToString(data_row["Name"]);
            if (!data_row.IsNull("ParentGroup_ID")) this.ParentGroup_ID.value = Convert.ToInt32(data_row["ParentGroup_ID"]); else this.ParentGroup_ID.setToNull();
            this.Color.value    = !data_row.IsNull("Color") ? data_row["Color"].ToString() : null;
        }
    }
}
