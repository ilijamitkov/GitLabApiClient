using System.Collections.Generic;
using System.Threading.Tasks;
using GitLabApiClient.Models.Users.Responses;

namespace GitLabApiClient.Internal.Http
{
    public interface IGitLabHttpFacade
    {
        Task<IList<T>> GetPagedList<T>(string uri);
        
        Task<T> Get<T>(string uri);

        Task<T> Post<T>(string uri, object data = null) where T : class;

        Task Post(string uri, object data = null);

        Task<T> Put<T>(string uri, object data);

        Task Put(string uri, object data);

        Task Delete(string uri);

        Task<Session> LoginAsync(string username, string password);
    }
}