using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.DB.Models;

namespace Core.DB.API
{
	public enum ModelApiMode
	{
		MODE_CREATE,
		MODE_UPDATE,
		MODE_READ,
		MODE_DELETE,
	}

	public abstract class BaseModelAPI<T> where T: Model
	{

		protected T _model; 
		public T model { get { return _model; } }

		private ModelApiMode _api_mode;
		public ModelApiMode api_mode { get { return _api_mode; } }

		public BaseModelAPI( object pk, ModelApiMode mode ) {
			_api_mode = mode;
			_setModel(pk, Model.toModelType(typeof(T)));
		}

		public abstract void _setModel(object pk, ModelType model_type);
		

		public abstract void update(); // update the db using the model
	}
}
