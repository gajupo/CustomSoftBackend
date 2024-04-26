using Application.Commands;
using Infrastructure.Repositories.Core;
using MediatR;

namespace Application.Handlers.CommandHandlers
{
    public class DeleteProveedorCommandHandler : IRequestHandler<DeleteProveedorCommand, int>
    {
        private readonly IProveedorRepository _proveedorRespository;

        public DeleteProveedorCommandHandler(IProveedorRepository proveedorRepository)
        {
            _proveedorRespository = proveedorRepository;
        }
        public async Task<int> Handle(DeleteProveedorCommand request, CancellationToken cancellationToken)
        {
            var proveedorDetails = await _proveedorRespository.GetByIdAsync(request.Id, cancellationToken);
            if(proveedorDetails == null)
            {
                return default;
            }

            var rowsAffected = await _proveedorRespository.DeleteAsync(proveedorDetails.Id, cancellationToken);

            return rowsAffected;
        }
    }
}
