using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GitLabApiClient.Internal.Http;
using GitLabApiClient.Models.Branches.Responses;
using NSubstitute;
using Xunit;
using GitLabApiClient.Internal.Http.Serialization;

namespace GitLabApiClient.UnitTests
{
    public class BranchesClientTests
    {
        private readonly IGitLabHttpFacade _httpFacadeMock;
        private readonly GitLabClient _client;
        public BranchesClientTests()
        {
            _httpFacadeMock = Substitute.For<IGitLabHttpFacade>();
            _client = new GitLabClient(_httpFacadeMock);
        }

        [Fact]
        public async Task GetAsync_Returns_a_list_of_branches()
        {
            // Arrange
            _httpFacadeMock.GetPagedList<Branch>("projects/test/repository/branches").Returns(new List<Branch>
            {
                new Branch { Name = "test1"}, new Branch { Name = "test2"}
            });
            
            // Act
            var branches = await _client.Branches.GetAsync("test", CancellationToken.None);
            
            //Assert
            Assert.Collection(branches, branch => Assert.Equal( branch.Name, "test1"), branch => Assert.Equal(branch.Name, "test2"));
        }
    }
}