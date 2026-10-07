using System.ClientModel;
using Microsoft.Extensions.AI;
using OpenAI;

#pragma warning disable OPENAI001

namespace AgentHelpers;

public static class ClientHelpers
{
    public static string ChatClientModelId = Environment.GetEnvironmentVariable("LOCAL_LLM_MODEL")
                                             ?? "nvidia/NVIDIA-Nemotron-3.5-Lightning-30B-A3B-NVFP4";

    public static IChatClient InitializeOpenAiChatClient()
    {
        var client = CreateOpenAiClient();

        var chatClient1 = client
            .GetChatClient(ChatClientModelId)
            .AsIChatClient();
        return chatClient1;
    }

    public static IChatClient InitializeExtractionClient()
    {
        var modelId = Environment.GetEnvironmentVariable("LOCAL_LLM__EXTRACTION_MODEL")
                      ?? "nvidia/NVIDIA-Nemotron-3.5-Lightning-30B-A3B-NVFP4";

        // Use Chat Completions for local OpenAI-compatible servers. Their reasoning
        // metadata is not guaranteed to use the Responses API ReasoningStatus values.
        return CreateOpenAiClient()
            .GetChatClient(modelId)
            .AsIChatClient();
    }

    private static OpenAIClient CreateOpenAiClient()
    {
        var endpoint = new Uri(
            Environment.GetEnvironmentVariable("LOCAL_LLM_BASE_URL")
            ?? "http://10.10.21.2:8000/v1");

        var openAiOptions = new OpenAIClientOptions
        {
            Endpoint = endpoint
        };

// The SDK requires a credential object even if the local server ignores it.
        var apiKey = Environment.GetEnvironmentVariable("LOCAL_LLM_API_KEY") ?? "not-needed";
        var client = new OpenAIClient(new ApiKeyCredential(apiKey), openAiOptions);
        return client;
    }
}