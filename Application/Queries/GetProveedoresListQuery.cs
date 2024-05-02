using Application.DTOs;
using Domain.Entities;
using FluentResults;
using MediatR;

namespace Application.Queries
{
    public class GetProveedoresListQuery: IRequest<Result<List<Proveedor>>>
    {

    }
}
