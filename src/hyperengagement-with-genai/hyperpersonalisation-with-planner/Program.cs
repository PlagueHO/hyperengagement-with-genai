using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.Experimental.Agents;

namespace hyperpersonalisation_with_planner;

internal class Program
{
#pragma warning disable SKEXP0101 // Suppress experimental Semantic Kernel warnings

    /// <summary>
    /// Specific model is required that supports agents and function calling.
    /// Currently this is limited to Open AI hosted services.
    /// </summary>
    private const string OpenAIFunctionEnabledModel = "gpt-4-0613";

    /// <summary>
    /// Multiple Azure OpenAI Service agents collaborate to produce a hyper-personalized podcast
    /// </summary>
    async Task Main(string[] args)
    {
        // Load the configuration
        LoadConfiguration();

        Console.WriteLine("======== Generate Podcast Script ========");
        IAgentThread? thread = null;
        try
        {
            // Create copy-writer agent to generate ideas
            var copyWriter = await CreateCopyWriterAsync();
            // Create art-director agent to review ideas, provide feedback and final approval
            var artDirector = await CreateArtDirectorAsync();

            // Create collaboration thread to which both agents add messages.
            thread = await copyWriter.NewThreadAsync();

            // Add the user message
            var messageUser = await thread.AddUserMessageAsync("concept: maps made out of egg cartons.");
            DisplayMessage(messageUser);

            bool isComplete = false;
            do
            {
                // Initiate copy-writer input
                var agentMessages = await thread.InvokeAsync(copyWriter).ToArrayAsync();
                DisplayMessages(agentMessages, copyWriter);

                // Initiate art-director input
                agentMessages = await thread.InvokeAsync(artDirector).ToArrayAsync();
                DisplayMessages(agentMessages, artDirector);

                // Evaluate if goal is met.
                if (agentMessages.First().Content.Contains("PRINT IT", StringComparison.OrdinalIgnoreCase))
                {
                    isComplete = true;
                }
            }
            while (!isComplete);
        }
        finally
        {
            // Clean-up (storage costs $)
            await Task.WhenAll(s_agents.Select(a => a.DeleteAsync()));
        }
    }

    private async Task<IAgent> CreateCopyWriterAsync(IAgent? agent = null)
    {
        return
        Track(
                await new AgentBuilder()
                    .WithOpenAIChatCompletion(OpenAIFunctionEnabledModel, appConfiguration["OpenAi:ApiKey"])
                    .WithInstructions("You are a copywriter with ten years of experience and are known for brevity and a dry humor. You're laser focused on the goal at hand. Don't waste time with chit chat. The goal is to refine and decide on the single best copy as an expert in the field.  Consider suggestions when refining an idea.")
                    .WithName("Copywriter")
                    .WithDescription("Copywriter")
                    .WithPlugin(agent?.AsPlugin())
                    .BuildAsync());
    }

    private async Task<IAgent> CreateArtDirectorAsync()
    {
        return
        Track(
                await new AgentBuilder()
                    .WithOpenAIChatCompletion(OpenAIFunctionEnabledModel, appConfiguration["OpenAi:ApiKey"])
                    .WithInstructions("You are an art director who has opinions about copywriting born of a love for David Ogilvy. The goal is to determine is the given copy is acceptable to print, even if it isn't perfect.  If not, provide insight on how to refine suggested copy without example.  Always respond to the most recent message by evaluating and providing critique without example.  Always repeat the copy at the beginning.  If copy is acceptable and meets your criteria, say: PRINT IT.")
                    .WithName("Art Director")
                    .WithDescription("Art Director")
                    .BuildAsync());
    }

    private void DisplayMessages(IEnumerable<IChatMessage> messages, IAgent? agent = null)
    {
        foreach (var message in messages)
        {
            DisplayMessage(message, agent);
        }
    }

    private static void DisplayMessage(IChatMessage message, IAgent? agent = null)
    {
        Console.WriteLine($"[{message.Id}]");
        if (agent != null)
        {
            Console.WriteLine($"# {message.Role}: ({agent.Name}) {message.Content}");
        }
        else
        {
            Console.WriteLine($"# {message.Role}: {message.Content}");
        }
    }

    // Track agents for clean-up
    private List<IAgent> s_agents = new();

    private IAgent Track(IAgent agent)
    {
        s_agents.Add(agent);

        return agent;
    }

    IConfiguration appConfiguration = null!;

    private void LoadConfiguration()
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true);
        appConfiguration = configurationBuilder.Build();
    }
}
