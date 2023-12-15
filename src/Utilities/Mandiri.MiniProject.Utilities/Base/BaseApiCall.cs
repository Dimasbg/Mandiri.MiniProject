using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mandiri.MiniProject.Utilities.Base
{
    public class BaseApiCall
    {
        protected async Task<T> CallEndpoint<T>(Task<HttpResponseMessage> hit, CancellationToken cancellationToken)
        {
            HttpResponseMessage r = await hit;
            Console.WriteLine(r.StatusCode);

            if (r.IsSuccessStatusCode)
                return await r.Content.ReadAsAsync<T>(cancellationToken);

            if (r.StatusCode == HttpStatusCode.NotFound)
                return default;

            if (r.StatusCode == HttpStatusCode.BadRequest)
                throw new ApiBadRequestException(await r.Content.ReadAsStringAsync(cancellationToken));

            throw new ApiCallException($"Server error when call '{r.RequestMessage!.RequestUri}', with code:{r.StatusCode}, and message:{await r.Content.ReadAsStringAsync(cancellationToken)}");
        }
        protected async Task CallEndpoint(Task<HttpResponseMessage> hit, CancellationToken cancellationToken)
        {
            HttpResponseMessage r = await hit;
            Console.WriteLine(r.StatusCode);

            if (r.IsSuccessStatusCode)
                return;

            if (r.StatusCode == HttpStatusCode.NotFound)
                throw new DataNotFoundException(await r.Content.ReadAsStringAsync(cancellationToken));

            if (r.StatusCode == HttpStatusCode.BadRequest)
                throw new ApiBadRequestException(await r.Content.ReadAsStringAsync(cancellationToken));

            throw new ApiCallException($"Server error when call '{r.RequestMessage!.RequestUri}', with code:{r.StatusCode}, and message:{await r.Content.ReadAsStringAsync(cancellationToken)}");
        }
    }
}
