namespace GoWheels_WebAPI.Models.Entities
{
    public class DriverBooking : BaseModel  
    {
        public required DateTime PickUpDate { get; set; }
        public required DateTime DropOffDate { get; set; }
        public decimal Total { get; set; }
        public bool IsCancel { get; set; } = false!;
        public required int DriverId { get; set; }
        public Driver Driver { get; set; } = null!;
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
