using System;
using GitLabApiClient.Internal.Utilities;
using GitLabApiClient.Models.Groups.Requests;

namespace GitLabApiClient.Internal.Queries
{
    internal sealed class BranchesQueryBuilder : QueryBuilder<CreateBranchQueryOptions>
    {
        protected override void BuildCore(CreateBranchQueryOptions options)
        {
            if (!options.Id.IsNullOrEmpty())
            {
                Add("id", options.Id);
            }
                
            if (!options.Branch.IsNullOrEmpty())
            {
                Add("branch", options.Branch);
            }
            
            if (!options.Ref.IsNullOrEmpty())
            {
                Add("ref", options.Ref);
            }
        }

    }
}