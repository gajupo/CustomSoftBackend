using Application.Commands;
using Application.DTOs;
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
    public class CreateProveedorCommandHandler : IRequestHandler<CreateProveedorCommand, Result<Proveedor>>
    {
        private readonly IProveedorRepository _proveedorRespository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CreateProveedorCommandHandler(IProveedorRepository proveedorRepository, IMapper mapper,
            ILogger<CreateProveedorCommandHandler> logger)
        {
            _proveedorRespository = proveedorRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Result<Proveedor>> Handle(CreateProveedorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var created = await _proveedorRespository.CreateAsync(_mapper.Map<Proveedor>(request), cancellationToken);

                return Result.Ok(created);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "Something bad happened when trying to create the proveedor");
                return Result.Fail(new DatabaseError("Something bad happened when trying to create the proveedor")
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
