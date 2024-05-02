using Domain.Entities;
using FluentResults;
using MediatR;

namespace Application.Commands
{
    public class UpdateProveedorCommand: IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime FechaAlta { get; set; }
        public string? RFC { get; set; }
        public string? Direccion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<Archivo>? Archivos { get; set; } = new List<Archivo>();
    }
}
