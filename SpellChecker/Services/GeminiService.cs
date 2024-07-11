using Mscc.GenerativeAI;
using SpellChecker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpellChecker.Services
{
    public class GeminiService
    {
        private readonly GenerativeModel _model;

        public GeminiService(string apiKey)
        {
            var googleAI = new GoogleAI(apiKey);
            _model = googleAI.GenerativeModel(model: Model.GeminiPro);
        }

        public async Task<SpellingCorrectionResponse> ImproveSentenceAsync(string sentence)
        {
            var prompt = $"Correct the following sentence and explain any spelling errors in plain text: \"{sentence}\"";
            var response = await _model.GenerateContent(prompt);

            // Extract spelling corrections from the response
            var spellingCorrections = ExtractSpellingCorrections(response.Text);

            return new SpellingCorrectionResponse
            {
                ImprovedSentence = ExtractImprovedSentence(response.Text),
                SpellingCorrections = spellingCorrections
            };
        }

        public async Task<string> GenerateFeedbackAsync(string correctedSentence)
        {
            var prompt = $"Provide feedback on the following corrected sentence: \"{correctedSentence}\"";
            var response = await _model.GenerateContent(prompt);

            // Extract feedback from the response
            var feedback = ExtractFeedback(response.Text);

            return feedback;
        }

        private string ExtractImprovedSentence(string responseText)
        {
            // Assuming the improved sentence is the first part before "Spelling errors:"
            var splitParts = responseText.Split(new string[] { "Spelling errors:" }, System.StringSplitOptions.None);
            return splitParts[0].Trim();
        }

        private List<string> ExtractSpellingCorrections(string responseText)
        {
            var correctionsList = new List<string>();
            // Assuming spelling corrections are listed after "Spelling errors:" and before the next section
            var splitParts = responseText.Split(new string[] { "Spelling errors:" }, System.StringSplitOptions.None);
            if (splitParts.Length > 1)
            {
                var corrections = splitParts[1].Trim().Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var correction in corrections)
                {
                    correctionsList.Add(correction.Trim());
                }
            }
            else
            {
                correctionsList.Add("No spelling corrections found");
            }
            return correctionsList;
        }

        private string ExtractFeedback(string responseText)
        {
            // Extract feedback from the response
            // Adjust this logic based on the actual structure of the feedback response from your API
            return responseText; // Example logic; customize as per API response structure
        }
    }
}
