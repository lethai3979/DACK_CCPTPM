namespace GoWheels_WebAPI.Models.ViewModels
{
    public class DriverBookingVM
    {
        public DateTime RecieveDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal Total { get; set; }
        public bool IsCancel { get; set; }
        public DriverVM Driver { get; set; } = null!;   

    }
}
