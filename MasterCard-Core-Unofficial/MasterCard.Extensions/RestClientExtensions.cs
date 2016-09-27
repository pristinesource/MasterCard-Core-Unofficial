using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace MasterCard_Core_Unofficial.MasterCard.Extensions {
    internal static class RestClientExtensions {
        internal static async Task<IRestResponse> ExecuteRequestAsync(this IRestClient _client, IRestRequest request) {
            var tcs = new TaskCompletionSource<IRestResponse>();
            _client.ExecuteAsync(request, response => {
                tcs.SetResult(response);
            });
            return await tcs.Task;
        }

    }
}
