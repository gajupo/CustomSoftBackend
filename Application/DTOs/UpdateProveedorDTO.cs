using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class UpdateProveedorDto
    {
        [Required]
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime? FechaAlta { get; set; }
        public string? RFC { get; set; }
        public string? Direccion { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; }
        public List<Archivo>? Archivos { get; set; } = new List<Archivo>();
    }
}
