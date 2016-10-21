using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterCard.Core;
using MasterCard.Core.Model;
using MasterCard.Core.Security.OAuth;
using NUnit.Framework;

namespace TestMasterCard
{
    [TestFixture()]
    public class TerminationInquiryHistoryRequestTest {

        [SetUp]
        public void setup() {
            //base.setup();

            ApiConfig.SetDebug(true);
            /*
            ApiConfig.SetSandbox(true);
            string certPath = Util.GetCurrenyAssemblyPath() + "\\mcapi_sandbox_key.p12";
            ApiConfig.SetAuthentication(new OAuthAuthentication("L5BsiPgaF-O3qA36znUATgQXwJB6MRoMSdhjd7wt50c97279!50596e52466e3966546d434b7354584c4975693238513d3d", certPath, "alias", "password"));
            */

            string consumerKey = "L5BsiPgaF-O3qA36znUATgQXwJB6MRoMSdhjd7wt50c97279!50596e52466e3966546d434b7354584c4975693238513d3d";   // You should copy this from "My Keys" on your project page e.g. UTfbhDCSeNYvJpLL5l028sWL9it739PYh6LU5lZja15xcRpY!fd209e6c579dc9d7be52da93d35ae6b6c167c174690b72fa
            string keyAlias = "alias";   // For production: change this to the key alias you chose when you created your production key
            string keyPassword = "password";   // For production: change this to the key alias you chose when you created your production key
            var path = Util.GetCurrenyAssemblyPath(); // This returns the path to your assembly so it be used to locate your cert
            string certPath = path + "\\mcapi_sandbox_key.p12"; // e.g. /Users/yourname/project/sandbox.p12 | C:\Users\yourname\project\sandbox.p12
            ApiConfig.SetAuthentication(new OAuthAuthentication(consumerKey, certPath, keyAlias, keyPassword));   // You only need to set this once
            ApiConfig.SetSandbox(true); // For production: use ApiConfig.setSandbox(false)

        }

        [Test]
        public void TestExampleTerminationHistoryInquiry() {

            var map = new RequestMap();
            map.Set("IRN", "19962010012700000");
            map.Set("AcquirerId", "1996");
            map.Set("PageOffset", "0");
            map.Set("PageLength", "10");
            map.Set("id", "");

            var response = TerminationInquiryHistoryRequest.Read(map);
            Assert.NotNull(response);

            int c = 0;
            foreach(var r in response) {
                //putResponse("example_termination_history_inquiry[" + c.ToString() + "]", r);
                c++;
            }
        }
    }



    public class TerminationInquiryHistoryRequest : BaseObject {

        public TerminationInquiryHistoryRequest(RequestMap bm) : base(bm) {
        }

        public TerminationInquiryHistoryRequest(IDictionary<string, object> map) : base(map) {
        }

        public TerminationInquiryHistoryRequest() {
        }

        /// <summary>
        /// Creates object of type TerminationInquiryHistoryRequest
        /// </summary>
        /// <param name="criteria">containing the required parameters to read</param>
        /// <returns>TerminationInquiryHistoryRequest of the response.</returns>
        public static ResourceList<TerminationInquiryHistoryRequest> Read(RequestMap criteria) {
            return BaseObject.ExecuteForList("read", new TerminationInquiryHistoryRequest(criteria));
        }

        /// <summary>
        /// Creates object of type TerminationInquiryHistoryRequest
        /// </summary>
        /// <param name="criteria">containing the required parameters to read</param>
        /// <returns>TerminationInquiryHistoryRequest of the response.</returns>
        public static ResourceList<TerminationInquiryHistoryRequest> Read(IDictionary<string, object> criteria) {
            return BaseObject.ExecuteForList("read", new TerminationInquiryHistoryRequest(criteria));
        }

        protected override OperationConfig GetOperationConfig(string operationUUID) {
            if(operationUUID == "read") {
                return new OperationConfig(
                    "/fraud/merchant/v3/termination-inquiry/{IRN}",
                    "read",
                    new List<string>() { "PageOffset", "PageLength", "AcquirerId" },
                    new List<string>()
                );
            }

            throw new Exception("Invalid operation supplied: " + operationUUID);
        }

        protected override OperationMetadata GetOperationMetadata() {
            return new OperationMetadata("1.0.1", null);
        }
    }
}
