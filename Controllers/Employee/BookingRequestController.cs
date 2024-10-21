using AutoMapper;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Controllers.Employee
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Employee")]
    [ApiController]
    public class BookingRequestController : ControllerBase
    {
        private readonly BookingService _bookingService;
        private readonly InvoiceService _invoiceService;
        private readonly IMapper _mapper;

        public BookingRequestController(BookingService bookingService, InvoiceService invoiceService, IMapper mapper)
        {
            _bookingService = bookingService;
            _invoiceService = invoiceService;
            _mapper = mapper;
        }


        [HttpGet("GetAllCancelRequest")]
        public async Task<ActionResult<OperationResult>> GetAllCancelRequestAsync()
        {
            try
            {
                var requests = await _bookingService.GetAllCancelRequestAsync();
                var requestVMs = _mapper.Map<List<BookingVM>>(requests);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: requestVMs);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        } 


        [HttpPost("ExamineCancelBooking/{id}&&{isAccept}")]
        public async Task<ActionResult<OperationResult>> ExamineCancelBookingAsync(int id, bool isAccept)
        {
            try
            {
                await _invoiceService.RefundAsync(id, isAccept);
                return new OperationResult(true, "Cancellation request processed successfully", StatusCodes.Status200OK);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, dbEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (InvalidOperationException operationEx)
            {
                return new OperationResult(false, operationEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet("GetAllRefundInvoice")]
        public async Task<ActionResult<OperationResult>> GetAllRefundInvoiceAsync()
            => await _invoiceService.GetAllRefundInvoicesAsync();
    }
}
