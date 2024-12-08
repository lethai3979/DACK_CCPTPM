using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class ReportService : IReportService
    {
        private readonly IGenericRepository<Report> _reportRepository;
        private readonly IBookingService _bookingService;
        private readonly IInvoiceService _invoiceService;
        private readonly IUserService _userService;  
        private readonly IPostService _postService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;

        public ReportService(IGenericRepository<Report> reportRepository,
                                IBookingService bookingService,
                                IInvoiceService invoiceService,
                                IUserService userService,
                                IPostService postService,
                                IHttpContextAccessor httpContextAccessor)
        {
            _reportRepository = reportRepository;
            _bookingService = bookingService;
            _invoiceService = invoiceService;
            _userService = userService;
            _postService = postService;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                   .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public async Task<List<Report>> GetAllAsync()
            => await _reportRepository.GetAllAsync();

        public async Task<Report> GetByIdAsync(int id)
            => await _reportRepository.GetByIdAsync(id);

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
                    var post = await _postService.GetByIdAsync(report.PostId);
                    if(post.IsDisabled)
                    {
                        return;
                    }    
                    await _postService.DisablePostByIdAsync(post.Id);
                    var bookings = await _bookingService.GetAllUnRecieveBookingsByPostIdAsync(post.Id);
                    foreach (var booking in bookings)
                    {
                        await _bookingService.ExamineCancelBookingRequestAsync(booking, true);
                        if (booking.IsPay)
                        {
                            await _invoiceService.RefundReportedBookingAsync(booking);
                        }     
                    }
                    await _userService.UpdateUserReportPointAsync(report.Post.UserId!, report.ReportType.ReportPoint);
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
    }
}
