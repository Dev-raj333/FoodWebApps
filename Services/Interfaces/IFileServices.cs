namespace FoodDonationWebApp.Services.Interfaces
{
    public interface IFileServices
    {
        Task<string> SaveFileAsync(IFormFile file,string folderName);
    }
}
