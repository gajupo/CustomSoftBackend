using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Archivo
    {
        public int Id { get; set; }
        [Required]
        public FileType TipoArchivo { get; set; }
        [Required]
        public string? Nombre { get; set; }
        public long Tamano { get; set; }
        public string? Extension { get; set; }
        public DateTime FechaCreacion { get; set; }
        [Required]
        public string? Ruta { get; set; }
    }
}
