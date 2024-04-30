namespace CustomSoft.WebApp.Server.Dtos
{
    public class InvoicesDto
    {
        public int ProveedorId { get; set; }
        public List<IFormFile>? files { get; set; }

        public string? DestinationFolder { get; set; }
    }
}
