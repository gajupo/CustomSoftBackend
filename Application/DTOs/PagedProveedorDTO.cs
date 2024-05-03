namespace Application.DTOs
{
    public class PagedProveedorDTO
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<ProveedorDto>? Items { get; set; }
    }
}
