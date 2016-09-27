using MasterCard.Core.Security;
using System;
using System.Collections.Generic;

namespace MasterCard.Core
{
	public static class ApiConfig
	{
		private static bool SANDBOX = true;

		private static bool DEBUG = false;

		private static AuthenticationInterface authentication;

		private static Dictionary<string, object> cryptographyMap = new Dictionary<string, object>();

		private static string API_BASE_LIVE_URL = "https://api.mastercard.com";

		private static string API_BASE_SANDBOX_URL = "https://sandbox.api.mastercard.com";

		public static string GetLiveUrl()
		{
			return ApiConfig.API_BASE_LIVE_URL;
		}

		public static string GetSandboxUrl()
		{
			return ApiConfig.API_BASE_SANDBOX_URL;
		}

		public static void SetDebug(bool debug)
		{
			ApiConfig.DEBUG = debug;
		}

		public static bool IsDebug()
		{
			return ApiConfig.DEBUG;
		}

		public static void SetSandbox(bool sandbox)
		{
			ApiConfig.SANDBOX = sandbox;
		}

		public static bool IsSandbox()
		{
			return ApiConfig.SANDBOX;
		}

		public static bool IsProduction()
		{
			return !ApiConfig.SANDBOX;
		}

		public static AuthenticationInterface GetAuthentication()
		{
			return ApiConfig.authentication;
		}

		public static void SetAuthentication(AuthenticationInterface authentication)
		{
			ApiConfig.authentication = authentication;
		}

		public static void AddCryptographyInterceptor(CryptographyInterceptor cryptographyInterceptor)
		{
			if (!ApiConfig.cryptographyMap.ContainsKey(cryptographyInterceptor.GetTriggeringPath()))
			{
				ApiConfig.cryptographyMap.Add(cryptographyInterceptor.GetTriggeringPath(), cryptographyInterceptor);
			}
		}

		public static CryptographyInterceptor GetCryptographyInterceptor(string basePath)
		{
			foreach (KeyValuePair<string, object> current in ApiConfig.cryptographyMap)
			{
				if (current.Key.Contains(basePath) || basePath.Contains(current.Key))
				{
					return (CryptographyInterceptor)current.Value;
				}
			}
			return null;
		}

        public static void setLocalhost() {
            API_BASE_SANDBOX_URL = "http://localhost:8080";
            API_BASE_LIVE_URL = "http://localhost:8080";
        }

        public static void unsetLocalhost() {
            API_BASE_SANDBOX_URL = "https://sandbox.api.mastercard.com";
            API_BASE_LIVE_URL = "https://api.mastercard.com";
        }

        public static void setAPIBaseCustomHosts(Uri SandboxUrl = null, Uri LiveUrl = null) {
            API_BASE_SANDBOX_URL = SandboxUrl?.ToString() ?? API_BASE_SANDBOX_URL;
            API_BASE_LIVE_URL = LiveUrl?.ToString() ?? API_BASE_LIVE_URL;
        }

        public static void unsetAPIBaseCustomHosts() {
            unsetLocalhost();
        }
    }
}
