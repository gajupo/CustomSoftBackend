using MediatR;

namespace Application.Commands
{
    public class DeleteProveedorCommand: IRequest<int>
    {
        public int Id { get; set; }
    }
}
