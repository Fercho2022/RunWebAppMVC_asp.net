using RunWebAppGroup.Models;

namespace RunWebAppGroup.Interfaces
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAllClub();

        Task<Club> GetClubById(int id);

        Task<Club> GetClubByIdNoTracking(int id);

        Task<IEnumerable<Club>> GetClubByCity(string city);

        bool Add(Club club);

        bool Update(Club club);

        bool Delete(Club club);

        bool Save();


    }
}
