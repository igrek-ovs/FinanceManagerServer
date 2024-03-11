namespace FinancialAccountingServer.Services.interfaces
{
    public interface IBlobService
    {
        Task<string> UploadBlobAsync(IFormFile file);

        Task<bool> DeleteBlobAsync(string imagePath);
    }
}
