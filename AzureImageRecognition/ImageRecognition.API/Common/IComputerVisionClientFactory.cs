using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.Face;

namespace ImageRecognition.API.Common
{
    public interface IComputerVisionClientFactory
    {
        IComputerVisionClient CreateComputerVisionClient();

        IFaceClient CreateFaceClient();
    }
}