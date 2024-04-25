using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Archivo
    {
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public Factura Factura { get; set; }
        public string Nombre { get; set; }
        public long Tamano { get; set; }
        public string Extension { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Ruta { get; set; }
    }
}
