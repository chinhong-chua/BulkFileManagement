using BulkFileManagementApi.Models;

namespace BulkFileManagementApi.Services
{
    public interface IFileManipulationService
    {
        Task<IEnumerable<string>> ManipulateFilesAsync(FileManipulationRequest request);
    }
}
