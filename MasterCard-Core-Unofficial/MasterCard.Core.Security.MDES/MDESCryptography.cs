using MasterCard.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MasterCard.Core.Security.MDES
{
	public class MDESCryptography : CryptographyInterceptor
	{
		private List<string> fieldsToHide = new List<string>
		{
			"publicKeyFingerprint",
			"oaepHashingAlgorithm",
			"iv",
			"encryptedData",
			"encryptedKey"
		};

		private string triggeringPath = "/mdes/tokenization/";

		private RSA publicKey;

		private string publicKeyFingerPrint;

		private RSA privateKey;

		public MDESCryptography(string publicKeyLocation, string privateKeyLocation)
		{
			X509Certificate2 x509Certificate = new X509Certificate2(publicKeyLocation);
			this.publicKey = x509Certificate.GetRSAPublicKey();
			this.publicKeyFingerPrint = x509Certificate.Thumbprint;
			string text = File.ReadAllText(privateKeyLocation);
			this.privateKey = CryptUtil.GetRSAFromPrivateKeyString(text);
		}

		public string GetTriggeringPath()
		{
			return this.triggeringPath;
		}

		public IDictionary<string, object> Encrypt(IDictionary<string, object> map)
		{
			if (map.ContainsKey("cardInfo"))
			{
				string text = JsonConvert.SerializeObject((IDictionary<string, object>)map["cardInfo"]);
				text = CryptUtil.SanitizeJson(text);
				Tuple<byte[], byte[], byte[]> expr_3D = CryptUtil.EncryptAES(Encoding.UTF8.GetBytes(text));
				byte[] item = expr_3D.Item1;
				byte[] item2 = expr_3D.Item2;
				byte[] arg_57_0 = expr_3D.Item3;
				string value = CryptUtil.HexEncode(item);
				string value2 = CryptUtil.HexEncode(arg_57_0);
				string value3 = CryptUtil.HexEncode(CryptUtil.EncrytptRSA(item2, this.publicKey));
				string value4 = this.publicKeyFingerPrint;
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("publicKeyFingerprint", value4);
				dictionary.Add("encryptedKey", value3);
				dictionary.Add("oaepHashingAlgorithm", "SHA256");
				dictionary.Add("iv", value);
				dictionary.Add("encryptedData", value2);
				map.Remove("cardInfo");
				map.Add("cardInfo", dictionary);
			}
			return map;
		}

		public IDictionary<string, object> Decrypt(IDictionary<string, object> map)
		{
			if (map.ContainsKey("token"))
			{
				IDictionary<string, object> dictionary = (IDictionary<string, object>)map["token"];
				if (dictionary.ContainsKey("") && dictionary.ContainsKey(""))
				{
					byte[] encryptionKey = CryptUtil.DecryptRSA(CryptUtil.HexDecode((string)dictionary["encryptedKey"]), this.privateKey);
					byte[] arg_8F_0 = CryptUtil.HexDecode((string)dictionary["iv"]);
					byte[] encryptedData = CryptUtil.HexDecode((string)dictionary["encryptedData"]);
					byte[] bytes = CryptUtil.DecryptAES(arg_8F_0, encryptionKey, encryptedData);
					string @string = Encoding.UTF8.GetString(bytes);
					foreach (string current in this.fieldsToHide)
					{
						dictionary.Remove(current);
					}
					foreach (KeyValuePair<string, object> current2 in ((Dictionary<string, object>)RequestMap.AsDictionary(@string)))
					{
						dictionary.Add(current2.Key, current2.Value);
					}
				}
			}
			return map;
		}
	}
}
