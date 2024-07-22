using RunWebAppGroup.Data.Enum;
using RunWebAppGroup.Models;

namespace RunWebAppGroup.ViewModels
{
    public class EditRaceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int AddressId { get; set; }


        public Address Address { get; set; }
        public IFormFile Image { get; set; }

        public string? Url { get; set; }
        public RaceCategory RaceCategory { get; set; }
    }
}
