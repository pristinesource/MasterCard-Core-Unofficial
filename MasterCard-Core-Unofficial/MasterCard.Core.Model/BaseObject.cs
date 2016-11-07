using System;
using System.Collections.Generic;
using MasterCard_Core_Unofficial.MasterCard.Core;

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

		protected static ResourceList<T> ExecuteForList<T>(string operationUUID, T inputObject, IndividualApiConfig apiConfig = null) where T : BaseObject
		{
			return new ResourceList<T>(BaseObject.Execute<T>(operationUUID, inputObject, apiConfig));
		}

		protected static T Execute<T>(string operationUUID, T inputObject, IndividualApiConfig apiConfig = null) where T : BaseObject
		{
			IDictionary<string, object> dictionary = new ApiController(inputObject.GetOperationMetadata().Version, apiConfig).Execute(inputObject.GetOperationConfig(operationUUID), inputObject.GetOperationMetadata(), inputObject);
            dictionary = dictionary ?? new Dictionary<string, object>();
			if (inputObject != null)
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
