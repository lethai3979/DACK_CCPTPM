using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Infrastructure;
using GoWheels_WebAPI.Models.Entities;
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
        private readonly ApplicationDbContext _context;
        private readonly string _userId;

        public ReportService(IGenericRepository<Report> reportRepository,
                                IBookingService bookingService,
                                IInvoiceService invoiceService,
                                IUserService userService,
                                IPostService postService,
                                IHttpContextAccessor httpContextAccessor,
                                ApplicationDbContext context)
        {
            _reportRepository = reportRepository;
            _bookingService = bookingService;
            _invoiceService = invoiceService;
            _userService = userService;
            _postService = postService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _userId = _httpContextAccessor.HttpContext?.User?
                   .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public List<Report> GetAll()
            => _reportRepository.GetAll();

        public Report GetById(int id)
            => _reportRepository.GetById(id);

        public void CreateReport(Report report)
        {
            try
            {
                report.IsDeleted = false;
                report.CreatedById = _userId;
                report.CreatedOn = DateTime.Now;
                _reportRepository.Add(report);
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
                    
        public async Task ConfirmReport(int id, bool isAccept)
        {
            using (var transaction = new TransactionManager(_context))
            {
                transaction.BeginTransaction();
                try
                {
                    var report = _reportRepository.GetById(id);
                    report.IsDeleted = true;
                    report.ModifiedById = _userId;
                    report.ModifiedOn = DateTime.Now;
                    _reportRepository.Update(report);
                    if (!isAccept)
                    {
                        return;
                    }
                    var post = _postService.GetById(report.PostId);
                    if (post.IsDisabled)
                    {
                        return;
                    }
                    _postService.DisablePostById(post.Id);
                    var bookings = _bookingService.GetAllUnRecieveBookingsByPostId(post.Id);
                    foreach (var booking in bookings)
                    {
                        await _bookingService.ExamineCancelBookingRequestAsync(booking, true);
                        if (booking.IsPay)
                        {
                            _invoiceService.RefundReportedBooking(booking);
                        }
                    }
                    await _userService.UpdateUserReportPointAsync(report.Post.UserId!, report.ReportType.ReportPoint);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
