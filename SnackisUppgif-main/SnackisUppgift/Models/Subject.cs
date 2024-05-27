using System.Text.Json.Serialization;

namespace SnackisUppgift.Models
{
    public class Subject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("category")]
        public string Category { get; set; }
    }
}
