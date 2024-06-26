﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Proveedor
    {
        public int Id { get; set; }
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public DateTime FechaAlta { get; set; }
        [Required]
        public string? RFC { get; set; }
        [Required]
        public string? Direccion { get; set; }
        public bool Activo { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public List<Archivo>? Archivos { get; set; } = new List<Archivo>();
    }
}
