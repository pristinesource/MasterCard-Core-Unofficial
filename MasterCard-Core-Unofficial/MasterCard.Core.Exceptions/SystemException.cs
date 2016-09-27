using System;
using System.Collections.Generic;

namespace MasterCard.Core.Exceptions
{
	public class SystemException : ApiException
	{
		public SystemException()
		{
		}

		public SystemException(string message) : base(message)
		{
		}

		public SystemException(string message, string description) : base(message)
		{
			this.description = description;
		}

		public SystemException(string message, Exception cause) : base(message, cause)
		{
		}

		public SystemException(Exception cause) : base(cause)
		{
		}

		public SystemException(int status, IDictionary<string, object> errorData) : base(status, errorData)
		{
		}
	}
}
