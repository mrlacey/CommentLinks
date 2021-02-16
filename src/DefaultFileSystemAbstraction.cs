// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

namespace CommentLinks
{
    public class DefaultFileSystemAbstraction : IFileSystemAbstraction
    {
        public bool FileExists(string filePath)
        {
            return System.IO.File.Exists(filePath);
        }
    }
}
