﻿using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace ImageRecognition.API.Common
{
    public class ComputerVisionClientFactory : IComputerVisionClientFactory
    {

        private readonly AzureConfig azureConfig;

        // private ILogger<ComputerVisionClientFactory> logger;

        //private IComputerVisionClient computerVisionClient;

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
            // logger.LogInformation("Creating Client");

            var apiKeyServiceClientCredentials = new ApiKeyServiceClientCredentials(this.azureConfig.SubscriptionKey);
            var computerVisionClient = new ComputerVisionClient(apiKeyServiceClientCredentials)
            {
                Endpoint = this.azureConfig.EndPoint
            };
            return computerVisionClient;
        }

    }
}
