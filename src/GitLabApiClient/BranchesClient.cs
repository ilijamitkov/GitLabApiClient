using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GitLabApiClient.Internal.Http;
using GitLabApiClient.Internal.Queries;
using GitLabApiClient.Models.Branches.Responses;
using GitLabApiClient.Models.Groups.Requests;
using GitLabApiClient.Models.Groups.Responses;
using GitLabApiClient.Models.Projects.Responses;

namespace GitLabApiClient
{
    /// <summary>
    /// Used to query GitLab API to retrieve, modify, create branches.
    /// <exception cref="GitLabException">Thrown if request to GitLab API does not indicate success</exception>
    /// <exception cref="HttpRequestException">Thrown if request to GitLab API fails</exception>
    /// </summary>
    public sealed class BranchesClient
    {
        private readonly IGitLabHttpFacade _httpFacade;
        private readonly BranchesQueryBuilder _queryBuilder;

        internal BranchesClient(
            IGitLabHttpFacade httpFacade,
            BranchesQueryBuilder queryBuilder)
        {
            _httpFacade = httpFacade;
            _queryBuilder = queryBuilder;
        }

        /// <summary>
        /// Get a list of repository branches from a project, sorted by name alphabetically.
        /// This endpoint can be accessed without authentication if the repository is publicly accessible.
        /// </summary>
        /// <param name="projectId">The ID or URL-encoded path of the project owned by the authenticated user.</param>
        /// <param name="cancellationToken">Request CancellationToken</param>
        /// <returns>Branches.</returns>
        public async Task<IList<Branch>> GetAsync(string projectId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var registration = cancellationToken.Register(cancellationToken.ThrowIfCancellationRequested))
            {
            }
            
            return await _httpFacade.GetPagedList<Branch>($"projects/{projectId}/repository/branches");
        }
  
        
        /// <summary>
        /// Get a single project repository branch.
        /// This endpoint can be accessed without authentication if the repository is publicly accessible.
        /// </summary>
        /// <param name="projectId">The ID or URL-encoded path of the project owned by the authenticated user.</param>
        /// <param name="branchName">The name of the branch</param>
        /// <param name="cancellationToken">Request CancellationToken</param>
        /// <returns>Branches.</returns>
        public async Task<Branch> GetAsync(string projectId, string branchName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var registration = cancellationToken.Register(cancellationToken.ThrowIfCancellationRequested))
            {
            }
            
            return await _httpFacade.Get<Branch>($"projects/{projectId}/repository/branches/{branchName}");
        }
        
        /// <summary>
        /// Create repository branch.
        /// </summary>
        /// <returns>The newly created branch.</returns>
        public async Task<Branch> CreateAsync(string projectId,
                                              Action<CreateBranchQueryOptions> options,
                                              CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var registration = cancellationToken.Register(cancellationToken.ThrowIfCancellationRequested))
            {
            }
            var queryOptions = new CreateBranchQueryOptions();
            options?.Invoke(queryOptions);
            string url = _queryBuilder.Build($"projects/{projectId}/repository/branches", queryOptions);
            
            return await _httpFacade.Post<Branch>(url);
        }

        /// <summary>
        /// Delete repository branch
        /// </summary>
        /// <param name="projectId">The ID or URL-encoded path of the project owned by the authenticated user</param>
        /// <param name="branchName">The name of the branch</param>
        /// <param name="cancellationToken">Request CancellationToken</param>
        /// <returns></returns>
        public async Task DeleteAsync(string projectId, string branchName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var registration = cancellationToken.Register(cancellationToken.ThrowIfCancellationRequested))
            {
            }
            await _httpFacade.Delete($"projects/{projectId}/repository/branches/{branchName}");
        }
        
        /// <summary>
        /// Delete repository branch
        /// </summary>
        /// <param name="projectId">The ID or URL-encoded path of the project owned by the authenticated user</param>
        /// <param name="cancellationToken">Request CancellationToken</param>
        /// <returns></returns>
        public async Task DeleteMergedAsync(string projectId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var registration = cancellationToken.Register(cancellationToken.ThrowIfCancellationRequested))
            {
            }
            await _httpFacade.Delete($"projects/{projectId}/repository/merged_branches");
        }
    }
}
