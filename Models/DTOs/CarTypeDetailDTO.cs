namespace GoWheels_WebAPI.Models.DTOs
{
    public class CarTypeDetailDTO
    {
        public int Id { get; set; }
        public int CarTypeId { get; set; }
        public string? CarTypeName { get; set; }
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
