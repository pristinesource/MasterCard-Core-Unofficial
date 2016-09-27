using System;
using System.Collections.Generic;

namespace MasterCard.Core.Model
{
	public abstract class BaseObject : RequestMap
	{
		protected BaseObject()
		{
		}

		protected BaseObject(RequestMap bm) : base(bm)
		{
		}

		protected BaseObject(IDictionary<string, object> map) : base(map)
		{
		}

		protected abstract OperationConfig GetOperationConfig(string operationUUID);

		protected abstract OperationMetadata GetOperationMetadata();

		protected static ResourceList<T> ExecuteForList<T>(string operationUUID, T inputObject) where T : BaseObject
		{
			return new ResourceList<T>(BaseObject.Execute<T>(operationUUID, inputObject));
		}

		protected static T Execute<T>(string operationUUID, T inputObject) where T : BaseObject
		{
			IDictionary<string, object> dictionary = new ApiController(inputObject.GetOperationMetadata().Version).Execute(inputObject.GetOperationConfig(operationUUID), inputObject.GetOperationMetadata(), inputObject);
			if (dictionary != null)
			{
				inputObject.Clear();
				inputObject.AddAll(dictionary);
			}
			else
			{
				inputObject = (T)((object)Activator.CreateInstance(inputObject.GetType()));
				inputObject.AddAll(dictionary);
			}
			return inputObject;
		}
	}
}
