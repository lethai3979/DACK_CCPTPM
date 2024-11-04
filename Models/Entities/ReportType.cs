namespace GoWheels_WebAPI.Models.Entities
{
    public class ReportType : BaseModel
    {
        public required string Name { get; set; }
        public required int ReportPoint { get; set; }
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
