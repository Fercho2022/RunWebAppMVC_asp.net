﻿using Microsoft.AspNetCore.Mvc;
using RunWebAppGroup.Interfaces;
using RunWebAppGroup.ViewModels;

namespace RunWebAppGroup.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository) {
            _userRepository = userRepository;
        }


       
        public async Task<IActionResult> Index()
        {
           var users= await _userRepository.GetAllUsers();

            List<UserViewModel> result= new List<UserViewModel>();

            foreach (var user in users)
            {
                var userViewModel = new UserViewModel()

                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Mileage = user.Mileage,
                    ProfileImageUrl = user.ProfileImageUrl


                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserById(id);

            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl=user.ProfileImageUrl
                Address=
            };
            return View(userDetailViewModel);

        }


    }
}
