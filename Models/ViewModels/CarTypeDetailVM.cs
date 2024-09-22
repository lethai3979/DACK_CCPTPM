namespace GoWheels_WebAPI.Models.ViewModels
{
    public class CarTypeDetailVM
    {
        public int Id { get; set; }
        public int CarTypeId { get; set; }
        public string? CarTypeName { get; set; }
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
