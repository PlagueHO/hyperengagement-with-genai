using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace hyperpersonalisation_with_planner.plugins;

internal sealed class TimePlugin
{
    /// <summary>
    /// Plugin to get the current date and time
    /// </summary>
    /// <param name="logger"></param>
    /// <returns>Current date and time in UTC format</returns>
    [KernelFunction, Description("Retrieves the current time in UTC.")]
    public string GetCurrentUtcTime(
        ILogger? logger = null)
    {
        var currentTime = DateTime.UtcNow.ToString("R");
        Console.WriteLine("GetCurrentUtcTime: Current date and time returned is {0}", currentTime);
        
        return currentTime;
    }
}
