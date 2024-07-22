using Microsoft.AspNetCore.Mvc;
using RunWebAppGroup.Extensions;
using RunWebAppGroup.Interfaces;
using RunWebAppGroup.Models;
using RunWebAppGroup.ViewModels;


namespace RunWebAppGroup.Controllers
{
    public class ClubController : Controller
    {

        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {

            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAllClub();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var club = await _clubRepository.GetClubById(id);
            return View(club);
        }

        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createViewModel = new CreateClubViewModel
            {
                AppUserId = curUserId,
            };
            return View(createViewModel);


        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);


                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId = clubVM.AppUserId,
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,

                    }

                };

                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(clubVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetClubById(id);
            if (club == null) return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = (int)club.AddressId,
                Address = club.Address,
                Url = club.Image,
                ClubCategory = club.ClubCategory,

            };
            return View(clubVM);

        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            var userClub = await _clubRepository.GetClubByIdNoTracking(id);

            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVM);
                }

                var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);

                var Club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address,
                };

                _clubRepository.Update(Club);

                return RedirectToAction("Index");

            }

            else
            {
                return View(clubVM);
            }
        }
    }

}

