using Domain.Entities;
using FluentResults;
using MediatR;

namespace Application.Queries
{
    public class GetProveedoresListQuery: IRequest<Result<(List<Proveedor>, int)>>
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
