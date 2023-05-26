namespace Book_Lending_System.FileUploadService
{
    public class LocalFileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public LocalFileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string filePath = "")
        {
            string path = filePath;
            if (string.IsNullOrWhiteSpace(filePath))
                path = Path.GetFileNameWithoutExtension(file.FileName);

            string fileType = Path.GetExtension(file.FileName);

            var filePathToCreate = Path.Combine(_environment.ContentRootPath, @"wwwroot\images\uploads", path + fileType);
            //string checkFileExistPath = Path.Combine(_environment.ContentRootPath, @"wwwroot\images\uploads", path + fileType);
            //if (File.Exists(checkFileExistPath))
            //{
            //    File.Delete(checkFileExistPath);
            //}


            using (var fileStream = new FileStream(filePathToCreate, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            string filePathToReturn = Path.Combine(path + fileType);
            return filePathToReturn;
        }
    }
}
