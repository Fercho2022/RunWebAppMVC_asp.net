using Microsoft.AspNetCore.Mvc;
using RunWebAppGroup.Extensions;
using RunWebAppGroup.Interfaces;
using RunWebAppGroup.ViewModels;

namespace RunWebAppGroup.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRespository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(IDashboardRepository dashboardRespository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor
            )
        {
            _dashboardRespository = dashboardRespository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRespository.GetAllUserRaces();
            var userClubs = await _dashboardRespository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId= _httpContextAccessor.HttpContext.User.GetUserId();
            var user= await _dashboardRespository.GetUserById(curUserId);
            if (user == null) return View("Error");

            return View();
        }
    }
}
