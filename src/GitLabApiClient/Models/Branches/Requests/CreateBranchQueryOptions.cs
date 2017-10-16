using System.Collections.Generic;

namespace GitLabApiClient.Models.Groups.Requests
{
	/// <summary>
	/// Options for Groups listing
	/// </summary>
	public class CreateBranchQueryOptions
    {
        internal CreateBranchQueryOptions() { }

		/// <summary>
		/// The ID or URL-encoded path of the project owned by the authenticated user
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Return list of authorized groups matching the search criteria
		/// </summary>
		public string Branch { get; set; }

		/// <summary>
		/// The branch name or commit SHA to create branch from
		/// </summary>
		public string Ref { get; set; }
    }
}
