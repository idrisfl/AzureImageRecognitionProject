namespace ImageRecognition.UI.Models
{
    public class ImageResult
    {
        public List<string> ImageDescriptions { get; set; }
        public ImageResult()
        {
            ImageDescriptions = new List<string>();
        }
    }
}
