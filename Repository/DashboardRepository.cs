using RunWebAppGroup.Data;
using RunWebAppGroup.Interfaces;
using RunWebAppGroup.Models;

namespace RunWebAppGroup.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Club>> GetAllUserClubs()
        {
            var curUser = _httpContextAccessor.HttpContext?.User;
            var userClubs = _context.Clubs.Where(r => r.AppUserId == curUser.ToString());
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.ToString();
            var userRaces = _context.Races.Where(r => r.AppUserId == curUser.ToString());
            return userRaces.ToList();
        }

    }
}