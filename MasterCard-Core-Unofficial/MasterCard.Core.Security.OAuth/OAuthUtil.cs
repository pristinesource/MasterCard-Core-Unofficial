using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MasterCard_Core_Unofficial.MasterCard.Core;

namespace MasterCard.Core.Security.OAuth
{
	internal static class OAuthUtil
	{
		private static Random random = new Random();

		private static readonly string VALID_CHAR = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

		private static string GetNonce()
		{
			int num = 17;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < num; i++)
			{
				stringBuilder.Append(OAuthUtil.VALID_CHAR[OAuthUtil.random.Next(0, OAuthUtil.VALID_CHAR.Length - 1)]);
			}
			return stringBuilder.ToString();
		}

		private static string GetTimestamp()
		{
			long num = DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
			return (num / 10000000L).ToString();
		}

		public static string GetBaseString(string requestUrl, string httpMethod, SortedDictionary<string, string> oauthParameters)
		{
			return string.Concat(new string[]
			{
				Util.UriRfc3986(httpMethod.ToUpper()),
				"&",
				Util.UriRfc3986(Util.NormalizeUrl(requestUrl)),
				"&",
				Util.UriRfc3986(Util.NormalizeParameters(requestUrl, oauthParameters))
			});
		}

		public static string RsaSign(string baseString, AuthenticationInterface auth = null)
		{
			return (auth ?? ApiConfig.GetAuthentication()).SignMessage(baseString);
		}

		public static string GenerateSignature(string URL, string method, string body, string clientId, AsymmetricAlgorithm privateKey, AuthenticationInterface auth = null)
		{
			OAuthParameters oAuthParameters = new OAuthParameters();
			oAuthParameters.setOAuthConsumerKey(clientId);
			oAuthParameters.setOAuthNonce(OAuthUtil.GetNonce());
			oAuthParameters.setOAuthTimestamp(OAuthUtil.GetTimestamp());
			oAuthParameters.setOAuthSignatureMethod("RSA-SHA1");
			oAuthParameters.setOAuthVersion("1.0");
			if (!string.IsNullOrEmpty(body))
			{
				string oAuthBodyHash = Util.Base64Encode(Util.Sha1Encode(body));
				oAuthParameters.setOAuthBodyHash(oAuthBodyHash);
			}
			string oAuthSignature = OAuthUtil.RsaSign(OAuthUtil.GetBaseString(URL, method, oAuthParameters.getBaseParameters()), auth);
			oAuthParameters.setOAuthSignature(oAuthSignature);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> current in oAuthParameters.getBaseParameters())
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append(OAuthParameters.OAUTH_KEY).Append(" ");
				}
				else
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(current.Key).Append("=\"").Append(Util.UriRfc3986(current.Value)).Append("\"");
			}
			return stringBuilder.ToString();
		}
	}
}
