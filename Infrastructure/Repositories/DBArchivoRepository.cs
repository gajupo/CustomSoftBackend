using AutoMapper;
using Domain.Entities;
using Infrastructure.Config;
using Infrastructure.Repositories.Core;
using System.Threading;

namespace Infrastructure.Repositories
{
    public class DBArchivoRepository : IDBArchivoRepository
    {
        private readonly IDatabaseService _databaseService;
        private readonly IMapper _mapper;
        public DBArchivoRepository(IDatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        public async Task<List<Archivo>> GetAllArchivosByProveedorAsync(int proveedorId, CancellationToken cancellationToken)
        {
            var paramValues = new object[]
            {
                proveedorId,
            };
            var datatable = await _databaseService.ExecuteQueryFuncAsync("fn_get_archivos_by_proveedor($1)",
                cancellationToken,
                paramValues);

            if (datatable.Rows.Count > 0)
            {
                return _mapper.Map<List<Archivo>>(datatable.Rows);
            }

            return new List<Archivo>();
        }

        public Task<int> SaveInvoiceFile(Archivo archivo, CancellationToken cancellationToken)
        {

            var paramValues = new object[]
            {
                archivo.ProveedorId,
                archivo.TipoArchivo.ToString(),
                archivo.Nombre,
                archivo.Tamano,
                archivo.Extension,
                archivo.Ruta
            };

            return _databaseService.ExecuteNonQueryFuncAsync(
                "fn_insert_archivo_proveedor($1, $2, $3, $4, $5, $6)",
                cancellationToken,
                paramValues);
        }
    }
}
