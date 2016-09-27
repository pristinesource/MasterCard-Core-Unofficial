using System;
using System.Collections.Generic;
using System.Text;

namespace MasterCard.Core.Exceptions
{
	public class InvalidRequestException : ApiException
	{
		protected IList<FieldError> fieldErrors = new List<FieldError>();

		public virtual IList<FieldError> FieldErrors
		{
			get
			{
				return this.fieldErrors;
			}
		}

		public InvalidRequestException()
		{
		}

		public InvalidRequestException(string message) : base(message)
		{
		}

		public InvalidRequestException(string message, string description) : base(message)
		{
			this.description = description;
		}

		public InvalidRequestException(string message, Exception cause) : base(message, cause)
		{
		}

		public InvalidRequestException(Exception cause) : base(cause)
		{
		}

		public InvalidRequestException(int status, IDictionary<string, object> errorData) : base(status, errorData)
		{
		}

		public virtual bool hasFieldErrors()
		{
			return this.fieldErrors.Count > 0;
		}

		public override string describe()
		{
			StringBuilder stringBuilder = new StringBuilder(base.describe());
			foreach (FieldError current in this.FieldErrors)
			{
				stringBuilder.Append("\n").Append(current.ToString());
			}
			stringBuilder.Append("\n");
			return stringBuilder.ToString();
		}
	}
}
