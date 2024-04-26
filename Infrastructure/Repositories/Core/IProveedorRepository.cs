using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Core
{
    public interface IProveedorRepository
    {
        Task<List<Proveedor>> GetAllAsync(CancellationToken cancellationToken);
        Task<Proveedor> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Proveedor> CreateAsync(Proveedor proveedor, CancellationToken cancellationToken);
        Task<int> UpdateAsync(Proveedor proveedor, CancellationToken cancellationToken);
        Task<int> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
