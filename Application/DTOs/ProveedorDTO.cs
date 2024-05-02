using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class ProveedorDto
    {
        public int Id { get; set; }
        [Required]
        public string? Nombre { get; set; }
        public DateTime? FechaAlta { get; set; }
        [Required]
        public string? RFC { get; set; }
        [Required]
        public string? Direccion { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; }
        public List<Archivo>? Archivos { get; set; } = new List<Archivo>();
    }
}
