using Application.DTOs;
using Domain.Entities;
using FluentResults;
using MediatR;

namespace Application.Queries
{
    public class GetProveedorByIdQuery: IRequest<Result<Proveedor>>
    {
        public int Id { get; set; }
    }
}
