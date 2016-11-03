using System;
using System.Collections.Generic;
using System.Text;

namespace MasterCard.Core.Exceptions
{
	public class ApiException : Exception
	{
		protected string source;

		protected string reasonCode;

		protected string recoverable;

		protected string description;

    protected int status;

		private IDictionary<string, object> errorData;

		public virtual IDictionary<string, object> ErrorData
		{
			get
			{
				return this.errorData;
			}
		}

		public override string Source
		{
			get
			{
				return this.source;
			}
		}

		public virtual string ReasonCode
		{
			get
			{
				return this.reasonCode;
			}
		}

		public virtual string Recoverable
		{
			get
			{
				return this.recoverable;
			}
		}

		public override string Message
		{
			get
			{
				if (this.description == null)
				{
					return base.Message;
				}
				return this.description;
			}
		}

    public virtual int StatusCode {
      get {
        return this.status;
      }
    }

    public ApiException()
		{
		}

		public ApiException(string message) : base(message)
		{
		}

		public ApiException(string message, string description) : base(message)
		{
			this.description = description;
		}

		public ApiException(string message, Exception cause) : base(message, cause)
		{
		}

		public ApiException(Exception cause) : base(cause.Message, cause)
		{
		}

		public ApiException(int status, IDictionary<string, object> errorData)
		{
      this.status = status;
			this.errorData = errorData;
			if (errorData.ContainsKey("Errors"))
			{
				IDictionary<string, object> dictionary = (IDictionary<string, object>)errorData["Errors"];
				if (dictionary.ContainsKey("Error"))
				{
					IDictionary<string, object> dictionary2;
					if (dictionary["Error"].GetType() == typeof(List<Dictionary<string, object>>))
					{
						dictionary2 = ((List<Dictionary<string, object>>)dictionary["Error"])[0];
					}
					else
					{
						dictionary2 = (Dictionary<string, object>)dictionary["Error"];
					}
					this.source = dictionary2["Source"].ToString();
					this.reasonCode = dictionary2["ReasonCode"].ToString();
					this.description = dictionary2["Description"].ToString();
					this.recoverable = dictionary2["Recoverable"].ToString();
				}
			}
		}

		public virtual string describe()
		{
			return new StringBuilder().Append(base.GetType().Name).Append(": \"").Append(this.Message).Append("\" (Source: ").Append(this.Source).Append(", ReasonCode: ").Append(this.ReasonCode).Append(", Recoverable: ").Append(this.Recoverable).Append(")").ToString();
		}
	}
}
