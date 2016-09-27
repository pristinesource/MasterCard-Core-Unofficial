using System;
using System.Collections.Generic;
using System.Text;

namespace MasterCard.Core.Exceptions
{
	public class FieldError
	{
		internal string errorCode;

		internal string fieldName;

		internal string message;

		public virtual string ErrorCode
		{
			get
			{
				return this.errorCode;
			}
		}

		public virtual string FieldName
		{
			get
			{
				return this.fieldName;
			}
		}

		public virtual string Message
		{
			get
			{
				return this.message;
			}
		}

		internal FieldError(InvalidRequestException outerInstance, IDictionary<string, object> data)
		{
			this.errorCode = (string)data["code"];
			this.fieldName = (string)data["field"];
			this.message = (string)data["message"];
		}

		public override string ToString()
		{
			StringBuilder expr_05 = new StringBuilder();
			expr_05.Append("Field error: ").Append(this.FieldName).Append("\" ").Append(this.Message).Append("\" (").Append(this.ErrorCode).Append(")");
			return expr_05.ToString();
		}
	}
}
