using Domain.Core;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands
{
    public class AddInvoicesCommand: IRequest<bool>
    {
        public int ProveedorId { get; set; }
        public List<IFormFile>? files { get; set; }

        public string? DestinationFolder { get; set; }
    }
}
