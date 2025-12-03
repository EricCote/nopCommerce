using Ecomzen.Plugin.Misc.Gifts.Domains;

namespace Ecomzen.Plugin.Misc.Gifts.Services;

/// <summary>
/// Interface for Gifts greeting service
/// </summary>
public interface IGiftsGreetingService
{
    /// <summary>
    /// Gets the greeting
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<AlloGreeting> GetGreetingAsync();

    /// <summary>
    /// Inserts a greeting
    /// </summary>
    /// <param name="greeting">Greeting</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertGreetingAsync(AlloGreeting greeting);

    /// <summary>
    /// Updates the greeting
    /// </summary>
    /// <param name="greeting">Greeting</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateGreetingAsync(AlloGreeting greeting);
}
