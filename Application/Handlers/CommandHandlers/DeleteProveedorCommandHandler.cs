using Application.Commands;
using Common.Exceptions;
using FluentResults;
using Infrastructure.Repositories.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Application.Handlers.CommandHandlers
{
    public class DeleteProveedorCommandHandler : IRequestHandler<DeleteProveedorCommand, Result<int>>
    {
        private readonly IProveedorRepository _proveedorRespository;
        private readonly ILogger _logger;

        public DeleteProveedorCommandHandler(IProveedorRepository proveedorRepository, ILogger<DeleteProveedorCommand> logger)
        {
            _proveedorRespository = proveedorRepository;
            _logger = logger; 
        }
        public async Task<Result<int>> Handle(DeleteProveedorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var proveedorDetails = await _proveedorRespository.GetByIdAsync(request.Id, cancellationToken);
                if (proveedorDetails == null) return Result.Fail(new NotFoundError($"The given id = ${request.Id} was not found"));

                var rowsAffected = await _proveedorRespository.DeleteAsync(proveedorDetails.Id, cancellationToken);

                if (rowsAffected == 0) return Result.Fail(new BadRequestError($"Unable to delete the proveedor with the id = ${request.Id}"));

                return Result.Ok(rowsAffected);
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
