namespace TOKENAPI.Services
{
    public class FileHandler
    {
        static readonly string baseFolder = "Uploads/";
        static readonly string rootUrl = "http://localhost:5085/";
        public static string SaveImage(string folder, IFormFile image)
        {
            string ImageName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{baseFolder}\\{folder}");
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            var exactPath = Path.Combine(FilePath, ImageName);
            using (var fileSystem = new FileStream(exactPath, FileMode.Create))
            {
                image.CopyTo(fileSystem);
            }
            return rootUrl + baseFolder + folder + "/" + ImageName;
        }
        public static void DeleteImage(string imageName)
        {
            string exactPath = imageName.Substring(rootUrl.Length);
            var filePath = Path.Combine(exactPath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
