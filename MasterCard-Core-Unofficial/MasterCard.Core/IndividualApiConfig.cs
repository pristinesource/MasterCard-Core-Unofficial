using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterCard.Core.Security;

namespace MasterCard_Core_Unofficial.MasterCard.Core {
  public class IndividualApiConfig {

    private bool SANDBOX = true;

    private AuthenticationInterface authentication;

    private Dictionary<string, object> cryptographyMap = new Dictionary<string, object>();

    private string API_BASE_LIVE_URL = "https://api.mastercard.com";

    private string API_BASE_SANDBOX_URL = "https://sandbox.api.mastercard.com";

    public string GetLiveUrl() {
      return this.API_BASE_LIVE_URL;
    }

    public string GetSandboxUrl() {
      return this.API_BASE_SANDBOX_URL;
    }

    public void SetSandbox(bool sandbox) {
      this.SANDBOX = sandbox;
    }

    public bool IsSandbox() {
      return this.SANDBOX;
    }

    public bool IsProduction() {
      return !this.SANDBOX;
    }

    public AuthenticationInterface GetAuthentication() {
      return this.authentication;
    }

    public void SetAuthentication(AuthenticationInterface authentication) {
      this.authentication = authentication;
    }

    public void AddCryptographyInterceptor(CryptographyInterceptor cryptographyInterceptor) {
      if(!this.cryptographyMap.ContainsKey(cryptographyInterceptor.GetTriggeringPath())) {
        this.cryptographyMap.Add(cryptographyInterceptor.GetTriggeringPath(), cryptographyInterceptor);
      }
    }

    public CryptographyInterceptor GetCryptographyInterceptor(string basePath) {
      foreach(KeyValuePair<string, object> current in this.cryptographyMap) {
        if(current.Key.Contains(basePath) || basePath.Contains(current.Key)) {
          return (CryptographyInterceptor)current.Value;
        }
      }
      return null;
    }

    public void setLocalhost() {
      API_BASE_SANDBOX_URL = "http://localhost:8080";
      API_BASE_LIVE_URL = "http://localhost:8080";
    }

    public void unsetLocalhost() {
      API_BASE_SANDBOX_URL = "https://sandbox.api.mastercard.com";
      API_BASE_LIVE_URL = "https://api.mastercard.com";
    }

    public void setAPIBaseCustomHosts(Uri SandboxUrl = null, Uri LiveUrl = null) {
      API_BASE_SANDBOX_URL = SandboxUrl?.ToString() ?? API_BASE_SANDBOX_URL;
      API_BASE_LIVE_URL = LiveUrl?.ToString() ?? API_BASE_LIVE_URL;
    }

    public void unsetAPIBaseCustomHosts() {
      unsetLocalhost();
    }
  }
}
