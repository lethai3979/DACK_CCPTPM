namespace GoWheels_WebAPI.Utilities
{
    public class SearchFilterModel
    {
        public string? Company { get; set; }
        public string? CarType { get; set; }
        public int Seat { get; set; }
        public string? Gear { get; set; }
        public string? Fuel { get; set; }
        public bool HasDriver { get; set; }
    }
}
