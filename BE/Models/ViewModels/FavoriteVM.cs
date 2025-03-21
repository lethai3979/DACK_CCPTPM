namespace GoWheels_WebAPI.Models.ViewModels
{
    public class FavoriteVM
    {
        public int Id { get; set; }
        public PostVM Post { get; set; } = null!;
    }
}
