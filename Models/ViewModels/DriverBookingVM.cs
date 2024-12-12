namespace GoWheels_WebAPI.Models.ViewModels
{
    public class DriverBookingVM : BaseModelVM
    {
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public decimal Total { get; set; }
        public bool IsCancel { get; set; }
        public DriverVM Driver { get; set; } = null!;

        //public List<InvoiceVM> Invoices { get; set; } = new List<InvoiceVM>();
    }
}
