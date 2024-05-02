using Application.Commands;
using AutoMapper;
using Common.Exceptions;
using Domain.Entities;
using FluentResults;
using Infrastructure.Repositories.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Application.Handlers.CommandHandlers
{
    public class UpdateProveedorCommandHandler : IRequestHandler<UpdateProveedorCommand, Result<int>>
    {
        private readonly IProveedorRepository _proveedorRespository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UpdateProveedorCommandHandler(IProveedorRepository proveedorRepository, IMapper mapper,
            ILogger<UpdateProveedorCommand> logger)
        {
            _proveedorRespository = proveedorRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Result<int>> Handle(UpdateProveedorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var proveedorDetails = await _proveedorRespository.GetByIdAsync(request.Id, cancellationToken);
                if (proveedorDetails == null) return Result.Fail(new NotFoundError($"The given id = ${request.Id} was not found"));

                var rowsAffected = await _proveedorRespository.UpdateAsync(_mapper.Map<Proveedor>(request), cancellationToken);
                if(rowsAffected == 0) return Result.Fail(new BadRequestError($"Unable to update the proveedor with the id = ${request.Id}"));

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
