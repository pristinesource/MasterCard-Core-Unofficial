using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterCard.Core;
using MasterCard.Core.Security.OAuth;
using TestMasterCard;

namespace temp
{
    public class Program
    {
        public static void Main(string[] args)
        {

            TerminationInquiryHistoryRequestTest t = new TerminationInquiryHistoryRequestTest();


            ApiConfig.SetDebug(true);
            /*
            ApiConfig.SetSandbox(true);
            string certPath = Util.GetCurrenyAssemblyPath() + "\\mcapi_sandbox_key.p12";
            ApiConfig.SetAuthentication(new OAuthAuthentication("L5BsiPgaF-O3qA36znUATgQXwJB6MRoMSdhjd7wt50c97279!50596e52466e3966546d434b7354584c4975693238513d3d", certPath, "alias", "password"));
            */

            string consumerKey = "5g0TPoE5j0kbAyrSomfEpUo7UWsZPuee0rJjCPeP7dfc984d!53e27fcb4cf14262b8251a4d108c30c50000000000000000";   // You should copy this from "My Keys" on your project page e.g. UTfbhDCSeNYvJpLL5l028sWL9it739PYh6LU5lZja15xcRpY!fd209e6c579dc9d7be52da93d35ae6b6c167c174690b72fa
            string keyAlias = "keyalias";   // For production: change this to the key alias you chose when you created your production key
            string keyPassword = "keystorepassword";   // For production: change this to the key alias you chose when you created your production key
            var path = Util.GetCurrenyAssemblyPath(); // This returns the path to your assembly so it be used to locate your cert
            string certPath = path + "\\EZMATCH-DEV_sandbox.p12"; // e.g. /Users/yourname/project/sandbox.p12 | C:\Users\yourname\project\sandbox.p12
            ApiConfig.SetAuthentication(new OAuthAuthentication(consumerKey, certPath, keyAlias, keyPassword));   // You only need to set this once
            ApiConfig.SetSandbox(true); // For production: use ApiConfig.setSandbox(false)

            t.TestExampleTerminationHistoryInquiry();

        }
    }
}
