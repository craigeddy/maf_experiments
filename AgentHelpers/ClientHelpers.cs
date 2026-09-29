using System.ClientModel;
using Microsoft.Extensions.AI;
using OpenAI;

namespace AgentHelpers;

public static class ClientHelpers
{
    public static IChatClient InitializeOpenAiClient()
    {
        var endpoint = new Uri(
            Environment.GetEnvironmentVariable("LOCAL_LLM_BASE_URL")
            ?? "http://192.168.5.2:8000/v1");

        var modelId = Environment.GetEnvironmentVariable("LOCAL_LLM_MODEL")
                      ?? "nvidia/NVIDIA-Nemotron-3.5-Lightning-30B-A3B-NVFP4";

        var openAiOptions = new OpenAIClientOptions
        {
            Endpoint = endpoint
        };

// The SDK requires a credential object even if the local server ignores it.
        var apiKey = Environment.GetEnvironmentVariable("LOCAL_LLM_API_KEY") ?? "not-needed";
        var client = new OpenAIClient(new ApiKeyCredential(apiKey), openAiOptions);

        var chatClient1 = client
            .GetChatClient(modelId)
            .AsIChatClient();
        return chatClient1;
    }
}