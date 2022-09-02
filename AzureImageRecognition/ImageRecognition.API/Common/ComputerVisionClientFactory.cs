using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Extensions.Options;

namespace ImageRecognition.API.Common
{
    public class ComputerVisionClientFactory : IComputerVisionClientFactory
    {
        private readonly AzureConfig azureConfig;

        public ComputerVisionClientFactory(IOptions<AzureConfig> config)
        {
            this.azureConfig = config.Value;
        }

        /*
         * AUTHENTICATE
         * Creates a Computer Vision client used by each example.
         */
        public IComputerVisionClient CreateClient()
        {
            var apiKeyServiceClientCredentials = new ApiKeyServiceClientCredentials(this.azureConfig.SubscriptionKey);
            var computerVisionClient = new ComputerVisionClient(apiKeyServiceClientCredentials)
            {
                Endpoint = this.azureConfig.EndPoint
            };
            return computerVisionClient;
        }
    }
}
