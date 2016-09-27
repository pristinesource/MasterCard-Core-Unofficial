using System;
using System.Collections.Generic;
using System.Reflection;

namespace MasterCard.Core.Model
{
	public class ResourceList<T> : List<T> where T : BaseObject
	{
		public ResourceList(IDictionary<string, object> map)
		{
			if (map.ContainsKey("list") && typeof(List<Dictionary<string, object>>) == map["list"].GetType())
			{
				List<Dictionary<string, object>> arg_55_0 = (List<Dictionary<string, object>>)map["list"];
				Type type = base.GetType().GetGenericArguments()[0];
				foreach (Dictionary<string, object> current in arg_55_0)
				{
					T t = (T)((object)Activator.CreateInstance(type));
					t.AddAll(current);
					base.Add(t);
				}
			}
		}
	}
}
