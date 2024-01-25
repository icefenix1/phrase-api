using System.Text.Json.Serialization;

namespace phrase_api.Models.External.WordsAPI
{
    public class SearchResponse
    {
        [JsonPropertyName("query")]
        public Query Query { get; set; }

        [JsonPropertyName("results")]
        public SearchResults Results { get; set; }
    }

    public class Query
    {
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }
    }

    public class SearchResults
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("data")]
        public List<string> Data { get; set; }
    }
}
