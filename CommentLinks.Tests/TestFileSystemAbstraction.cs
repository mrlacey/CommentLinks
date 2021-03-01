// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

namespace CommentLinks.Tests
{
    public class TestFileSystemAbstraction : IFileSystemAbstraction
    {
        public bool FileExists(string filePath)
        {
            return true;
        }
    }
}
