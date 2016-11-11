#if NET461
using log4net;
using log4net.Config;
using System.Web;
#elif NETSTANDARD1_6
using Microsoft.Extensions.Logging;
using RestSharp.Extensions.MonoHttp;
#endif
using MasterCard.Core.Exceptions;
using MasterCard.Core.Model;
using MasterCard.Core.Security;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using MasterCard_Core_Unofficial.MasterCard.Extensions;
using MasterCard_Core_Unofficial.MasterCard.Core;
using System.Threading.Tasks;

namespace MasterCard.Core {
  public class ApiController {
#if NET461
        private static readonly ILog log;
#elif NETSTANDARD1_6
    private static readonly ILogger log;
#endif

    private string hostUrl;

    private string apiVersion;

    private IRestClient restClient;

    static ApiController() {
#if NET461
            ApiController.log = LogManager.GetLogger(typeof(ApiController));
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if(ApiConfig.IsDebug()) {
                if(File.Exists("log4net.xml")) {
                    XmlConfigurator.Configure(new FileInfo("log4net.xml"));
                    return;
                }
                BasicConfigurator.Configure();
            }
#elif NETSTANDARD1_6
      ILoggerFactory loggerFactory = new LoggerFactory();

      if(ApiConfig.IsDebug()) {
        loggerFactory.AddDebug();
      }

      ApiController.log = loggerFactory.CreateLogger<ApiController>();
#endif
    }

    private readonly IndividualApiConfig _apiConfig;

    public ApiController(string apiVersion, IndividualApiConfig apiConfig = null) {
      this.apiVersion = apiVersion;
      this._apiConfig = apiConfig;
      this.CheckState();
      this.hostUrl = _apiConfig?.GetLiveUrl() ?? ApiConfig.GetLiveUrl();
      if(_apiConfig?.IsSandbox() ?? ApiConfig.IsSandbox()) {
        this.hostUrl = _apiConfig?.GetSandboxUrl() ?? ApiConfig.GetSandboxUrl();
      }
    }

    public void SetRestClient(IRestClient restClient) {
      this.restClient = restClient;
    }

    public virtual IDictionary<string, object> Execute(OperationConfig config, OperationMetadata metadata, BaseObject requestMap) {
      RestyRequest request;
      CryptographyInterceptor interceptor;
      IRestClient restClient;
      try {
        request = this.GetRequest(config, metadata, requestMap);
        interceptor = request.interceptor;
        if(this.restClient != null) {
          restClient = this.restClient;
          restClient.BaseUrl = request.BaseUrl;
        } else {
          restClient = new RestClient(request.BaseUrl);
        }
      } catch(Exception ex) {
        throw new ApiException(ex.Message, ex);
      }
      IRestResponse restResponse;
      try {
        ApiController.log.Debug(string.Concat(new object[]
        {
                    ">>Execute(action='",
                    config.Action,
                    "', resourcePaht='",
                    config.ResourcePath,
                    "', requestMap='",
                    requestMap,
                    "'"
        }));
        ApiController.log.Debug("excute(), request.Method='" + request.Method + "'");
        ApiController.log.Debug("excute(), request.URL=" + request.AbsoluteUrl.ToString());
        ApiController.log.Debug("excute(), request.Header=");
        ApiController.log.Debug(request.Parameters.Where(x => x.Type == ParameterType.HttpHeader));
        ApiController.log.Debug("excute(), request.Body=");
        ApiController.log.Debug(request.Parameters.Where(x => x.Type == ParameterType.RequestBody));

        
        restClient.UseSynchronizationContext = true;
#if NET461
        System.Net.ServicePointManager.Expect100Continue = false;
        System.Net.ServicePointManager.MaxServicePointIdleTime = 5000;
        //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
        restResponse = restClient.Execute(request);
#elif NETSTANDARD1_6
        restResponse = AsyncHelpers.RunSync(async () => {
          return await restClient.ExecuteRequestAsync(request);
        },
        () => {
          this._apiConfig?.getDoEvents()?.Invoke();
          return Task.FromResult(0);
        });
#endif
        ApiController.log.Debug("Execute(), response.Header=");
        ApiController.log.Debug(restResponse.Headers);
        ApiController.log.Debug("Execute(), response.Body=");
        ApiController.log.Debug(restResponse.Content.ToString());
      } catch(Exception ex3) {
        Exception ex2 = new ApiCommunicationException(ex3.Message, ex3);
        ApiController.log.Error(ex2.Message, ex2);
        throw ex2;
      }
      if(restResponse.ErrorException != null || restResponse.Content == null) {
        Exception ex4 = new MasterCard.Core.Exceptions.SystemException(restResponse.ErrorMessage, restResponse.ErrorException);
        ApiController.log.Error(ex4.Message, ex4);
        throw ex4;
      }
      IDictionary<string, object> dictionary = null;
      if(restResponse.Content.StartsWith("{") || restResponse.Content.StartsWith("[") || restResponse.ContentType == "application/json") {
        try {
          dictionary = RequestMap.AsDictionary(restResponse.Content);
          if(interceptor != null) {
            dictionary = interceptor.Encrypt(dictionary);
          }
        } catch(Exception) {
          throw new MasterCard.Core.Exceptions.SystemException("Error: parsing JSON response", restResponse.Content);
        }
      } else if(restResponse.Content.StartsWith("<") || restResponse.ContentType == "application/xml") {
        try {
          dictionary = RequestMap.AsDictionaryFromXml(restResponse.Content);
          if(interceptor != null) {
            dictionary = interceptor.Encrypt(dictionary);
          }
        } catch(Exception) {
          throw new MasterCard.Core.Exceptions.SystemException("Error: parsing XML response", restResponse.Content);
        }
      }
      if(restResponse.StatusCode < HttpStatusCode.MultipleChoices) {
        ApiController.log.Debug("<<Execute()");
        return dictionary;
      }
      try {
        ApiController.ThrowException(dictionary, restResponse);
      } catch(Exception ex5) {
        ApiController.log.Error(ex5.Message, ex5);
        throw ex5;
      }
      return null;
    }

