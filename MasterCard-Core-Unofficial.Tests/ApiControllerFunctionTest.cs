﻿#if NOT_USED
using System;
using System.Net;
using System.Collections.Generic;
using NUnit.Framework;
using Newtonsoft.Json;
using RestSharp;
using Moq;


using MasterCard.Core;
using MasterCard.Core.Model;
using MasterCard.Core.Security.OAuth;

namespace MasterCard.Test
{
	[TestFixture ()]
	public class ApiControllerTest
	{

		[SetUp]
		public void setup ()
		{
			var authentication = new OAuthAuthentication ("gVaoFbo86jmTfOB4NUyGKaAchVEU8ZVPalHQRLTxeaf750b6!414b543630362f426b4f6636415a5973656c33735661383d", "../../Test/prod_key.p12", "alias", "password");
			ApiConfig.setAuthentication (authentication);
		}


		/// <summary>
		/// Test base object.
		/// </summary>
		public class TestBaseObject : BaseObject
		{
			public TestBaseObject(RequestMap bm) : base(bm)
			{
			}

			public TestBaseObject() : base()
			{
			}

			public override string GetResourcePath (String action)
			{
				return "/testurl/test-base-object";
			}

			public override List<string> GetHeaderParams(string action) {
				return new List<String> {  };
			}
		}

		/// <summary>
		/// Mocks the client.
		/// </summary>
		/// <returns>The client.</returns>
		/// <param name="responseCode">Response code.</param>
		/// <param name="responseMap">Response map.</param>
		public IRestClient mockClient(HttpStatusCode responseCode, RequestMap responseMap) {

			var restClient = new Mock<IRestClient>();

			restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
				.Returns(new RestResponse
					{
						StatusCode = responseCode,
						Content = (responseMap != null ) ? JsonConvert.SerializeObject(responseMap).ToString() : ""
					});

			return restClient.Object;
		}


		[Test]
		public void Test200WithMap ()
		{

			RequestMap responseMap = new RequestMap (" { \"user.name\":\"andrea\", \"user.surname\":\"rizzini\" }");
			ApiController controller = new ApiController ();

			controller.SetRestClient (mockClient (HttpStatusCode.OK, responseMap));

			IDictionary<String,Object> result = controller.execute ("create", "test1", new TestBaseObject (responseMap));
			RequestMap responseMapFromResponse = new RequestMap (result);

			Assert.IsTrue (responseMapFromResponse.ContainsKey ("user"));
			Assert.IsTrue (responseMapFromResponse.ContainsKey ("user.name"));
			Assert.IsTrue (responseMapFromResponse.ContainsKey ("user.surname"));

			Assert.AreEqual("andrea", responseMapFromResponse["user.name"]);
			Assert.AreEqual("rizzini", responseMapFromResponse["user.surname"]);

		}

		[Test]
		public void Test200WithList ()
		{

			RequestMap responseMap = new RequestMap ("[ { \"name\":\"andrea\", \"surname\":\"rizzini\" } ]");
			ApiController controller = new ApiController ();

			controller.SetRestClient (mockClient (HttpStatusCode.OK, responseMap));

			IDictionary<String,Object> result = controller.execute ("create", "test1", new TestBaseObject ());
			RequestMap responseMapFromResponse = new RequestMap (result);

			Assert.IsTrue (responseMapFromResponse.ContainsKey ("list"));
			Assert.AreEqual (typeof(List<Dictionary<String,Object>>), responseMapFromResponse ["list"].GetType () );

			Assert.AreEqual("andrea", responseMapFromResponse["list[0].name"]);
			Assert.AreEqual("rizzini", responseMapFromResponse["list[0].surname"]);

		}



		[Test]
		public void Test204 ()
		{

			RequestMap responseMap = new RequestMap (" { \"user.name\":\"andrea\", \"user.surname\":\"rizzini\" }");
			ApiController controller = new ApiController ();

			controller.SetRestClient (mockClient (HttpStatusCode.NoContent, null));

			IDictionary<String,Object> result = controller.execute ("create", "test1", new TestBaseObject (responseMap));

			Assert.IsTrue (result == null);

		}


