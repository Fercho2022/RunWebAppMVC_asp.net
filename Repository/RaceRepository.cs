using Microsoft.EntityFrameworkCore;
using RunWebAppGroup.Data;
using RunWebAppGroup.Interfaces;
using RunWebAppGroup.Models;

namespace RunWebAppGroup.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly DataContext _context;

        public RaceRepository(DataContext context)
        {
            _context = context;
        }
        public bool Add(Race race)
        {
            _context.Races.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Races.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAllRace()
        {
            return await _context.Races.ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetRaceByCity(string city)
        {
            return await _context.Races.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Race> GetRaceById(int id)
        {
            return await _context.Races.Include(a => a.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Race> GetRaceByIdNoTracking(int id)
        {
            return await _context.Races.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }
    }
}

