using Microsoft.AspNetCore.Mvc;

namespace GoWheels_WebAPI.Repositories
{
    public class SalePromotionTypeRepository : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
