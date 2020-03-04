using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DB.Models
{
	public enum ModelType
	{
		MODEL_STAFF,
		MODEL_SUPPLIER,
		MODEL_PRODUCT,
		MODEL_BARCODE,
		MODEL_BRAND,
		MODEL_STOCK,
		MODEL_SALE_PRODUCT,
		MODEL_SALE,
		MODEL_PAYMENT,
	}

	[Serializable]
	public abstract class Model
	{
		/* your models must inherit this class and must be seiralizable & call the base constructor */

		public abstract bool matchPK(object pk);	// returns true if the pk mathch with the pk of the model
		public abstract object getPK();				// returns the pk
		public abstract ModelType getType();		// returns the type of the model
		public abstract void validateRelation();    // validate relations fields

		public Model() {
			toModelType( this.GetType() ); // throws an error if not implimented for this model
		}

		// ensure every model type has been impliemented
		public static void initialize() {
			foreach ( ModelType type in Enum.GetValues(typeof(ModelType)).Cast<ModelType>()) {
				newModel(type);
			}
		}


		public static ModelType toModelType(Type type)
		{
			if (type == typeof(StaffModel))		return ModelType.MODEL_STAFF;
			if (type == typeof(SupplierModel))	return ModelType.MODEL_SUPPLIER;
			if (type == typeof(ProductModel))	return ModelType.MODEL_PRODUCT;
			if (type == typeof(BarcodeModel))	return ModelType.MODEL_BARCODE;
			if (type == typeof(BrandModel))		return ModelType.MODEL_BRAND;
			if (type == typeof(StockModel))		return ModelType.MODEL_STOCK;
			if (type == typeof(SaleProductModel)) return ModelType.MODEL_SALE_PRODUCT;
			if (type == typeof(SaleModel))		return ModelType.MODEL_SALE;
			if (type == typeof(PaymentModel))	return ModelType.MODEL_PAYMENT;
			throw new NotImplementedException("TODO:ModelType");
		}

		public static Model newModel(ModelType model_type)
		{
			switch (model_type)
			{
				// case ModelType.MODEL_CLIENT: return new ClientModel();
				// case ModelType.MODEL_SUPPLIER: return new SupplierModel();
				default: throw new NotImplementedException("TODO:Model");
			}
		}

	}
}
