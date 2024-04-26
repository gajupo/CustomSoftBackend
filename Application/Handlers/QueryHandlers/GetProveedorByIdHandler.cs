using Application.DTOs;
using Application.Queries;
using AutoMapper;
using Infrastructure.Repositories.Core;
using MediatR;

namespace Application.Handlers.QueryHandlers
{
    public class GetProveedorByIdHandler : IRequestHandler<GetProveedorByIdQuery, ProveedorDto>
    {
        private readonly IProveedorRepository _proveedorRespository;
        private readonly IMapper _mapper;

        public GetProveedorByIdHandler(IProveedorRepository proveedorRepository, IMapper mapper)
        {
            _mapper = mapper;
            _proveedorRespository = proveedorRepository;
        }
        public async Task<ProveedorDto> Handle(GetProveedorByIdQuery request, CancellationToken cancellationToken)
        {
            var proveedor = await _proveedorRespository.GetByIdAsync(request.Id, cancellationToken);

            return _mapper.Map<ProveedorDto>(proveedor);
        }
    }
}
