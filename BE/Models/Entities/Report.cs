using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoWheels_WebAPI.Models.Entities
{
    public class Report : BaseModel
    {
        public string? Content { get; set; }
        public required int PostId { get; set; }
        [ValidateNever]
        public Post Post { get; set; } = null!;
        public required int ReportTypeId { get; set; }
        [ValidateNever]
        public ReportType ReportType { get; set; } = null!;
    }
}
