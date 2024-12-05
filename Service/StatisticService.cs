namespace GoWheels_WebAPI.Service
{
    public class StatisticService
    {
        private readonly InvoiceService _invoiceService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _userId;

        public StatisticService(InvoiceService invoiceService, IHttpContextAccessor contextAccessor, string userId)
        {
            _invoiceService = invoiceService;
            _contextAccessor = contextAccessor;
            _userId = userId;
        }
    }
}
