using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.DB.Models;

namespace Core.DB.API
{
	public class ModelAPI<T> : BaseModelAPI<T> where T: Model
	{
		public ModelAPI(object pk, ModelApiMode mode) : base(pk, mode) {}

		public override void _setModel(object pk, ModelType model_type)
		{
			// need pk
			if (api_mode != ModelApiMode.MODE_CREATE) if (pk is null) throw new ArgumentNullException("required argument pk");

			if (api_mode == ModelApiMode.MODE_CREATE) {
				// if (!(pk is null)) logWarning("for model creation mode pk is not-required");
				_model = Model.newModel(model_type) as T; // because new T(); wont work
			}

			throw new NotImplementedException("TODO: set _model from database using model_type and pk");
		}

		public override void update() {
			throw new NotImplementedException("TODO: update db with the model");
		}
	}
}
