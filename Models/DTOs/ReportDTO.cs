namespace GoWheels_WebAPI.Models.DTOs
{
    public class ReportDTO
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int PostId { get; set; }
        public int ReportTypeId { get; set; }
    }
}
