using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class BaseModelDTO
    {
        [JsonProperty(Order = -2)]
        public int Id { get; set; }

        [JsonProperty(Order = -2)]
        public string? CreatedById { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        [JsonProperty(Order = -2)]
        public DateTime CreatedOn { get; set; }

        [JsonProperty(Order = -2)]
        public string? ModifiedById { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        [JsonProperty(Order = -2)]
        public DateTime? ModifiedOn { get; set; }

        [JsonProperty(Order = -2)]
        [Required]
        public bool IsDeleted { get; set; }
    }
}
