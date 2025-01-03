using System.Collections.Generic;
using System.Threading.Tasks;
using static Transversal.Common.RestClient;

namespace Transversal.Common
{
    public interface IRestClient
    {
        Task<RestEntity> PostAsync(string uri, dynamic request);
        Task<RestEntity> GetAsync(string uri);
        Task<RestEntity> GetAsyncWithHeaders(string uri, string token, List<HeaderRest> headers);
        Task<RestEntity> PostAsyncWithHeaders(string uri, List<HeaderRest> headers, dynamic request);
        Task<RestEntity> PostAsyncWithTokenAndHeaders(string uri, dynamic request, string token, List<HeaderRest> headers);
    }
}
