using System.Collections.Generic;
using System.Threading.Tasks;
using ImageRecognition.API.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Configuration;
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
        // GET: api/Images
        [HttpGet("tags")]
        public async Task<IActionResult> Get()
        {
            var tags = new Dictionary<string, double>();
            var client = ComputerVisionClientFactory.Authenticate(this.azureConfig.EndPoint, this.azureConfig.SubscriptionKey);
            const string ANALYZE_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png";


            var results = await client.TagImageWithHttpMessagesAsync(ANALYZE_URL_IMAGE);

            foreach (var tag in results.Body.Tags)
            {
                tags.Add(tag.Name, tag.Confidence);
            }
            return this.Ok(tags);
        }

        // GET: api/Images/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Images
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Images/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
