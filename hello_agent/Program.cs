using Microsoft.Extensions.AI;

var chatClient = AgentHelpers.ClientHelpers.InitializeOpenAiChatClient();

var agent = chatClient.AsAIAgent(
    name: "Joker",
    instructions: "You are good at telling jokes.");

// Invoke the agent and output the text result.
Console.WriteLine(await agent.RunAsync("Tell me a joke about a pirate."));

// Invoke the agent with streaming support.
await foreach (var update in agent.RunStreamingAsync("Tell me a joke about a pirate."))
{
    Console.Write(update.ResponseId);
    Console.WriteLine(update);
}

var response = await agent.RunAsync(
    "Explain the difference between tool calling and ordinary text completion.");

Console.WriteLine(response.Text);

