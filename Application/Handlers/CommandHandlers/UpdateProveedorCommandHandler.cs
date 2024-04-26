using Application.Commands;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.CommandHandlers
{
    public class UpdateProveedorCommandHandler : IRequestHandler<UpdateProveedorCommand, int>
    {
        private readonly IProveedorRepository _proveedorRespository;
        private readonly IMapper _mapper;

        public UpdateProveedorCommandHandler(IProveedorRepository proveedorRepository, IMapper mapper)
        {
            _proveedorRespository = proveedorRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(UpdateProveedorCommand request, CancellationToken cancellationToken)
        {
            var proveedorDetails = await _proveedorRespository.GetByIdAsync(request.Id, cancellationToken);
            if(proveedorDetails == null)
            {
                return default;
            }

            var rowsAffected = await _proveedorRespository.UpdateAsync(_mapper.Map<Proveedor>(request), cancellationToken);

            return rowsAffected;
        }
    }
}
