namespace GoWheels_WebAPI.Models.ViewModels
{
    public class ReportTypeVM : BaseModelVM
    {
        public required string Name { get; set; }
        public required int ReportPoint { get; set; }
        public ICollection<ReportVM> ReportVMs { get; set; } = new List<ReportVM>();     
    }
}
