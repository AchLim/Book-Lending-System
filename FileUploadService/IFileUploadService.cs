namespace Book_Lending_System.FileUploadService
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string fileName = "");
    }
}
