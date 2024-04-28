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
        private readonly IDBArchivoRepository _dbArchivoRepository;

        public GetProveedorByIdHandler(IProveedorRepository proveedorRepository, IMapper mapper, IDBArchivoRepository dbArchivoRepository)
        {
            _mapper = mapper;
            _proveedorRespository = proveedorRepository;
            _dbArchivoRepository = dbArchivoRepository;
        }
        public async Task<ProveedorDto> Handle(GetProveedorByIdQuery request, CancellationToken cancellationToken)
        {
            var proveedor = await _proveedorRespository.GetByIdAsync(request.Id, cancellationToken);

            proveedor.Archivos = await _dbArchivoRepository.GetAllArchivosByProveedorAsync(proveedor.Id, cancellationToken);

            return _mapper.Map<ProveedorDto>(proveedor);
        }
    }
}
