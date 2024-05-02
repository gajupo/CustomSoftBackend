using FluentResults;
using MediatR;

namespace Application.Commands
{
    public class DeleteProveedorCommand: IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
}
