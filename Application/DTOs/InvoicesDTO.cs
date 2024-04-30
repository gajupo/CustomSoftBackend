using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class InvoicesDto
    {
        public int? ProveedorId { get; set; }
        public List<IFormFile>? files { get; set; }

        public string? DestinationFolder { get; set; }
    }
}
