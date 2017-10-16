using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GitLabApiClient.Internal.Http;
using GitLabApiClient.Internal.Queries;
using GitLabApiClient.Internal.Utilities;
using GitLabApiClient.Models.Projects.Requests;
using GitLabApiClient.Models.Projects.Responses;
using GitLabApiClient.Models.Users.Responses;

namespace GitLabApiClient
{
    /// <summary>
    /// Used to query GitLab API to retrieve, modify, create projects.
    /// <exception cref="GitLabException">Thrown if request to GitLab API fails</exception>
    /// <exception cref="HttpRequestException">Thrown if request to GitLab API fails</exception>
    /// </summary>
    public sealed class ProjectsClient
    {
        private readonly IGitLabHttpFacade _httpFacade;
        private readonly ProjectsQueryBuilder _queryBuilder;

        internal ProjectsClient(IGitLabHttpFacade httpFacade, ProjectsQueryBuilder queryBuilder)
        {
            _httpFacade = httpFacade;
            _queryBuilder = queryBuilder;
        }

        /// <summary>
        /// Retrieves project by it's id.
        /// </summary>
        /// <param name="projectId">Id of the project.</param>
        /// <param name="cancellationToken">Request CancellationToken</param>
        public async Task<Project> GetAsync(int projectId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var registration = cancellationToken.Register(cancellationToken.ThrowIfCancellationRequested))
            {
            }
            return await _httpFacade.Get<Project>($"projects/{projectId}");
        }
            

        /// <summary>
        /// Get a list of visible projects for authenticated user. 
        /// When accessed without authentication, only public projects are returned.
        /// </summary>
        /// <param name="cancellationToken">Request CancellationToken</param>
        public async Task<IList<Project>> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsync(null, cancellationToken);
        }
        
        /// <summary>
        /// Get a list of visible projects for authenticated user. 
        /// When accessed without authentication, only public projects are returned.
        /// </summary>
        /// <param name="options">Query options.</param>
        /// <param name="cancellationToken">Request CancellationToken</param>
        public async Task<IList<Project>> GetAsync(Action<ProjectQueryOptions> options, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var registration = cancellationToken.Register(cancellationToken.ThrowIfCancellationRequested))
            {
            }
            var queryOptions = new ProjectQueryOptions();
            options?.Invoke(queryOptions);

            string url = _queryBuilder.Build("projects", queryOptions);
            return await _httpFacade.GetPagedList<Project>(url);
        }

        /// <summary>
        /// Get the users list of a project.
        /// </summary>
        /// <param name="projectId">Id of the project.</param>
        /// <param name="cancellationToken">Request CancellationToken</param>
        public async Task<IList<User>> GetUsersAsync(int projectId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var registration = cancellationToken.Register(cancellationToken.ThrowIfCancellationRequested))
            {
            }
            return await _httpFacade.GetPagedList<User>($"projects/{projectId}/users");
        }

        /// <summary>
        /// Creates new project.
        /// </summary>
        /// <param name="request">Create project request.</param>
        /// <param name="cancellationToken">Request CancellationToken</param>
        /// <returns>Newly created project.</returns>
        public async Task<Project> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var registration = cancellationToken.Register(cancellationToken.ThrowIfCancellationRequested))
            {
            }
            Guard.NotNull(request, nameof(request));
            return await _httpFacade.Post<Project>("projects", request);
        }

        /// <summary>
        /// Updates existing project.
        /// </summary>
        /// <param name="request">Update project request.</param>
        /// <param name="cancellationToken">Request CancellationToken</param>
        /// <returns>Newly modified project.</returns>
        public async Task<Project> UpdateAsync(UpdateProjectRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var registration = cancellationToken.Register(cancellationToken.ThrowIfCancellationRequested))
            {
            }
            Guard.NotNull(request, nameof(request));
            return await _httpFacade.Put<Project>($"projects/{request.ProjectId}", request);
        }

        /// <summary>
        /// Deletes project.
        /// </summary>
        /// <param name="id">Id of the project.</param>
        /// <param name="cancellationToken">Request CancellationToken</param>
        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await _httpFacade.Delete($"projects/{id}");
        }
            
    }
}