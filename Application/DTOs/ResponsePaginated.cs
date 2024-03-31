namespace GestionSucursalesAPI.Application.DTOs
{
    public class ResponsePaginated
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
