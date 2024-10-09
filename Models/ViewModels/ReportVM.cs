using GoWheels_WebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class ReportVM
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public required int PostId { get; set; }
        public PostVM Post { get; set; } = null!;
        public required int ReportId { get; set; }
        public string? ReportName { get; set; }
    }
}
