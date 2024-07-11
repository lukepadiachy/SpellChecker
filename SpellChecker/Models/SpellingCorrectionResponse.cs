namespace SpellChecker.Models
{
    public class SpellingCorrectionResponse
    {
        public string ImprovedSentence { get; set; }
        public List<string> SpellingCorrections { get; set; }
    }
}

