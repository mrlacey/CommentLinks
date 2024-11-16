// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System.Threading.Tasks;

namespace CommentLinks
{
	public class SponsorDetector
	{
		// This might be the code you see, but it's not what I compile into the extensions when built ;)
		public static async Task<bool> IsSponsorAsync()
		{
			return await Task.FromResult(false);
		}
	}
}
