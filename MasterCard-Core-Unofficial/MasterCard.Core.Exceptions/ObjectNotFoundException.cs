using System;
using System.Collections.Generic;

namespace MasterCard.Core.Exceptions
{
	public class ObjectNotFoundException : ApiException
	{
		public ObjectNotFoundException()
		{
		}

		public ObjectNotFoundException(string message) : base(message)
		{
		}

		public ObjectNotFoundException(string message, string description) : base(message)
		{
			this.description = description;
		}

		public ObjectNotFoundException(string message, Exception cause) : base(message, cause)
		{
		}

		public ObjectNotFoundException(Exception cause) : base(cause)
		{
		}

		public ObjectNotFoundException(int status, IDictionary<string, object> errorData) : base(status, errorData)
		{
		}
	}
}
