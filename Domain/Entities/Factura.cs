using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Factura
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public DateTime FechaEmision { get; set; }
        public List<Archivo> Archivos { get; set; } = new List<Archivo>();
    }
}
