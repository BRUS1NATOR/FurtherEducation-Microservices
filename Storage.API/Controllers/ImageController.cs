using Microsoft.AspNetCore.Mvc;
using Storage.API.Attributes;
using Storage.Net;
using Storage.Net.Blobs;
using Swashbuckle.AspNetCore.Annotations;

namespace Storage.API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        IBlobStorage _storage;
        IConfiguration _configuration;

        public StorageController(IConfiguration configuration)
        {
            _configuration = configuration;
            _storage = StorageFactory.Blobs.DirectoryFiles(_configuration["StorageSettings:ImageFolder"]);
        }

        [HttpPost]
        [SwaggerOperation("Загрузить изображение")]
        public async Task<string> UploadImage([AllowedExtensions(new string[] { ".jpg", ".png", ".gif" })] IFormFile file)
        {
            var filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            using (Stream stream = file.OpenReadStream())
            {
                await _storage.WriteAsync("/" + filename, stream);
            }

            return string.Format("/{0}/{1}", "Images", filename);
        }

        [HttpGet]
        [SwaggerOperation("Получить все изображения")]
        public async Task<List<string>> GetImages()
        {
            var blobs = await _storage.ListFilesAsync(new ListOptions());
            return blobs.Select(x => "/Images" + x.FullPath).ToList();
        }
    }
}
