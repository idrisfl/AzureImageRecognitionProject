using System.Text;
using ImageRecognition.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ImageRecognition.UI.Controllers
{
    public class ImageUploadController : Controller
    {
        private readonly string _imageRecogntionApiBaseUrl;
        private readonly HttpClient _httpClient;

        public ImageUploadController(IConfiguration config)
        {
            _imageRecogntionApiBaseUrl = config.GetValue<string>("ImageRecognitionAPIBaseUrl");
            _httpClient = new HttpClient { BaseAddress = new Uri(_imageRecogntionApiBaseUrl)  };
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            List<string> resultOriginal = new List<string>();
            List<string> translatedResult = new List<string>();

            try
            {
                var imageResult = new List<ImageResult>();

                foreach (var file in files.Where(f => f.Length > 0))
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);
                    var byteArrayContent = ms.ToArray();
                    ByteArrayContent byteContent = new(byteArrayContent);
                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(byteContent, "filetest", "filename");

                    var response = await _httpClient.PostAsync("/api/images/describe",  multipartContent);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    resultOriginal.Add(responseContent);

                    var content = new StringContent(JsonConvert.SerializeObject(responseContent), Encoding.UTF8, "application/json");
                    var finalResponse = await _httpClient.PostAsync("/api/translation/translate", content);
                    var finalResponseContent = await finalResponse.Content.ReadAsStringAsync();
                    translatedResult.Add(finalResponseContent);

                    var result = ParseResultOnly<IEnumerable<TranslatedResult>>(finalResponseContent);

                    var fileResult = new ImageResult
                    {
                        ImageDescription = result.First().Translations.First().Text,
                        Base64ImageString = Convert.ToBase64String(byteArrayContent, Base64FormattingOptions.None)
                    };
                    imageResult.Add(fileResult);
                }
                return View("Result", imageResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View("Index");
            }
        }

        private static T ParseResultOnly<T>(string content)
        {
            var result = JsonConvert.DeserializeObject<T>(content);

            return result;
        }
    }
}
