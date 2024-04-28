using Domain.Entities;

namespace Infrastructure.Repositories.Core
{
    public interface IArchivoRepository
    {
        Task<int> SaveInvoiceFile(Archivo archivo, CancellationToken cancellationToken);
    }
}
