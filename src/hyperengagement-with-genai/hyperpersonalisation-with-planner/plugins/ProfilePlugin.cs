using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace hyperpersonalisation_with_planner.plugins;

internal sealed class CustomerPlugin
{
    /// <summary>
    /// Plugin to get the customers profile information
    /// </summary>
    /// <param name="emailAddress">The email address of the customer whose profile should be returned</param>
    /// <param name="logger">The logger</param>
    /// <returns>Current date and time in UTC format</returns>
    [KernelFunction, Description("Given the Contoso books customer e-mail address, return their profile information including name, age, location and list of interests")]
    public string GetCustomerProfile(
        [Description("The email address of the customer whose profile should be returned.")] string emailAddress,
        ILogger? logger = null)
    {
        logger?.LogInformation("GetCustomerProfile: returning the customer profile for {0}", emailAddress);

        // In a real application this would be looked up using an API or database request
        return @"
            Name: Jane Smith
            Age: 35
            Location: Manly, Sydney, Australia
            Interests (based on recent purchases and searches): DevOps, Horror Fiction, Fantasy, Software Development, Travel";
    }
}
