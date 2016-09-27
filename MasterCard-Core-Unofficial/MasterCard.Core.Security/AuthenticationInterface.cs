using RestSharp;
using System;

namespace MasterCard.Core.Security
{
	public interface AuthenticationInterface
	{
		void SignRequest(Uri uri, IRestRequest request);

		string SignMessage(string message);
	}
}
