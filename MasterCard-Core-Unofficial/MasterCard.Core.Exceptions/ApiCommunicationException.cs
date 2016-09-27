using System;

namespace MasterCard.Core.Exceptions
{
	public class ApiCommunicationException : ApiException
	{
		public ApiCommunicationException()
		{
		}

		public ApiCommunicationException(string message) : base(message)
		{
		}

		public ApiCommunicationException(string message, Exception cause) : base(message, cause)
		{
		}

		public ApiCommunicationException(Exception cause) : base(cause)
		{
		}
	}
}
