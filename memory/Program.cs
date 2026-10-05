using memory;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

// https://github.com/microsoft/agent-framework/tree/main/dotnet/samples/01-get-started/04_memory

var model = Environment.GetEnvironmentVariable("LOCAL_LLM__EXTRACTION_MODEL");
if(string.IsNullOrEmpty(model))
{
    Console.WriteLine("Please set the LOCAL_LLM__EXTRACTION_MODEL environment variable to a valid model ID.");
    return;
}

var chatClient = AgentHelpers.ClientHelpers.InitializeOpenAiChatClient();

var extractionClient = AgentHelpers.ClientHelpers.InitializeExtractionClient();

var agent = chatClient.AsAIAgent(new ChatClientAgentOptions
{
    ChatOptions = new ChatOptions
    {
        ModelId = AgentHelpers.ClientHelpers.ChatClientModelId,
        Instructions = "You are a friendly assistant. Always address the user by their name.",
    },
    AIContextProviders = [new UserInfoMemory(extractionClient)]
});

// Create a new session for the conversation.
var session = await agent.CreateSessionAsync();

Console.WriteLine(">> Use session with blank memory\n");

// Invoke the agent and output the text result.
Console.WriteLine(await agent.RunAsync("Hello, what is the square root of 9?", session));
Console.WriteLine(await agent.RunAsync("My name is Ruaidhrí", session));
Console.WriteLine(await agent.RunAsync("I am 20 years old", session));

// We can serialize the session. The serialized state will include the state of the memory component.
var sessionElement = await agent.SerializeSessionAsync(session);

Console.WriteLine("\n>> Use deserialized session with previously created memories\n");

// Later we can deserialize the session and continue the conversation with the previous memory component state.
var deserializedSession = await agent.DeserializeSessionAsync(sessionElement);
Console.WriteLine(await agent.RunAsync("What is my name and age?", deserializedSession));

Console.WriteLine("\n>> Read memories using memory component\n");

// It's possible to access the memory component via the agent's GetService method.
var userInfo = agent.GetService<UserInfoMemory>()?.GetUserInfo(deserializedSession);

// Output the user info that was captured by the memory component.
Console.WriteLine($"MEMORY - User Name: {userInfo?.UserName}");
Console.WriteLine($"MEMORY - User Age: {userInfo?.UserAge}");

Console.WriteLine("\n>> Use new session with previously created memories\n");

// It is also possible to set the memories using a memory component on an individual session.
// This is useful if we want to start a new session, but have it share the same memories as a previous session.
var newSession = await agent.CreateSessionAsync();
if (userInfo is not null && agent.GetService<UserInfoMemory>() is UserInfoMemory newSessionMemory)
{
    newSessionMemory.SetUserInfo(newSession, userInfo);
}

// Invoke the agent and output the text result.
// This time the agent should remember the user's name and use it in the response.
Console.WriteLine(await agent.RunAsync("What is my name and age?", newSession));
