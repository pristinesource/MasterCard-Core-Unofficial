/*
 * Copyright 2016 MasterCard International.
 *
 * Redistribution and use in source and binary forms, with or without modification, are 
 * permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this list of 
 * conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of 
 * conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 * Neither the name of the MasterCard International Incorporated nor the names of its 
 * contributors may be used to endorse or promote products derived from this software 
 * without specific prior written permission.
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
 * SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
 * TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
 * IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING 
 * IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
 * SUCH DAMAGE.
 *
 */




using System;
using System.Collections.Generic;
using NUnit.Framework;


using MasterCard.Core;
using MasterCard.Core.Exceptions;
using MasterCard.Core.Model;
using MasterCard.Core.Security.OAuth;
using MasterCard.Core.Security.MDES;

namespace TestMasterCard
{


	[TestFixture ()]
	public class TokenActivationTest
	{

		[SetUp]
		public void setup ()
		{
			ApiConfig.SetSandbox (true);
			ApiConfig.SetDebug (true);


            var path = MasterCard.Core.Util.GetCurrenyAssemblyPath();


            var authentication = new OAuthAuthentication ("gVaoFbo86jmTfOB4NUyGKaAchVEU8ZVPalHQRLTxeaf750b6!414b543630362f426b4f6636415a5973656c33735661383d", path+"\\prod_key.p12", "alias", "password");
			ApiConfig.SetAuthentication (authentication);

			String mastercardPublic = path+"\\mastercard_public.crt";
			String mastercardPrivate = path+"\\mastercard_private.pem";
            var interceptor = new MDESCryptography(mastercardPublic, mastercardPrivate);
			ApiConfig.AddCryptographyInterceptor (interceptor);

		}

        
            
            
            
            
            
            
                        

        [Test ()]
        public void example_test_tokenization()
        {
            RequestMap parameters = new RequestMap();
            
			parameters.Set("tokenRequestorId", "12345678901" );
			parameters.Set("requestId", "123456");
			parameters.Set("cardInfo.accountNumber", "5123456789012345");
			parameters.Set("cardInfo.expiryMonth", "12");
			parameters.Set("cardInfo.expiryYear", "16");
			parameters.Set("cardInfo.securityCode", "123");
			parameters.Set("cardInfo.billingAddress.line", "100 1st Street");
			parameters.Set("cardInfo.billingAddress.line2", "Apt. 4B");
			parameters.Set("cardInfo.billingAddress.city", "St. Louis");
			parameters.Set("cardInfo.billingAddress.countrySubdivision", "MO");
			parameters.Set("cardInfo.billingAddress.postalCode", "61000");
			parameters.Set("cardInfo.billingAddress.country", "USA");
            
            

			TokenActivation response = TokenActivation.Create(parameters);
			Assert.That("APPROVED", Is.EqualTo(response["decision"]).IgnoreCase);
            
            

        }
        
            
        
    }
}

