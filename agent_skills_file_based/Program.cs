
// https://github.com/microsoft/agent-framework/blob/main/dotnet/samples/02-agents/AgentSkills/Agent_Step01_FileBasedSkills


using AgentHelpers;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

// --- Skills Provider ---
// Discovers skills from the 'skills' directory containing SKILL.md files.
// The script runner runs file-based scripts (e.g. Python) as local subprocesses.
var skillsProvider = new AgentSkillsProvider(
    Path.Combine(AppContext.BaseDirectory, "skills"),
    SubprocessScriptRunner.RunAsync);
    
var chatClient = AgentHelpers.ClientHelpers.InitializeOpenAiChatClient();
var agent = chatClient.AsAIAgent(new ChatClientAgentOptions
    {
        Name = "UnitConverterAgent",
        ChatOptions = new ChatOptions
        {
            ModelId = AgentHelpers.ClientHelpers.ChatClientModelId,
            Instructions = "You are a friendly assistant that can convert units.",
        },
        AIContextProviders =  [skillsProvider]
    }).AsBuilder()
    .UseToolApproval(new ToolApprovalAgentOptions
    {
        // NOTE: Auto-approving all skill tools is done here for simplicity in
        // this demonstration. In production, you should prompt the user before
        // allowing script execution. See Agent_Step07_SkillsAutoApproval for a
        // walkthrough of the full approval flow.
        AutoApprovalRules = [AgentSkillsProvider.AllToolsAutoApprovalRule],
    })
    .Build();

// --- Example: Unit conversion ---
Console.WriteLine("Converting units with file-based skills");
Console.WriteLine(new string('-', 60));

var response = await agent.RunAsync(
    "How many fids is a 20 wids? And how many pounds is 75 kilograms? Report what skills you used to answer the question.");

Console.WriteLine($"Agent: {response.Text}");