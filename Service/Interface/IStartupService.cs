namespace GoWheels_WebAPI.Service.Interface
{
    public interface IStartupService
    {
        Task UpdateBookingsOnStartup();
        Task UpdatePostOnStartup();

    }
}
