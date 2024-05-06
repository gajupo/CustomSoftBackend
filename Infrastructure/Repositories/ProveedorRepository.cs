using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Infrastructure.Config;
using Infrastructure.Repositories.Core;
using System.Data;

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

        public async Task<(List<Proveedor>, int)> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var paramValues = new object[]
            {
                pageSize,
                pageNumber
            };
            var totalRows = await _databaseService.ExecuteNonQueryFuncAsync("get_total_rows_proveedores()", cancellationToken, paramValues);
            var datatable = await _databaseService.ExecuteQueryFuncAsync("sp_get_all_proveedores($1,$2)", cancellationToken, paramValues);
            
            if (datatable.Rows.Count > 0)
            {
                return (_mapper.Map<List<Proveedor>>(datatable.Rows), totalRows);
            }
            
            return (new List<Proveedor>(), 0);
        }

        public async Task<Proveedor> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var datatable = await _databaseService.ExecuteQueryFuncAsync("sp_get_proveedor($1)", cancellationToken, id);
            if (datatable.Rows.Count > 0)
            {
                return _mapper.Map<Proveedor>(datatable.Rows[0]);
            }
            return null;
        }

        public async Task<Proveedor> CreateAsync(Proveedor proveedor, CancellationToken cancellationToken)
        {
            var paramValues = new object[]
            {
                proveedor.Nombre,
                DateTime.Now,
                proveedor.RFC,
                proveedor.Direccion,
                proveedor.Activo,
                DateTime.Now
            };
            var id = await _databaseService.ExecuteNonQueryFuncAsync(
                "sp_insert_proveedor($1, $2, $3, $4, $5, $6)",
                cancellationToken,
                paramValues);

            proveedor.Id = id;

            return proveedor;
        }

        public Task<int> UpdateAsync(Proveedor proveedor, CancellationToken cancellationToken)
        {
            var paramValues = new object[]
            {
                proveedor.Id,
                proveedor.Nombre,
                proveedor.RFC,
                proveedor.Direccion,
                proveedor.Activo
            };

            return _databaseService.ExecuteNonQuerySPAsync("sp_update_proveedor", cancellationToken,
                paramValues);
        }

        public Task<int> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            return _databaseService.ExecuteNonQuerySPAsync("sp_delete_proveedor", cancellationToken, id);
        }

        public async Task<DataTable> GetAllByCreatedRangeDateAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            var paramValues = new object[]
            {
                startDate,
                endDate
            };
            var datatable = await _databaseService.ExecuteQueryFuncAsync("sp_get_all_proveedores_by_date_range($1,$2)", cancellationToken, paramValues);

            if (datatable.Rows.Count > 0)
            {
                return datatable;
            }

            return new DataTable();
        }
    }
}
