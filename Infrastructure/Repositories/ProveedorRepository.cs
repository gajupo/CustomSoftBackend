using AutoMapper;
using Domain.Entities;
using Infrastructure.Config;
using Infrastructure.Repositories.Core;

namespace Infrastructure.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMapper _mapper;

        public ProveedorRepository(IDatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        public async Task<List<Proveedor>> GetAllAsync()
        {
            var dataSet = await _databaseService.ExecuteQueryAsync("sp_get_all_proveedores");
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                return _mapper.Map<List<Proveedor>>(dataSet.Tables[0]);
            }
            return new List<Proveedor>();
        }

        public async Task<Proveedor> GetByIdAsync(int id)
        {
            var dataSet = await _databaseService.ExecuteQueryAsync("sp_get_proveedor_by_id", ("id", id));
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                return _mapper.Map<Proveedor>(dataSet.Tables[0].Rows[0]);
            }
            return null;
        }

        public async Task CreateAsync(Proveedor proveedor)
        {
            await _databaseService.ExecuteNonQueryAsync("sp_create_proveedor",
                ("nombre", proveedor.Nombre),
                ("fechaAlta", proveedor.FechaAlta),
                ("rfc", proveedor.RFC),
                ("direccion", proveedor.Direccion),
                ("activo", proveedor.Activo),
                ("fechaCreacion", proveedor.FechaCreacion));
        }

        public async Task UpdateAsync(Proveedor proveedor)
        {
            await _databaseService.ExecuteNonQueryAsync("sp_update_proveedor",
                ("id", proveedor.Id),
                ("nombre", proveedor.Nombre),
                ("rfc", proveedor.RFC),
                ("direccion", proveedor.Direccion),
                ("activo", proveedor.Activo),
                ("fechaModificacion", proveedor.FechaModificacion));
        }

        public async Task DeleteAsync(int id)
        {
            await _databaseService.ExecuteNonQueryAsync("sp_delete_proveedor", ("id", id));
        }
    }
}
