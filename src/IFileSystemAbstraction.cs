// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

namespace CommentLinks
{
	public interface IFileSystemAbstraction
	{
		bool FileExists(string filePath);
	}
}
