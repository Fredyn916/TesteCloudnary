using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;

        public UploadController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imagem)
        {
            string fileName = imagem.FileName;

            byte[] imagemBytes;
            using (var memoryStream = new MemoryStream())
            {
                await imagem.CopyToAsync(memoryStream);
                imagemBytes = memoryStream.ToArray();
            }

            using (var memoryStream = new MemoryStream(imagemBytes))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(fileName, memoryStream)
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return StatusCode((int)uploadResult.StatusCode, "Erro ao carregar a imagem.");
                }

                return Ok(new { url = uploadResult.SecureUrl });
            }
        }
    }

}
