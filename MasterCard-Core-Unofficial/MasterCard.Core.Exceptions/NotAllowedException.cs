using System;
using System.Collections.Generic;

namespace MasterCard.Core.Exceptions
{
	public class NotAllowedException : ApiException
	{
		public NotAllowedException()
		{
		}

		public NotAllowedException(string message) : base(message)
		{
		}

		public NotAllowedException(string message, string description) : base(message)
		{
			this.description = description;
		}

		public NotAllowedException(string message, Exception cause) : base(message, cause)
		{
		}

		public NotAllowedException(Exception cause) : base(cause)
		{
		}

		public NotAllowedException(int status, IDictionary<string, object> errorData) : base(status, errorData)
		{
		}
	}
}
