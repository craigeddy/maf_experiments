
using Microsoft.Extensions.AI;

var chatClient = AgentHelpers.ClientHelpers.InitializeOpenAiClient();
var agent = chatClient.AsAIAgent(
    name: "Joker",
    instructions: "You are good at telling jokes.");
    
// Invoke the agent with a multi-turn conversation, where the context is preserved in the session object.
var session = await agent.CreateSessionAsync();
Console.WriteLine(await agent.RunAsync("Tell me a joke about a pirate.", session));
Console.WriteLine(await agent.RunAsync("Now add some emojis to the joke and tell it in the voice of a pirate's parrot.", session));

Console.WriteLine("New session...");
Console.WriteLine();

session = await agent.CreateSessionAsync();

// First turn
Console.WriteLine(await agent.RunAsync("My name is Alice and I love hiking.", session));

// Second turn — the agent remembers the user's name and hobby
Console.WriteLine(await agent.RunAsync("What do you remember about me?", session));
