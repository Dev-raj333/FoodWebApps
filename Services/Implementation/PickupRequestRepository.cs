using FoodDonationWebApp.Data;
using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDonationWebApp.Services.Implementation
{
    public class PickupRequestRepository : IPickupRequestRepository
    {
        private readonly FoodDbContext _context;

        public PickupRequestRepository(FoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PickupRequest>> GetAllPickupRequestsAsync()
        {
            return await _context.PickupRequests
                .Include(pr => pr.Donation)
                .Include(pr => pr.Volunteer)
                .ToListAsync();
        }

        public async Task<PickupRequest> GetPickupRequestByIdAsync(int id)
        {
            return await _context.PickupRequests
                .Include(pr => pr.Donation)
                .Include(pr => pr.Volunteer)
                .FirstOrDefaultAsync(pr => pr.Id == id);
        }

        public async Task AddPickupRequestAsync(PickupRequest pickupRequest)
        {
            _context.PickupRequests.Add(pickupRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePickupRequestAsync(PickupRequest pickupRequest)
        {
            _context.Entry(pickupRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePickupRequestAsync(int id)
        {
            var pickupRequest = await _context.PickupRequests.FindAsync(id);
            if (pickupRequest != null)
            {
                _context.PickupRequests.Remove(pickupRequest);
                await _context.SaveChangesAsync();
            }
        }

    }
}
