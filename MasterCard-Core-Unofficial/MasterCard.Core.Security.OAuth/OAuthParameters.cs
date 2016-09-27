using System;
using System.Collections.Generic;

namespace MasterCard.Core.Security.OAuth
{
	internal class OAuthParameters
	{
		public static readonly string OAUTH_BODY_HASH_KEY = "oauth_body_hash";

		public static readonly string OAUTH_CALLBACK_KEY = "oauth_callback";

		public static readonly string OAUTH_CONSUMER_KEY = "oauth_consumer_key";

		public static readonly string OAUTH_CONSUMER_SECRET = "oauth_consumer_secret";

		public static readonly string OAUTH_NONCE_KEY = "oauth_nonce";

		public static readonly string OAUTH_KEY = "OAuth";

		public static readonly string OAUTH_SIGNATURE_KEY = "oauth_signature";

		public static readonly string OAUTH_SIGNATURE_METHOD_KEY = "oauth_signature_method";

		public static readonly string OAUTH_TIMESTAMP_KEY = "oauth_timestamp";

		public static readonly string OAUTH_TOKEN_KEY = "oauth_token";

		public static readonly string OAUTH_TOKEN_SECRET_KEY = "oauth_token_secret";

		public static readonly string OAUTH_VERIFIER_KEY = "oauth_verifier";

		public static readonly string OAUTH_VERSION = "oauth_version";

		public static readonly string REALM_KEY = "realm";

		public static readonly string XOAUTH_REQUESTOR_ID_KEY = "xoauth_requestor_id";

		protected SortedDictionary<string, string> baseParameters;

		internal OAuthParameters()
		{
			this.baseParameters = new SortedDictionary<string, string>();
		}

		private void Put(string key, string value, SortedDictionary<string, string> dictionary)
		{
			dictionary.Add(key, value);
		}

		public void setOAuthConsumerKey(string consumerKey)
		{
			this.Put(OAuthParameters.OAUTH_CONSUMER_KEY, consumerKey, this.baseParameters);
		}

		public void setOAuthNonce(string oauthNonce)
		{
			this.Put(OAuthParameters.OAUTH_NONCE_KEY, oauthNonce, this.baseParameters);
		}

		public void setOAuthTimestamp(string timestamp)
		{
			this.Put(OAuthParameters.OAUTH_TIMESTAMP_KEY, timestamp, this.baseParameters);
		}

		public void setOAuthSignatureMethod(string signatureMethod)
		{
			this.Put(OAuthParameters.OAUTH_SIGNATURE_METHOD_KEY, signatureMethod, this.baseParameters);
		}

		public void setOAuthSignature(string signature)
		{
			this.Put(OAuthParameters.OAUTH_SIGNATURE_KEY, signature, this.baseParameters);
		}

		public void setOAuthBodyHash(string bodyHash)
		{
			this.Put(OAuthParameters.OAUTH_BODY_HASH_KEY, bodyHash, this.baseParameters);
		}

		public void setOAuthVersion(string version)
		{
			this.Put(OAuthParameters.OAUTH_VERSION, version, this.baseParameters);
		}

		public SortedDictionary<string, string> getBaseParameters()
		{
			return new SortedDictionary<string, string>(this.baseParameters);
		}
	}
}
