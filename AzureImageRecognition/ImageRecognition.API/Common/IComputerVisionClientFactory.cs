using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace ImageRecognition.API.Common
{
    public interface IComputerVisionClientFactory
    {
        IComputerVisionClient CreateClient();
    }
}