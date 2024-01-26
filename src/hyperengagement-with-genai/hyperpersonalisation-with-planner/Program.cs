using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planners;
using Microsoft.SemanticKernel.Plugins.Core;

namespace hyperpersonalisation_with_planner;

class Program
{
    /// <summary>
    /// Use a planner and plugins to create a hyper-personalised podcast script.
    /// </summary>
    static void Main(string[] args)
    {
        // Load the configuration
        LoadConfiguration();

        Console.WriteLine("======== Generate Podcast Script ========");

        var kernel = InitializeKernel();


    }

    private static IConfiguration appConfiguration = null!;

    private static void LoadConfiguration()
    {
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true);
        appConfiguration = configurationBuilder.Build();
    }

    /// <summary>
    /// Initialize the kernel and load plugins.
    /// </summary>
    /// <returns>A kernel instance</returns>
    private static Kernel InitializeKernel()
    {
        Kernel kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                apiKey: appConfiguration["AzureOpenAi:ApiKey"],
                endpoint: appConfiguration["AzureOpenAi:Endpoint"],
                deploymentName: appConfiguration["AzureOpenAi:DeploymentName"])
            .Build();

        return kernel;
    }
}
