using Domain.Core;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repositories.Core
{
    public interface IArchivoRepository
    {
        Task SaveInvoiceFile(IFormFile file, string filePath, CancellationToken cancellationToken);
    }
}