    private static void ThrowException(IDictionary<string, object> responseObj, IRestResponse response) {
      int statusCode = (int)response.StatusCode;
      if(statusCode == 400) {
        if(responseObj != null) {
          throw new InvalidRequestException(statusCode, responseObj);
        }
        throw new InvalidRequestException(statusCode.ToString(), response.Content);
      } else if(statusCode == 302) {
        if(responseObj != null) {
          throw new InvalidRequestException(statusCode, responseObj);
        }
        throw new InvalidRequestException(statusCode.ToString(), response.Content);
      } else if(statusCode == 401) {
        if(responseObj != null) {
          throw new AuthenticationException(statusCode, responseObj);
        }
        throw new AuthenticationException(statusCode.ToString(), response.Content);
      } else if(statusCode == 404) {
        if(responseObj != null) {
          throw new ObjectNotFoundException(statusCode, responseObj);
        }
        throw new ObjectNotFoundException(statusCode.ToString(), response.Content);
      } else if(statusCode == 405) {
        if(responseObj != null) {
          throw new NotAllowedException(statusCode, responseObj);
        }
        throw new NotAllowedException(statusCode.ToString(), response.Content);
      } else if(statusCode < 500) {
        if(responseObj != null) {
          throw new InvalidRequestException(statusCode, responseObj);
        }
        throw new InvalidRequestException(statusCode.ToString(), response.Content);
      } else {
        if(responseObj != null) {
          throw new MasterCard.Core.Exceptions.SystemException(statusCode, responseObj);
        }
        throw new MasterCard.Core.Exceptions.SystemException(statusCode.ToString(), response.Content);
      }
    }

    private void CheckState() {
      if((_apiConfig?.GetAuthentication() ?? ApiConfig.GetAuthentication()) == null) {
        throw new InvalidOperationException("No ApiConfig.authentication has been configured");
      }
      try {
        new Uri(_apiConfig?.GetLiveUrl() ?? ApiConfig.GetLiveUrl());
      } catch(UriFormatException innerException) {
        throw new InvalidOperationException("Invalid URL supplied for API_BASE_LIVE_URL", innerException);
      }
      try {
        new Uri(_apiConfig?.GetSandboxUrl() ?? ApiConfig.GetSandboxUrl());
      } catch(UriFormatException innerException2) {
        throw new InvalidOperationException("Invalid URL supplied for API_BASE_SANDBOX_URL", innerException2);
      }
    }

    private void AppendToQueryString(StringBuilder s, string stringToAppend) {
      if(s.ToString().IndexOf("?") == -1) {
        s.Append("?");
      }
      if(s.ToString().IndexOf("?") != s.Length - 1) {
        s.Append("&");
      }
      s.Append(stringToAppend);
    }

    private string GetURLEncodedString(object stringToEncode) {
      return HttpUtility.UrlEncode(stringToEncode.ToString(), Encoding.UTF8);
    }

