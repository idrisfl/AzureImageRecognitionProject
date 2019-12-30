using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageRecognition.API.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ImageRecognition.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly AzureConfig azureConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagesController"/> object.
        /// </summary>
        /// <param name="config"></param>
        public ImagesController(IOptions<AzureConfig> config)
        {
            this.azureConfig = config.Value;
        }

        /// <summary>
        /// Retrieves tags by analyzing image content
        /// </summary>
        /// <param name="base64Image">The base64 string representation of the image</param>
        /// <returns>A list of tags</returns>
        [HttpPost("tags")]
        public async Task<IActionResult> RetrieveTags([FromBody] string base64Image)
        {

            var tags = new List<string>();
            var client = ComputerVisionClientFactory.Authenticate(this.azureConfig.EndPoint, this.azureConfig.SubscriptionKey);
            byte[] data = System.Convert.FromBase64String(base64Image);

            using (var memoryStream = new MemoryStream(data))
            {
                var results = await client.TagImageInStreamWithHttpMessagesAsync(memoryStream);
                foreach (var tag in results.Body.Tags)
                {
                    tags.Add(tag.Name);
                }
            }

            return this.Ok(tags);
        }

        /// <summary>
        /// Retrieves description information of the image
        /// </summary>
        /// <param name="base64Image">The base64 string representation image</param>
        /// <returns>A list of possible descriptions of the image</returns>
        [HttpPost("descriptions")]
        public async Task<IActionResult> RetrieveDescription([FromBody] string base64Image)
        {
            var client = ComputerVisionClientFactory.Authenticate(this.azureConfig.EndPoint, this.azureConfig.SubscriptionKey);
            byte[] data = System.Convert.FromBase64String(base64Image);

            var descriptions = new List<string>();

            using (var memoryStream = new MemoryStream(data))
            {
                var results = await client.DescribeImageInStreamWithHttpMessagesAsync(memoryStream);
                foreach (var caption in results.Body.Captions)
                {
                    descriptions.Add($"description:{caption.Text}, confidence level:{caption.Confidence}");
                }

                return this.Ok(descriptions);
            }
        }
    }
}
