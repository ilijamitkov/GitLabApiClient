﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GitLabApiClient.Internal.Queries;
using GitLabApiClient.Models.Groups.Requests;
using Xunit;
using static GitLabApiClient.Test.Utilities.GitLabApiHelper;

namespace GitLabApiClient.Test
{
    public class GroupsClientTest
    {
        private readonly List<int> _groupIdsToClean = new List<int>();

        private readonly GroupsClient _sut = new GroupsClient(
            GetFacade(),
            new GroupsQueryBuilder(),
            new ProjectsGroupQueryBuilder());

        [Fact]
        public async Task GroupCanBeRetrievedByGroupId()
        {
            var group = await _sut.GetByGroupIdAsync(TestGroupName, CancellationToken.None);
            group.FullName.Should().Be(TestGroupName);
            group.FullPath.Should().Be(TestGroupName);
            group.Name.Should().Be(TestGroupName);
            group.Path.Should().Be(TestGroupName);
            group.Visibility.Should().Be(GroupsVisibility.Private);
            group.Description.Should().BeEmpty();
        }
        
        [Fact]
        public async Task ProjectsCanBeTransferToGroup_Throws_when_project_already_in_group()
        {
            Func<Task> action = () => _sut.TransferAsync(TestGroupName, TestProjectId.ToString(), CancellationToken.None);
            await Assert.ThrowsAsync<GitLabException>(action);
        }

        [Fact]
        public async Task ProjectsCanBeRetrievedFromGroup()
        {
            var project = await _sut.GetProjectsAsync(TestGroupName, o => o.Search = TestProjectName, CancellationToken.None);
            project.Should().ContainSingle(s => s.Name == TestProjectName);
        }

        [Fact]
        public async Task GroupsCanBeRetrievedFromSearch()
        {
            var group = await _sut.SearchAsync("gitlab", CancellationToken.None);
            group.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GroupsCanBeRetrievedFromQuery()
        {
            var group = await _sut.GetAsync(o =>
            {
                o.Search = "gitlab";
                o.Order = GroupsOrder.Name;
                o.Sort = GroupsSort.Descending;
                o.AllAvailable = true;
            }, CancellationToken.None);

            group.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GroupCanBeCreated()
        {
            string groupName = GetRandomGroupName();

            var request = new CreateGroupRequest(groupName, groupName)
            {
                Description = "description1",
                Visibility = GroupsVisibility.Public,
                LfsEnabled = true,
                RequestAccessEnabled = true
            };

            var response = await _sut.CreateAsync(request, CancellationToken.None);
            _groupIdsToClean.Add(response.Id);

            response.Name.Should().Be(groupName);
            response.Description.Should().Be("description1");
            response.Visibility.Should().Be(GroupsVisibility.Public);
            response.LfsEnabled.Should().BeTrue();
            response.RequestAccessEnabled.Should().BeTrue();
        }

        [Fact]
        public async Task CreatedGroupCanBeUpdated()
        {
            string groupName = GetRandomGroupName();

            var createGroupRequest = new CreateGroupRequest(groupName, groupName)
            {
                Description = "description1",
                Visibility = GroupsVisibility.Public,
                LfsEnabled = true,
                RequestAccessEnabled = true
            };

            var createGroupResponse = await _sut.CreateAsync(createGroupRequest, CancellationToken.None);
            _groupIdsToClean.Add(createGroupResponse.Id);

            string updateGroupName = GetRandomGroupName();

            var updateRequest = new UpdateGroupRequest(createGroupResponse.Id)
            {
                Name = updateGroupName,
                Description = "description2",
                Visibility = GroupsVisibility.Internal,
                LfsEnabled = false,
                RequestAccessEnabled = false
            };

            var updateGroupResponse = await _sut.UpdateAsync(updateRequest, CancellationToken.None);
            updateGroupResponse.Name.Should().Be(updateGroupName);
            updateGroupResponse.Description.Should().Be("description2");
            updateGroupResponse.Visibility.Should().Be(GroupsVisibility.Internal);
            updateGroupResponse.LfsEnabled.Should().BeFalse();
            updateGroupResponse.RequestAccessEnabled.Should().BeFalse();
        }

        public Task InitializeAsync() 
            => CleanupGroups();

        public Task DisposeAsync() 
            => CleanupGroups();

        private async Task CleanupGroups()
        {
            foreach (int groupId in _groupIdsToClean)
                await _sut.DeleteAsync(groupId.ToString(), CancellationToken.None);
        }

        private static string GetRandomGroupName() 
            => "test-gitlabapiclient" + Path.GetRandomFileName();
    }
}
