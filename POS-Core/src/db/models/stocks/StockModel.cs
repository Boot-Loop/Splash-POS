﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DB.Models
{
    public class StockModel : Model
    {
        public IntergerField    ID              { get; set; } = new IntergerField(name: "ID");
        public IntergerField    ProductID       { get; set; } = new IntergerField(name: "Product_ID", is_required: true);
        public IntergerField    WarehouseID     { get; set; } = new IntergerField(name: "Warehouse_ID");
        public IntergerField    SupplierID      { get; set; } = new IntergerField(name: "Supplier_ID");
        public IntergerField    Quantity        { get; set; } = new IntergerField(name: "Quantity", is_required: true);
        public FloatField       UnitPrice       { get; set; } = new FloatField(name: "UnitPrice", is_required: true);
        public DateTimeField    Date            { get; set; } = new DateTimeField(name: "Date");
        public TextField        ProductName     { get; set; } = new TextField(name: "ProductName");
        public TextField        WarehouseName   { get; set; } = new TextField(name: "WarehouseName");
        public TextField        SupplierName    { get; set; } = new TextField(name: "SupplierName");



        public StockModel(DataRow data_row) {
            this.ID.value = Convert.ToInt32(data_row["ID"]);
            this.ProductID.value = Convert.ToInt32(data_row["Product_ID"]);
            if (!data_row.IsNull("Warehouse_ID")) this.WarehouseID.value = Convert.ToInt32(data_row["Warehouse_ID"]); else this.WarehouseID.setToNull();
            if (!data_row.IsNull("Supplier_ID")) this.SupplierID.value = Convert.ToInt32(data_row["Supplier_ID"]); else this.SupplierID.setToNull();
            this.Quantity.value = Convert.ToInt32(data_row["Quantity"]);
            this.UnitPrice.value = Convert.ToDouble(data_row["UnitPrice"]);
            if (!data_row.IsNull("Date")) this.Date.value = Convert.ToDateTime(data_row["Date"]); else this.Date.setToNull();
            this.ProductName.value = !data_row.IsNull("ProductName") ? data_row["ProductName"].ToString() : null;
            this.WarehouseName.value = !data_row.IsNull("WarehouseName") ? data_row["WarehouseName"].ToString() : null;
            this.SupplierName.value = !data_row.IsNull("SupplierFirstName") ? data_row["SupplierFirstName"].ToString() : null;
        }

        public StockModel() { }

        public override object getPK() => ID.value;
        public override ModelType getType() => ModelType.MODEL_STOCK;
        public override bool matchPK(object pk) { return false; }
        public override void validateRelation() { }
    }
}
