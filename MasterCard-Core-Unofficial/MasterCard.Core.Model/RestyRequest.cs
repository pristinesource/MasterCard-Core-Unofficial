using MasterCard.Core.Security;
using RestSharp;
using System;

namespace MasterCard.Core.Model
{
	internal class RestyRequest : RestRequest
	{
		public Uri AbsoluteUrl
		{
			get;
			set;
		}

		public Uri BaseUrl
		{
			get;
			set;
		}

		public CryptographyInterceptor interceptor
		{
			get;
			set;
		}

		public RestyRequest(Uri url, Method method) : base(url, method)
		{
		}
	}
}
