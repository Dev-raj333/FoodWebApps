using FoodDonationWebApp.Models;

namespace FoodDonationWebApp.Services.Interfaces
{
    public interface IDonation
    {
        Task<IEnumerable<Donation>> GetAllDonationsAsync();
        Task<IEnumerable<Donation>> GetDonationsByDonorIdAsync(string donorId);
        Task<Donation> GetDonationByIdAsync(int id);
        Task AddDonationAsync(Donation donation);
        Task UpdateDonationAsync(Donation donation);
        Task DeleteDonationAsync(int id);
    }
}
