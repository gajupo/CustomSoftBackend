using Domain.Core;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands
{
    public class AddInvoicesCommand: IRequest<Result<bool>>
    {
        public int ProveedorId { get; set; }
        public List<IFormFile>? files { get; set; }

        public string? DestinationFolder { get; set; }
    }
}
