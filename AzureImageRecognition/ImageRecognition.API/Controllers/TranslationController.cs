using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImageRecognition.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly string _key;
        private readonly string _translatorEndpoint;
        private readonly string _location;
        private const string route = "/translate?api-version=3.0&from=en&to=fr";

        public TranslationController(IOptions<AzureConfig> config)
        {
            _translatorEndpoint = config.Value.TranslatorEndpoint;
            _key = config.Value.TranslationSubscriptionKey;
            _location = config.Value.Location;
        }

        [HttpPost("translate")]
        public async Task<IActionResult> TranslateText([FromBody] string text)
        {
            try
            {
                using var client = new HttpClient();
                using var request = new HttpRequestMessage();                
                object[] body = new object[] { new { Text = text } };
                var requestBody = JsonConvert.SerializeObject(body);

                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(_translatorEndpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", _key);
                request.Headers.Add("Ocp-Apim-Subscription-Region", _location);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                string result = await response.Content.ReadAsStringAsync();


                return this.Ok(result);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}
