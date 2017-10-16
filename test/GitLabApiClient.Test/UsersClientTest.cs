using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GitLabApiClient.Test.Utilities;
using Xunit;

namespace GitLabApiClient.Test
{
    public class UsersClientTest
    {
        private readonly UsersClient _sut = new UsersClient(GitLabApiHelper.GetFacade());

        [Fact]
        public async Task CurrentUserSessionCanBeRetrieved()
        {
            var session = await _sut.GetCurrentSessionAsync(CancellationToken.None);
            session.Username.Should().Be("root");
            session.Name.Should().Be("Administrator");
        }

        [Fact]
        public async Task UserRetrievedByName()
        {
            var user = await _sut.GetAsync("root", CancellationToken.None);
            
            Assert.NotNull(user);
            Assert.Equal(user.Username, "root");
            Assert.Equal(user.Name, "Administrator");
        }

        [Fact]
        public async Task NonExistingUserRetrievedAsNull()
        {
            var user = await _sut.GetAsync("test-gixxxtlabapiclient", CancellationToken.None);
            user.Should().BeNull();
        }
    }
}
