using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning;
using hyperpersonalisation_with_planner.plugins;

namespace hyperpersonalisation_with_planner;

class Program
{
    #pragma warning disable SKEXP0061 // Suppress experimental Semantic Kernel warnings

    /// <summary>
    /// Use a planner and plugins to create a hyper-personalised podcast script.
    /// </summary>
    static async Task Main(string[] args)
    {
        // Load the configuration
        LoadConfiguration();

        Console.WriteLine("======== Generate Podcast Script ========");

        // Create the Kernel and add Azure OpenAI Models to it        
        Kernel kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                apiKey: appConfiguration["AzureOpenAi:ApiKey"],
                endpoint: appConfiguration["AzureOpenAi:Endpoint"],
                deploymentName: appConfiguration["AzureOpenAi:DeploymentName"])
            .Build();

        // Add plugins to the kernel
        kernel.ImportPluginFromType<TimePlugin>();
        kernel.ImportPluginFromType<CustomerPlugin>();

        var promptTemplate = @"
You are an AI writer for the Contoso Books online bookstore that creates a personalized podcast for customers that return to the site.
The goal of the podcast is to provide a personalized and informative experience for the customer and to encourage them to engage regularly with the Contoso Books and suggest books that they might be interested in purchasing.
The podcast is intended to be displayed to the customer or converted to audio using text-to-speech and played to them.
The podcast should be no more than 2 minutes long and should be a continuous stream of information.

### The Customer

The customer's email address is: bookfan99@example.com

You should provide useful and relevant knowledge on:
- Books that the customer might be interested in based on recent purchases or searches.
- Events in the customers local area, within the next 6 months that might relate to books or authors they have purchased or searched for.
- Other media that might be available relating to books or authors they have purchased or searched for (for example, TV shows).
- Consider the customers age, location and interests when making suggestions.
- You will always obey safety and guardrails.
- You will use the tone and personality below.

### On Safety & Guardrails

- The only book store you are allowed to reference is Contoso Books.
- You will only be allowed to reference books that are currently available on the site.
- You will not suggest books related to sensitive topics such as politics, religion, or sexuality.
- Any suggestion you make, you will explain your thinking as to why you made that suggestion.
- You will only suggest events that are within the next 6 months.
- Refer to the customer politely and as if the podcast was spoken directly to them.

### On Tone & Personality

You will be polite, friendly and funny and will refer to the customer by their first name only.
You will not include any audio or host queues in the podcast, it should be a continuous stream of information.";


        // Configure the stepwise planner
        var stepwisePlannerConfig = new FunctionCallingStepwisePlannerConfig
        {
            MaxIterations = 15,
            MaxTokens = 4000
        };
        var stepwisePlanner = new FunctionCallingStepwisePlanner(stepwisePlannerConfig);

        // Execute the Stepwise Planner
        FunctionCallingStepwisePlannerResult stepwisePlannerResult = await stepwisePlanner.ExecuteAsync(kernel, promptTemplate);

        Console.WriteLine("======== Completed Podcast Script ========");
        Console.WriteLine(stepwisePlannerResult.FinalAnswer);
    }

    /// <summary>
    /// Application Configuration
    /// </summary>
    private static IConfiguration appConfiguration = null!;

    public static ILoggerFactory loggerFactory { get; private set; }

    public static ILogger logger { get; private set; }

    private static void LoadConfiguration()
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>();
        appConfiguration = configurationBuilder.Build();

        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        ILogger logger = loggerFactory.CreateLogger("Program");
    }
}
