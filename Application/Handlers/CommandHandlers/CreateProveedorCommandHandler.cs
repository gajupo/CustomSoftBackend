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
    public class CreateProveedorCommandHandler : IRequestHandler<CreateProveedorCommand, ProveedorDto>
    {
        private readonly IProveedorRepository _proveedorRespository;
        private readonly IMapper _mapper;

        public CreateProveedorCommandHandler(IProveedorRepository proveedorRepository, IMapper mapper)
        {
            _proveedorRespository = proveedorRepository;
            _mapper = mapper;
        }
        public async Task<ProveedorDto> Handle(CreateProveedorCommand request, CancellationToken cancellationToken)
        {
            var created = await _proveedorRespository.CreateAsync(_mapper.Map<Proveedor>(request), cancellationToken);

            return _mapper.Map<ProveedorDto>(created);
        }
    }
}
