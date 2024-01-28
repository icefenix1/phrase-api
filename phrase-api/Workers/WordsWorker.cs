using phrase_api.Contracts.Workers;
using phrase_api.Models.External.WordsAPI;
using System.Text.Json;

namespace phrase_api.Workers
{
    public class WordsWorker : IWordsWoker
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WordsWorker(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IEnumerable<string> GetParts()
        {
            return _configuration["WordsApi:Parts"].Split(',');
        }

        public async Task<IEnumerable<string>> Search(string partOfSpeach, string search)
        {
            // Load the API endpoint URI from configuration
            var apiUrl = _configuration["WordsApi:Endpoint"];

            var APIKey = _configuration["WordsApi:APIKey"];

            // Append the query parameter to the URI
            var fullApiUrl = $"{apiUrl}?partOfSpeech={partOfSpeach.ToLower()}&letterPattern=%5E{search}.&hasDetails=''";

            var request = new HttpRequestMessage(HttpMethod.Get, fullApiUrl);

            request.Headers.Add("x-rapidapi-key", APIKey);
            request.Headers.Add("x-rapidapi-host", "wordsapiv1.p.rapidapi.com");

            // Make a GET request to the external API
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {                  
                // Read the response content as a string
                var jsonString = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON string to the RootObject class
                var data = JsonSerializer.Deserialize<SearchResponse>(jsonString);

                return data.Results.Data.Where(r => r.Split(' ').Length == 1).OrderBy(ob => ob.Length).Take(15);
            }
            else
            {
                // Handle error scenarios
                return new string[] { $"Error: {response.StatusCode}" };
            }
        }
    }
}
