namespace ImageRecognition.UI.Models
{
    public class ImageResult
    {
        public List<string> ImageDescriptions { get; set; }

        public string ImageDescription { get; set; }

        public string Base64ImageString { get; set; }

        public ImageResult() => ImageDescriptions = new List<string>();
    }
}
