﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DB.Models
{
    public class ProductModel : Model
    {
        public IntergerField    ID                  { get; set; } = new IntergerField(name: "ID");
        public TextField        Name                { get; set; } = new TextField(name: "Name", is_required: true);
        public IntergerField    ProductGroupID      { get; set; } = new IntergerField(name: "ProductGroup_ID");
        public IntergerField    BrandID             { get; set; } = new IntergerField(name: "Brand_ID");
        public IntergerField    MeasurementUnitID   { get; set; } = new IntergerField(name: "MeasurementUnit_ID");
        public IntergerField    Code                { get; set; } = new IntergerField(name: "Code", is_required: true);
        public TextField        Description         { get; set; } = new TextField(name: "Description");
        public IntergerField    PLU                 { get; set; } = new IntergerField(name: "PLU");
        public TextField        Image               { get; set; } = new TextField(name: "Image");
        public TextField        Color               { get; set; } = new TextField(name: "Color");
        public FloatField       Price               { get; set; } = new FloatField(name: "Price", is_required: true);
        public BoolField        IsService           { get; set; } = new BoolField(name: "IsService", is_required: true);
        public DateTimeField    DateCreated         { get; set; } = new DateTimeField(name: "DateCreated", is_required: true);
        public DateTimeField    DateUpdated         { get; set; } = new DateTimeField(name: "DateUpdated", is_required: true);



        public ProductModel(DataRow data_row)
        {
            this.ID.value = Convert.ToInt32(data_row["ID"]);
            this.Name.value = data_row["Name"].ToString();
            if (!data_row.IsNull("ProductGroup_ID")) this.ProductGroupID.value = Convert.ToInt32(data_row["ProductGroup_ID"]); else this.ProductGroupID.setToNull();
            if (!data_row.IsNull("Brand_ID")) this.BrandID.value = Convert.ToInt32(data_row["Brand_ID"]); else this.BrandID.setToNull();
            if (!data_row.IsNull("MeasurementUnit_ID")) this.MeasurementUnitID.value = Convert.ToInt32(data_row["MeasurementUnit_ID"]); else this.MeasurementUnitID.setToNull();
            this.Code.value = Convert.ToInt32(data_row["Code"]);
            this.Description.value = !data_row.IsNull("Description") ? data_row["Description"].ToString() : null;
            if (!data_row.IsNull("PLU")) this.PLU.value = Convert.ToInt32(data_row["PLU"]); else this.PLU.setToNull();
            this.Image.value = !data_row.IsNull("Image") ? data_row["Image"].ToString() : null;
            this.Color.value = !data_row.IsNull("Color") ? data_row["Color"].ToString() : null;
            this.Price.value = Convert.ToDouble(data_row["Price"]);
            this.IsService.value = Convert.ToBoolean(data_row["IsService"]);
            this.DateCreated.value = Convert.ToDateTime(data_row["DateCreated"]);
            this.DateUpdated.value = Convert.ToDateTime(data_row["DateUpdated"]);
        }

        public ProductModel() { }

        public override object getPK() => ID.value;
        public override ModelType getType() => ModelType.MODEL_PRODUCT;
        public override bool matchPK(object pk) { return false; }
        public override void validateRelation() { }
    }
}
