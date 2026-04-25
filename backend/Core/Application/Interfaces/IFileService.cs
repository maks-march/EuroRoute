using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IFileService
{
    public Task<string[]> SaveFiles(CancellationToken cancellationToken, params IFormFile[] files);
    public Task<bool> DeleteFiles(CancellationToken cancellationToken, params string[] paths);
}