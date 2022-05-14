using ImageRecognition.API.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageRecognition.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        // private readonly AzureConfig azureConfig;
        IComputerVisionClientFactory computerVisionClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagesController"/> object.
        /// </summary>
        /// <param name="config"></param>
        public ImagesController(IComputerVisionClientFactory computerVisionClientFactory)
        {
            this.computerVisionClientFactory = computerVisionClientFactory;
        }

        /// <summary>
        /// Retrieves tags by analyzing image content
        /// </summary>
        /// <param name="file1">The image file sent through the form data</param>
        /// <returns>A list of tags</returns>
        [HttpPost("tags")]
        public async Task<IActionResult> RetrieveTags([FromForm(Name = "filetest")] IFormFile filetest)
        {
            if (filetest == null)
            {
                return BadRequest("Image file missing");
            }


            var client = this.computerVisionClientFactory.CreateClient();
            var tags = new List<string>();

            var results = await client.TagImageInStreamWithHttpMessagesAsync(filetest.OpenReadStream());
            foreach (var tag in results.Body.Tags)
            {
                tags.Add(tag.Name);
            }

            return this.Ok(tags);
        }

        /// <summary>
        /// Retrieves description information of the image
        /// </summary>
        /// <param name="filetest">The image file sent through the form data</param>
        /// <returns>A list of possible descriptions of the image</returns>
        [HttpPost("describe")]
        public async Task<IActionResult> RetrieveDescription([FromForm(Name = "filetest")] IFormFile filetest)
        {
            if (filetest == null)
            {
                return BadRequest("Image file missing");
            }

            var client = this.computerVisionClientFactory.CreateClient();

            var descriptions = new List<string>();

            var results = await client.DescribeImageInStreamWithHttpMessagesAsync(filetest.OpenReadStream());
            foreach (var caption in results.Body.Captions)
            {
                descriptions.Add($"Description:{caption.Text}, Confidence Level:{caption.Confidence}");
            }

            return this.Ok(descriptions);
        }
    }
}
