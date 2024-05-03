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
    public class GetProveedorListHandler : IRequestHandler<GetProveedoresListQuery, Result<(List<Proveedor>, int)>>
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
        public async Task<Result<(List<Proveedor>, int)>> Handle(GetProveedoresListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var (proveedores, totalRows) = await _proveedorRespository.GetAllAsync(request.pageNumber, request.pageSize,cancellationToken);

                ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 3 };
                await Parallel.ForEachAsync(proveedores, parallelOptions, async (proveedor, CancellationToken) =>
                {
                    proveedor.Archivos = await _dbArchivoRepository.GetAllArchivosByProveedorAsync(proveedor.Id, cancellationToken);
                });

                return Result.Ok((proveedores, totalRows));
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
