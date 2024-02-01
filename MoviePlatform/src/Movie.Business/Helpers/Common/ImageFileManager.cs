using Microsoft.AspNetCore.Http;

namespace Movie.Business.Helpers.Common
{
    public static class ImageFileManager
    {
        public static string Save(string root, string folderPath, IFormFile imageFile)
        {
            var newName = imageFile.FileName;
            newName = newName.Length > 64 ? newName.Substring(newName.Length - 64, 64) : newName;
            newName = Guid.NewGuid().ToString() + newName;
            string savePath = Path.Combine(root, folderPath, newName);

            using (FileStream stream = new FileStream(savePath,FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }
            return newName;
        }
        public static void Remove(string root,string folderPath,string imageUrl)
        {
            var deletePath = Path.Combine(root, folderPath, imageUrl);
            if(File.Exists(deletePath))
            {
                File.Delete(deletePath);
            }
        }
    }
}
