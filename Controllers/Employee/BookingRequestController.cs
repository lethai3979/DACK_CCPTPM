using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoWheels_WebAPI.Controllers.Employee
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Employee")]
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


        [HttpPost("ExamineCancelBooking/{id}&&{isAccept}")]
        public async Task<ActionResult<OperationResult>> ExamineCancelBookingAsync(int id, bool isAccept)
            => await _invoiceService.RefundAsync(id, isAccept);

        [HttpGet("GetAllRefundInvoice")]
        public async Task<ActionResult<OperationResult>> GetAllRefundInvoiceAsync()
            => await _invoiceService.GetAllRefundInvoicesAsync();
    }
}
