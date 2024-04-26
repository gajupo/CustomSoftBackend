using Application.DTOs;
using MediatR;

namespace Application.Queries
{
    public class GetProveedorByIdQuery: IRequest<ProveedorDto>
    {
        public int Id { get; set; }
    }
}
