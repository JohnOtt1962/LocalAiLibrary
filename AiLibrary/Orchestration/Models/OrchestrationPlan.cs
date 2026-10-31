using System.Text.Json.Serialization;

namespace LocalAiLibrary.AiLibrary.Orchestration.Models
{
    public enum Status
    {
        Open, 
        Pending,
        Closed,
    }

    public class OrchestrationPlan
    {
        [JsonPropertyName("items")]
        public List<OrchestrationItem> Items { get; set; } = new List<OrchestrationItem>();
        public List<PayloadItem> PayloadItems { get; set; } = new List<PayloadItem>();
    }

    public class OrchestrationItem
    {
        [JsonPropertyName("call_index")]
        public int CallIndex { get; set; }

        [JsonPropertyName("depends_on_indices")]
        public List<int> DependsOnIndices { get; set; } = new List<int>();

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = string.Empty;

        public Status ItemStatus { get; set; } = Status.Open;
    }

    public class PayloadItem
    {
        public string ItemName { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
    }
}