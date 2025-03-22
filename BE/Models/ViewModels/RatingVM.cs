namespace GoWheels_WebAPI.Models.ViewModels
{
    public class RatingVM : BaseModelVM
    {
        public string? Comment { get; set; }

        public float Point { get; set; }
        public string? UserName { get; set; }
        public string? UserImage {  get; set; }

    }
}
