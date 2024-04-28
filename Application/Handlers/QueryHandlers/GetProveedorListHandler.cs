using Application.DTOs;
using Application.Queries;
using MediatR;
using Infrastructure.Repositories.Core;
using AutoMapper;
using System.Threading.Tasks;

namespace Application.Handlers.QueryHandlers
{
    public class GetProveedorListHandler : IRequestHandler<GetProveedoresListQuery, List<ProveedorDto>>
    {
        private readonly IProveedorRepository _proveedorRespository;
        private readonly IMapper _mapper;
        private readonly IDBArchivoRepository _dbArchivoRepository;

        public GetProveedorListHandler(IProveedorRepository proveedorRepository, IMapper mapper, IDBArchivoRepository dbArchivoRepository)
        {
            _proveedorRespository = proveedorRepository;
            _mapper = mapper;
            _dbArchivoRepository = dbArchivoRepository;
        }
        public async Task<List<ProveedorDto>> Handle(GetProveedoresListQuery request, CancellationToken cancellationToken)
        {
            var proveedores = await _proveedorRespository.GetAllAsync(cancellationToken);

            await Parallel.ForEachAsync(proveedores, async (proveedor, CancellationToken) =>
            {
                proveedor.Archivos = await _dbArchivoRepository.GetAllArchivosByProveedorAsync(proveedor.Id, cancellationToken);
            });

            return _mapper.Map<List<ProveedorDto>>(proveedores);
        }
    }
}
