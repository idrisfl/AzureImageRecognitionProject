using ImageRecognition.API.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ImageRecognition.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacesController : ControllerBase
    {
        // Recognition model 4 was released in 2021 February.
        // It is recommended since its accuracy is improved
        // on faces wearing masks compared with model 3,
        // and its overall accuracy is improved compared
        // with models 1 and 2.
        private const string RECOGNITION_MODEL4 = RecognitionModel.Recognition04;

        private readonly IComputerVisionClientFactory _computerVisionClientFactory;

        private readonly FaceAttributeType[] requiredFaceAttributes = new FaceAttributeType[] {
            FaceAttributeType.Age,
            FaceAttributeType.Gender,
            FaceAttributeType.Smile,
            FaceAttributeType.FacialHair,
            FaceAttributeType.HeadPose,
            FaceAttributeType.Glasses,
            FaceAttributeType.Emotion,
            FaceAttributeType.QualityForRecognition
        };

        public FacesController(IComputerVisionClientFactory computerVisionClientFactory)
        {
            this._computerVisionClientFactory = computerVisionClientFactory;
        }

        [HttpPost("detect")]
        public async Task<IActionResult> DetectWithUrlAsync([FromBody]string url)
        {

            try
            {
                var faceClient = this._computerVisionClientFactory.CreateFaceClient();
                var result = await faceClient.Face.DetectWithUrlAsync(
                    url, 
                    returnFaceAttributes: requiredFaceAttributes.ToList(), 
                    returnFaceLandmarks: true, 
                    recognitionModel: RECOGNITION_MODEL4, 
                    detectionModel: DetectionModel.Detection01);

                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
