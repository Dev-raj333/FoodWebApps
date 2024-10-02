using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Implementation;
using FoodDonationWebApp.Services.Interfaces;
using FoodDonationWebApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodDonationWebApp.Controllers
{
    public class DonationController : Controller
    {
        private readonly IDonation _donation;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonationController(IDonation donation, UserManager<ApplicationUser> userManager) { 
            _donation = donation;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if (User.IsInRole("Admin"))
            {
                var donations = await _donation.GetAllDonationsAsync();
                return View(donations);
            }
            else if (User.IsInRole("Doner"))
            {
                // Donors can only view their own donations
                var donations = await _donation.GetDonationsByDonorIdAsync(userId);
                return View(donations);
            }

            // If the user is neither Admin nor Donor, redirect or return appropriate response
            return RedirectToAction("AccessDenied", "Account");
        }

        public async Task<IActionResult> Details(int id)
        {
            var donation = await _donation.GetDonationByIdAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }

        [Authorize(Roles = "Doner")]
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Retrieve the logged-in user's ID
            var viewModel = new DonationCreateVM
            {
                DonorID = userId
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Doner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonationCreateVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var donation = new Donation
                {
                    DonorID = viewModel.DonorID,
                    FoodType = (Models.FoodType)viewModel.FoodType,
                    Quantity = viewModel.Quantity,
                    ExpiraryDate = viewModel.ExpiraryDate,
                    Description = viewModel.Description,
                    Status = (Models.DonationStatus)viewModel.Status
                };

                await _donation.AddDonationAsync(donation);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete (int id)
        {
            var donation = await _donation.GetDonationByIdAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _donation.DeleteDonationAsync(id);
            return RedirectToAction("Index", "Donation");
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var donation = await _donation.GetDonationByIdAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }

        // Edit POST: Save the updated donation
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Donation donation)
        {
            if (id != donation.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _donation.UpdateDonationAsync(donation);
                return RedirectToAction(nameof(Index));
            }

            return View(donation);
        }


    }
}
