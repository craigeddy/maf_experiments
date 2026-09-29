using System.ComponentModel;
using Microsoft.Extensions.AI;

[Description("Get the weather for a given location.")]
static string GetWeather([Description("The location to get the weather for.")] string location)
    => $"The weather in {location} is cloudy with a high of 15°C.";

var chatClient = AgentHelpers.ClientHelpers.InitializeOpenAiClient();

var agent = chatClient.AsAIAgent(
    name: "Weather Person",
    instructions: "You are a helpful assistant", 
    tools: [AIFunctionFactory.Create(GetWeather)]);
    
// Non-streaming agent interaction with function tools.
Console.WriteLine(await agent.RunAsync("What is the weather like in Amsterdam?"));

// Streaming agent interaction with function tools.
await foreach (var update in agent.RunStreamingAsync("What is the weather like in Amsterdam?"))
{
    Console.WriteLine(update);
}    