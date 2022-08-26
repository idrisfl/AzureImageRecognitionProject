namespace ImageRecognition.UI.Models
{
    public class TranslatedResult
    {
        public IEnumerable<Translations> Translations { get; set; }
    }

    public class Translations
    {
        public string Text { get; set; }
    
        public string To { get; set; }
    }
}
