using Application.DTOs;
using Application.Queries;
using MediatR;
using Infrastructure.Repositories.Core;
using AutoMapper;

namespace Application.Handlers.QueryHandlers
{
    public class GetProveedorListHandler : IRequestHandler<GetProveedoresListQuery, List<ProveedorDto>>
    {
        private readonly IProveedorRepository _proveedorRespository;
        private readonly IMapper _mapper;

        public GetProveedorListHandler(IProveedorRepository proveedorRepository, IMapper mapper)
        {
            _proveedorRespository = proveedorRepository;
            _mapper = mapper;
        }
        public async Task<List<ProveedorDto>> Handle(GetProveedoresListQuery request, CancellationToken cancellationToken)
        {
            var proveedores = await _proveedorRespository.GetAllAsync(cancellationToken);

            return _mapper.Map<List<ProveedorDto>>(proveedores);
        }
    }
}
