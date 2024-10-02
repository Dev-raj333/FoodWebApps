using FoodDonationWebApp.Models;

namespace FoodDonationWebApp.Services.Interfaces
{
    public interface IPickupRequestRepository
    {
        Task<IEnumerable<PickupRequest>> GetAllPickupRequestsAsync();
        Task<PickupRequest> GetPickupRequestByIdAsync(int id);
        Task AddPickupRequestAsync(PickupRequest pickupRequest);
        Task UpdatePickupRequestAsync(PickupRequest pickupRequest);
        Task DeletePickupRequestAsync(int id);
    }
}
