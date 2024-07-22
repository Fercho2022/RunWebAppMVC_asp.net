using Microsoft.EntityFrameworkCore;
using RunWebAppGroup.Data;
using RunWebAppGroup.Interfaces;
using RunWebAppGroup.Models;

namespace RunWebAppGroup.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly DataContext _context;

        public ClubRepository(DataContext context)
        {
            _context = context;
        }
        public bool Add(Club club)
        {
            _context.Clubs.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _context.Clubs.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAllClub()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Club> GetClubById(int id)
        {
            return await _context.Clubs.Include(a => a.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Club> GetClubByIdNoTracking(int id)
        {
            return await _context.Clubs.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save();
        }
    }
}
