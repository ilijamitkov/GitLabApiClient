using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GitLabApiClient.Internal.Queries;
using GitLabApiClient.Test.Utilities;
using Xunit;

namespace GitLabApiClient.Test
{
    public class BranchesClientTest
    {
        
        private readonly BranchesClient _sut = new BranchesClient(
            GitLabApiHelper.GetFacade(),
            new BranchesQueryBuilder());
        
        [Fact]
        public async Task GetAsyncByProjectId_returns_list_of_repository_branches ()
        {
            var repos = await _sut.GetAsync(
                GitLabApiHelper.TestProjectId.ToString(), CancellationToken.None);

            Assert.NotEmpty(repos);
        }
        
        [Fact]
        public async Task GetAsyncByProjectIdAndBranchName_returns_single_project_repository_branch()
        {
            var repo = await _sut.GetAsync(
                GitLabApiHelper.TestProjectId.ToString(), "dev", CancellationToken.None);

            Assert.NotNull(repo);
        }

        [Fact]
        public async Task CreateAsync_creates_repository_branch()
        {
            var branch = await _sut.CreateAsync(
                GitLabApiHelper.TestProjectId.ToString(), o =>
                {
                    o.Branch = Guid.NewGuid().ToString();
                    o.Ref = "dev";
                },
                CancellationToken.None);
            Assert.NotNull(branch);
        }
    }
}