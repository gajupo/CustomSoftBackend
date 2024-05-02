using Application.Queries;
using MediatR;
using Infrastructure.Repositories.Core;
using FluentResults;
using Npgsql;
using Domain.Entities;
using Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.QueryHandlers
{
    public class GetProveedorListHandler : IRequestHandler<GetProveedoresListQuery, Result<List<Proveedor>>>
    {
        private readonly IProveedorRepository _proveedorRespository;
        private readonly IDBArchivoRepository _dbArchivoRepository;
        private readonly ILogger _logger;

        public GetProveedorListHandler(IProveedorRepository proveedorRepository, IDBArchivoRepository dbArchivoRepository,
            ILogger<GetProveedorListHandler> logger)
        {
            _proveedorRespository = proveedorRepository;
            _dbArchivoRepository = dbArchivoRepository;
            _logger = logger;
        }
        public async Task<Result<List<Proveedor>>> Handle(GetProveedoresListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var proveedores = await _proveedorRespository.GetAllAsync(cancellationToken);

                await Parallel.ForEachAsync(proveedores, async (proveedor, CancellationToken) =>
                {
                    proveedor.Archivos = await _dbArchivoRepository.GetAllArchivosByProveedorAsync(proveedor.Id, cancellationToken);
                });

                return Result.Ok(proveedores);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Something bad happened when trying to get the proveedores");
                return Result.Fail(new DatabaseError("Something bad happened when trying to get the proveedores")
                    .CausedBy(ex));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "UnExpected error please review the log for more details");
                return Result.Fail(new UnExpectedError("UnExpected error please review the log for more details")
                    .CausedBy(ex));
            }
        }
    }
}
