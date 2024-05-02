using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class InvoicesDto
    {
        [Required]
        public int? ProveedorId { get; set; }
        [Required]
        public List<IFormFile>? files { get; set; }

        public string? DestinationFolder { get; set; }
    }
}
