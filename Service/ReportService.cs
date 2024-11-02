using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class ReportService
    {
        private readonly ReportRepository _reportRepository;
        private readonly BookingService _bookingService;
        private readonly InvoiceService _invoiceService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly string _userId;

        public ReportService(ReportRepository reportRepository, 
                                BookingService bookingService,
                                InvoiceService invoiceService,
                                IHttpContextAccessor httpContextAccessor, 
                                IMapper mapper)
        {
            _reportRepository = reportRepository;
            _bookingService = bookingService;
            _invoiceService = invoiceService;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                   .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
            _mapper = mapper;
        }

        public async Task CreateReportAsync(Report report)
        {
            try
            {
                report.IsDeleted = false;
                report.CreatedById = _userId;
                report.CreatedOn = DateTime.Now;
                await _reportRepository.AddAsync(report);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.InnerException!.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ConfirmReportAsync(int id, bool isAccept)
        {
            try
            {
                var report = await _reportRepository.GetByIdAsync(id);
                report.IsDeleted = true;
                report.ModifiedById = _userId;
                report.ModifiedOn = DateTime.Now;
                await _reportRepository.UpdateAsync(report);
                if (isAccept)
                {
                    var bookings = await _bookingService.GetAllUnRecieveBookingsByPostIdAsync(report.PostId);
                    foreach (var booking in bookings)
                    {
                        await _bookingService.CancelReportedBookingsAsync(booking);
                        if (booking.IsPay)
                        {
                            await _invoiceService.RefundReportedBookingAsync(booking);
                        }     
                    }
                }
            }
            catch (NullReferenceException nullEx)
            {
                throw new NullReferenceException(nullEx.Message);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.InnerException!.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OperationResult> GetAllAsync()
        {
            var reports = await _reportRepository.GetAllAsync();
            if(reports.Count == 0)
            {
                return new OperationResult(false, "List empty", StatusCodes.Status404NotFound);
            } 
            var reportVMs = _mapper.Map<List<ReportVM>>(reports);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: reportVMs);
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            if (report == null)
            {
                return new OperationResult(false, "Report not found", StatusCodes.Status404NotFound);
            }
            var reportVM = _mapper.Map<ReportVM>(report);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: reportVM);
        }

    }
}
