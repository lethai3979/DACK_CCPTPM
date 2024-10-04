using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoWheels_WebAPI.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingRequestController : ControllerBase
    {
        private readonly BookingService _bookingService;
        private readonly InvoiceService _invoiceService;

        public BookingRequestController(BookingService bookingService, InvoiceService invoiceService)
        {
            _bookingService = bookingService;
            _invoiceService = invoiceService;
        }


        [HttpGet("GetAllCancelRequest")]
        public async Task<ActionResult<OperationResult>> GetAllCancelRequestAsync()
            => await _bookingService.GetAllCancelRequestAsync();



/*        public async Task<ActionResult<OperationResult>> ExamineCancelBooking(int id, bool isAccept)
            => await _bookingService.ExamineCancelBookingRequest(id, isAccept);*/
        
    }
}