    private Uri GetURL(OperationConfig config, OperationMetadata metadata, IDictionary<string, object> inputMap) {
      List<string> queryParams = config.QueryParams;
      string replacedPath = Util.GetReplacedPath(((metadata.Host == null) ? this.hostUrl : metadata.Host) + config.ResourcePath, inputMap);
      int num = 0;
      List<object> list = new List<object>();
      StringBuilder stringBuilder = new StringBuilder("{" + num++ + "}");
      list.Add(replacedPath);
      string action = config.Action;
      if((action == "read" || action == "delete" || action == "list" || action == "query") && inputMap != null && inputMap.Count > 0) {
        foreach(KeyValuePair<string, object> current in inputMap) {
          this.AppendToQueryString(stringBuilder, string.Concat(new object[]
          {
                        "{",
                        num++,
                        "}={",
                        num++,
                        "}"
          }));
          list.Add(this.GetURLEncodedString(current.Key.ToString()));
          list.Add(this.GetURLEncodedString(current.Value.ToString()));
        }
      }
      if(queryParams.Count > 0) {
        action = config.Action;
        if(action == "create" || action == "update") {
          foreach(KeyValuePair<string, object> current2 in Util.SubMap(inputMap, queryParams)) {
            this.AppendToQueryString(stringBuilder, string.Concat(new object[]
            {
                            "{",
                            num++,
                            "}={",
                            num++,
                            "}"
            }));
            list.Add(this.GetURLEncodedString(current2.Key.ToString()));
            list.Add(this.GetURLEncodedString(current2.Value.ToString()));
          }
        }
      }
      if(config.RepsonseType == DataType.Json) {
        this.AppendToQueryString(stringBuilder, "Format=JSON");
      } else if(config.RepsonseType == DataType.Xml) {
        this.AppendToQueryString(stringBuilder, "Format=XML");
      }
      Uri result;
      try {
        result = new Uri(string.Format(stringBuilder.ToString(), list.ToArray()));
      } catch(UriFormatException innerException) {
        throw new InvalidOperationException("Failed to build URI", innerException);
      }
      return result;
    }

    private RestyRequest GetRequest(OperationConfig config, OperationMetadata metadata, RequestMap requestMap) {
      RestyRequest restyRequest = null;
      IDictionary<string, object> dictionary = requestMap.Clone();
      IDictionary<string, object> dictionary2 = Util.SubMap(dictionary, config.HeaderParams);
      Uri uRL = this.GetURL(config, metadata, dictionary);
      Uri baseUrl = new Uri(string.Concat(new object[]
      {
                uRL.Scheme,
                "://",
                uRL.Host,
                ":",
                uRL.Port
      }));
      CryptographyInterceptor cryptographyInterceptor = _apiConfig?.GetCryptographyInterceptor(uRL.AbsolutePath) ?? ApiConfig.GetCryptographyInterceptor(uRL.AbsolutePath);
      string action = config.Action;
      if(!(action == "create")) {
        if(!(action == "delete")) {
          if(!(action == "update")) {
            if(action == "read" || action == "list" || action == "query") {
              restyRequest = new RestyRequest(uRL, Method.GET);
            }
          } else {
            restyRequest = new RestyRequest(uRL, Method.PUT);
            if(cryptographyInterceptor != null) {
              dictionary = cryptographyInterceptor.Encrypt(dictionary);
            }
            if(config.RequestType == DataType.Json) {
              restyRequest.AddJsonBody(dictionary);
            } else if(config.RequestType == DataType.Xml) {
              restyRequest.AddXmlBody(dictionary);
            }
          }
        } else {
          restyRequest = new RestyRequest(uRL, Method.DELETE);
        }
      } else {
        restyRequest = new RestyRequest(uRL, Method.POST);
        if(cryptographyInterceptor != null) {
          dictionary = cryptographyInterceptor.Encrypt(dictionary);
        }
        if(config.RequestType == DataType.Json) {
          restyRequest.AddJsonBody(dictionary);
        } else if(config.RequestType == DataType.Xml) {
          restyRequest.AddXmlBody(dictionary);
        }
      }
      if(config.RepsonseType == DataType.Json) {
        restyRequest.AddHeader("Accept", "application/json");
      } else if(config.RepsonseType == DataType.Xml) {
        restyRequest.AddHeader("Accept", "application/xml");
      }
      if(config.RequestType == DataType.Json) {
        restyRequest.AddHeader("Content-Type", "application/json");
      } else if(config.RequestType == DataType.Xml) {
        restyRequest.AddHeader("Content-Type", "application/xml");
      }

      restyRequest.AddHeader("User-Agent", "CSharp-SDK-Unofficial/" + this.apiVersion);
      foreach(KeyValuePair<string, object> current in dictionary2) {
        restyRequest.AddHeader(current.Key, current.Value.ToString());
      }
      (_apiConfig?.GetAuthentication() ?? ApiConfig.GetAuthentication()).SignRequest(uRL, restyRequest);
      restyRequest.AbsoluteUrl = uRL;
      restyRequest.BaseUrl = baseUrl;
      restyRequest.interceptor = cryptographyInterceptor;
      return restyRequest;
    }
  }
}
