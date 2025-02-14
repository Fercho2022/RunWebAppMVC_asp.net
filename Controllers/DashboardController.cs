﻿using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunWebAppGroup.Extensions;
using RunWebAppGroup.Interfaces;
using RunWebAppGroup.Models;
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

        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editVM, ImageUploadResult photoResult) { 
        
            user.Id= editVM.Id;
            user.Pace= editVM.Pace;
            user.Mileage= editVM.Mileage;
            user.ProfileImageUrl=photoResult.Url.ToString();
            user.City= editVM.City;
            user.State= editVM.State;
        
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
            var editUserViewModel = new EditUserDashboardViewModel()
            {

                Id = user.Id,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State

            };

            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserprofile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit profile");
                return View("EditUserProfile", editVM);
            }

            AppUser user= await _dashboardRespository.GetUserByIdNoTracking(editVM.Id);

            if(user.ProfileImageUrl=="" || user.ProfileImageUrl== null){

                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, photoResult);
                _dashboardRespository.Update(user);
                return RedirectToAction("Index");
            
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                MapUserEdit(user, editVM, photoResult);

                _dashboardRespository.Update(user);

                return RedirectToAction("Index");
            }
        }
    }
}
