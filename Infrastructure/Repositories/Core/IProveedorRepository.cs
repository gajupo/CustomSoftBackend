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
        Task<List<Proveedor>> GetAllAsync();
        Task<Proveedor> GetByIdAsync(int id);
        Task CreateAsync(Proveedor proveedor);
        Task UpdateAsync(Proveedor proveedor);
        Task DeleteAsync(int id);
    }
}
