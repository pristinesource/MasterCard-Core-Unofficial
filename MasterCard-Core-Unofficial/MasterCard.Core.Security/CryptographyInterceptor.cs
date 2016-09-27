using System;
using System.Collections.Generic;

namespace MasterCard.Core.Security
{
	public interface CryptographyInterceptor
	{
		string GetTriggeringPath();

		IDictionary<string, object> Encrypt(IDictionary<string, object> map);

		IDictionary<string, object> Decrypt(IDictionary<string, object> map);
	}
}
