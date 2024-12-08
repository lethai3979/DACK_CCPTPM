using GoWheels_WebAPI.Service.Interface;

namespace GoWheels_WebAPI.Service
{
    public class StatisticService
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _userId;

        public StatisticService(IInvoiceService invoiceService, IHttpContextAccessor contextAccessor, string userId)
        {
            _invoiceService = invoiceService;
            _contextAccessor = contextAccessor;
            _userId = userId;
        }
    }
}
