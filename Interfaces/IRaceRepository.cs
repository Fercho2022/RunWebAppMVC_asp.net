using RunWebAppGroup.Models;

namespace RunWebAppGroup.Interfaces
{
    public interface IRaceRepository
    {

        Task<IEnumerable<Race>> GetAllRace();

        Task<Race> GetRaceById(int id);

        Task<Race> GetRaceByIdNoTracking(int id);

        Task<IEnumerable<Race>> GetRaceByCity(string city);

        bool Add(Race race);

        bool Update(Race race);

        bool Delete(Race race);

        bool Save();
    }
}
