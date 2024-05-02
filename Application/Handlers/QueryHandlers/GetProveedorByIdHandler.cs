using Application.Queries;
using AutoMapper;
using Common.Exceptions;
using Domain.Entities;
using FluentResults;
using Infrastructure.Repositories.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Application.Handlers.QueryHandlers
{
    public class GetProveedorByIdHandler : IRequestHandler<GetProveedorByIdQuery, Result<Proveedor>>
    {
        private readonly IProveedorRepository _proveedorRespository;
        private readonly IDBArchivoRepository _dbArchivoRepository;
        private readonly ILogger _logger;

        public GetProveedorByIdHandler(IProveedorRepository proveedorRepository,
            IDBArchivoRepository dbArchivoRepository, ILogger<GetProveedorByIdHandler> logger)
        {
            _proveedorRespository = proveedorRepository;
            _dbArchivoRepository = dbArchivoRepository;
            _logger = logger;
        }
        public async Task<Result<Proveedor>> Handle(GetProveedorByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var proveedor = await _proveedorRespository.GetByIdAsync(request.Id, cancellationToken);

                if (proveedor == null) return Result.Fail(new NotFoundError($"The given id = ${request.Id} was not found"));

                proveedor.Archivos = await _dbArchivoRepository.GetAllArchivosByProveedorAsync(proveedor.Id, cancellationToken);


                return Result.Ok(proveedor);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Something bad happened when trying to get the proveedores");
                return Result.Fail(new DatabaseError("Something bad happened when trying to get the proveedores")
                    .CausedBy(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UnExpected error please review the log for more details");
                return Result.Fail(new UnExpectedError("UnExpected error please review the log for more details")
                    .CausedBy(ex));
            }
            
        }
    }
}
