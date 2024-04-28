using Domain.Entities;

namespace Infrastructure.Repositories.Core
{
    public interface IDBArchivoRepository: IArchivoRepository
    {
        Task<List<Archivo>> GetAllArchivosByProveedorAsync(int proveedorId, CancellationToken cancellationToken);
    }
}
