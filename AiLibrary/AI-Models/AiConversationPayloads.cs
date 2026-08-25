using System.Text.Json.Serialization;

namespace LocalAiLibrary.AiLibrary.AI_Models;

// 1. Represents an individual message in the conversation
public record ChatRequestMessage(
    [property: JsonPropertyName("role")] string Role,
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("tool_call_id")] string ToolCallId
);

// 2. Represents the complete payload sent to the API
public record ChatRequest(
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("messages")] IReadOnlyList<ChatRequestMessage> Messages,
    [property: JsonPropertyName("max_tokens")] int MaxTokens,
    [property: JsonPropertyName("tools")] List<Tool>? Tools = null,
    [property: JsonPropertyName("tool_choice")] string? ToolChoice = "auto"
);

public record Tool([property: JsonPropertyName("type")] string Type, // "function"
    [property: JsonPropertyName("function")] FunctionDefinition Function
);

public record FunctionDefinition(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("parameters")] object Parameters // JSON Schema object
);