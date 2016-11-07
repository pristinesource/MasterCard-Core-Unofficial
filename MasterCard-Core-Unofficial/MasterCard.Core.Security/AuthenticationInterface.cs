using RestSharp;
using System;
using MasterCard_Core_Unofficial.MasterCard.Core;

namespace MasterCard.Core.Security
{
	public interface AuthenticationInterface
	{
		void SignRequest(Uri uri, IRestRequest request);

		string SignMessage(string message);
	}
}
