using ContactProcessor.Models;
using System.Collections.Generic;

namespace ContactProcessor.Utilities.FileSystemHelper
{
    public interface IFileSystemHelper
    {
        string _uploadFolderFullPath { get; }

        string GetFullPath(string fileName);

        List<ContactViewModel> GetContactsFromFile(string fileFullPath);

        string GenerateUniqueFileName(string uploadedFileName);

        void EnsureDirectoryExists(string fileFullPath);
    }
}
