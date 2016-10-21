﻿#if ENABLE_TESTS


using System;
using NUnit.Framework;
using System.Text;

using MasterCard.Core;
using System.Collections.Generic;
using MasterCard.Core.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.IO;

namespace TestMasterCard
{
	[TestFixture]
	public class CryptUtilTest
	{
		[Test]
		public void TestHexUnHex ()
		{
			String nonHexed = "andrea_rizzini@mastercard.com";
			String hexed = CryptUtil.HexEncode(nonHexed);

			byte[] nonHexedBytes = CryptUtil.HexDecode (hexed);
			String nonHexed2 = System.Text.Encoding.UTF8.GetString (nonHexedBytes);

			Assert.AreEqual (nonHexed, nonHexed2);

		}

		[Test]
		public void TestEncryptDecryptAES ()
		{
			String data = "andrea_rizzini@mastercard.com";
			Tuple<byte[], byte[], byte[]> tuple = CryptUtil.EncryptAES (System.Text.Encoding.UTF8.GetBytes(data));


			byte[] decryptedData = CryptUtil.DecryptAES (tuple.Item1, tuple.Item2, tuple.Item3);
			String data2 = System.Text.Encoding.UTF8.GetString (decryptedData);

			Assert.AreEqual (data, data2);

		}

		[Test]
		public void TestEncryptDecryptRSA () {

            string certPath = MasterCard.Core.Util.GetCurrenyAssemblyPath() + "\\certificate.p12";
            X509Certificate2 cert = new X509Certificate2(certPath , "", X509KeyStorageFlags.Exportable);

            var publicKey = cert.GetRSAPublicKey();
            var privateKey = cert.GetRSAPrivateKey();

			String data = "andrea_rizzini@mastercard.com";

			byte[] encryptedData = CryptUtil.EncrytptRSA (Encoding.UTF8.GetBytes (data), publicKey);

			Assert.NotNull (encryptedData);

			byte[] decryptedData = CryptUtil.DecryptRSA (encryptedData, privateKey);

			String dataOut = System.Text.Encoding.UTF8.GetString (decryptedData);

			Assert.AreEqual (data, dataOut);





		}




	




	}
}

#endif