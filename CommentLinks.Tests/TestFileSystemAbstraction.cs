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
