using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodDonationWebApp.Controllers
{
    public class PickupRequestController : Controller
    {
        private readonly IPickupRequestRepository _pickupRequestRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PickupRequestController(IPickupRequestRepository pickupRequestRepository, UserManager<ApplicationUser> userManager)
        {
            _pickupRequestRepository = pickupRequestRepository;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var pickupRequests = await _pickupRequestRepository.GetAllPickupRequestsAsync();
            return View(pickupRequests);
        }

        public async Task<IActionResult> Details(int id)
        {
            var pickupRequest = await _pickupRequestRepository.GetPickupRequestByIdAsync(id);
            if (pickupRequest == null)
            {
                return NotFound();
            }
            return View(pickupRequest);
        }

        // Create (GET): Display form for creating a new pickup request, with optional donationId
        public async Task<IActionResult> Create(int donationId)
        {
            var volunteers = await _userManager.GetUsersInRoleAsync("Volunture");
            ViewBag.Volunteers = volunteers.Select(v => new { v.Id, v.FirstName, v.LastName }).ToList();

            // Optionally pass donationId to the view
            ViewBag.DonationId = donationId;

            return View();
        }

        // Create (POST): Handle form submission for creating a new pickup request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PickupRequest pickupRequest, int donationId)
        {
            if (ModelState.IsValid)
            {
                // Assign the donationId to the pickup request (if applicable)
                pickupRequest.DonationID = donationId;

                await _pickupRequestRepository.AddPickupRequestAsync(pickupRequest);
                return RedirectToAction("index", "Donation");
            }

            var volunteers = await _userManager.GetUsersInRoleAsync("Volunteer");
            ViewBag.Volunteers = volunteers.Select(v => new { v.Id, v.FirstName, v.LastName}).ToList();
            return View(pickupRequest);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var pickupRequest = await _pickupRequestRepository.GetPickupRequestByIdAsync(id);
            if (pickupRequest == null)
            {
                return NotFound();
            }
            return View(pickupRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PickupRequest pickupRequest)
        {
            if (id != pickupRequest.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _pickupRequestRepository.UpdatePickupRequestAsync(pickupRequest);
                return RedirectToAction(nameof(Index));
            }

            return View(pickupRequest);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var pickupRequest = await _pickupRequestRepository.GetPickupRequestByIdAsync(id);
            if (pickupRequest == null)
            {
                return NotFound();
            }
            return View(pickupRequest);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pickupRequestRepository.DeletePickupRequestAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