		[Test]
		public void Test405_NotAllowedException ()
		{

			RequestMap responseMap = new RequestMap ("{\"Errors\":{\"Error\":{\"Source\":\"System\",\"ReasonCode\":\"METHOD_NOT_ALLOWED\",\"Description\":\"Method not Allowed\",\"Recoverable\":\"false\"}}}");
			ApiController controller = new ApiController ();

			controller.SetRestClient (mockClient (HttpStatusCode.MethodNotAllowed, responseMap));

			Assert.Throws<MasterCard.Core.Exceptions.NotAllowedException> (() => controller.execute ("create", "test1", new TestBaseObject (responseMap)), "Method not Allowed");
		}


		[Test]
		public void Test40O_InvalidRequestException ()
		{

			RequestMap responseMap = new RequestMap ("{\"Errors\":{\"Error\":[{\"Source\":\"Validation\",\"ReasonCode\":\"INVALID_TYPE\",\"Description\":\"The supplied field: 'date' is of an unsupported format\",\"Recoverable\":false,\"Details\":null}]}}\n");

			ApiController controller = new ApiController ();

			controller.SetRestClient (mockClient (HttpStatusCode.BadRequest, responseMap));

			Assert.Throws<MasterCard.Core.Exceptions.InvalidRequestException> (() => controller.execute ("create", "test1", new TestBaseObject (responseMap)), "The supplied field: 'date' is of an unsupported format");
		}


		[Test]
		public void Test401_AuthenticationException ()
		{

			RequestMap responseMap = new RequestMap ("{\"Errors\":{\"Error\":[{\"Source\":\"OAuth.ConsumerKey\",\"ReasonCode\":\"INVALID_CLIENT_ID\",\"Description\":\"Oauth customer key invalid\",\"Recoverable\":false,\"Details\":null}]}}");
			ApiController controller = new ApiController ();

			controller.SetRestClient (mockClient (HttpStatusCode.Unauthorized, responseMap));

			Assert.Throws<MasterCard.Core.Exceptions.AuthenticationException> (() => controller.execute ("create", "test1", new TestBaseObject (responseMap)), "Oauth customer key invalid");
		}


		[Test]
		public void Test500_InvalidRequestException ()
		{

			RequestMap responseMap = new RequestMap ("{\"Errors\":{\"Error\":[{\"Source\":\"OAuth.ConsumerKey\",\"ReasonCode\":\"INVALID_CLIENT_ID\",\"Description\":\"Something went wrong\",\"Recoverable\":false,\"Details\":null}]}}");
			ApiController controller = new ApiController ();

			controller.SetRestClient (mockClient (HttpStatusCode.InternalServerError, responseMap));

			Assert.Throws<MasterCard.Core.Exceptions.SystemException> (() => controller.execute ( "create", "test1", new TestBaseObject (responseMap)), "Something went wrong");
		}


		[Test]
		public void Test200ShowById ()
		{

			RequestMap requestMap = new RequestMap ("{\n\"id\":\"1\"\n}");
			RequestMap responseMap = new RequestMap ("{\"Account\":{\"Status\":\"true\",\"Listed\":\"true\",\"ReasonCode\":\"S\",\"Reason\":\"STOLEN\"}}");
			ApiController controller = new ApiController ();

			controller.SetRestClient (mockClient (HttpStatusCode.OK, responseMap));

			IDictionary<String,Object> result = controller.execute ("read", "test1", new TestBaseObject (requestMap));
			RequestMap responseMapFromResponse = new RequestMap (result);

			Assert.AreEqual("true", responseMapFromResponse["Account.Status"]);
			Assert.AreEqual("STOLEN", responseMapFromResponse["Account.Reason"]);
		}
	}
}

#endif