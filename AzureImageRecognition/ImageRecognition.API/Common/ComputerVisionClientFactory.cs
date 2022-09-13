using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Extensions.Options;
using Microsoft.Azure.CognitiveServices.Vision;

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
        public IComputerVisionClient CreateComputerVisionClient()
        {
            var apiKeyServiceClientCredentials = new Microsoft.Azure.CognitiveServices.Vision.ComputerVision.ApiKeyServiceClientCredentials(this.azureConfig.SubscriptionKey);
            var computerVisionClient = new ComputerVisionClient(apiKeyServiceClientCredentials)
            {
                Endpoint = this.azureConfig.EndPoint
            };
            return computerVisionClient;
        }

        public IFaceClient CreateFaceClient()
        {
            var key = this.azureConfig.SubscriptionKey;
            var endpoint = this.azureConfig.EndPoint;
            var faceClient = new FaceClient(new Microsoft.Azure.CognitiveServices.Vision.Face.ApiKeyServiceClientCredentials(key)) 
            { 
                Endpoint = endpoint
            };

            return faceClient;
        }
    }
}
