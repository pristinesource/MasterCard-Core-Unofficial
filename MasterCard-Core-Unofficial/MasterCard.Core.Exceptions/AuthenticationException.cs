using System;
using System.Collections.Generic;

namespace MasterCard.Core.Exceptions
{
	public class AuthenticationException : ApiException
	{
		public AuthenticationException()
		{
		}

		public AuthenticationException(string message) : base(message)
		{
		}

		public AuthenticationException(string message, string description) : base(message)
		{
			this.description = description;
		}

		public AuthenticationException(string message, Exception cause) : base(message, cause)
		{
		}

		public AuthenticationException(Exception cause) : base(cause)
		{
		}

		public AuthenticationException(int status, IDictionary<string, object> errorData) : base(status, errorData)
		{
		}
	}
}
