using Nop.Data;
using Ecomzen.Plugin.Misc.Gifts.Domains;

namespace Ecomzen.Plugin.Misc.Gifts.Services;

/// <summary>
/// Gifts greeting service
/// </summary>
public class GiftsGreetingService : IGiftsGreetingService
{
    #region Fields

    private readonly IRepository<AlloGreeting> _greetingRepository;

    #endregion

    #region Ctor

    public GiftsGreetingService(IRepository<AlloGreeting> greetingRepository)
    {
        _greetingRepository = greetingRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets the greeting (returns the first record)
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<AlloGreeting> GetGreetingAsync()
    {
        return await _greetingRepository.Table.FirstOrDefaultAsync();
    }

    /// <summary>
    /// Inserts a greeting
    /// </summary>
    /// <param name="greeting">Greeting</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task InsertGreetingAsync(AlloGreeting greeting)
    {
        await _greetingRepository.InsertAsync(greeting);
    }

    /// <summary>
    /// Updates the greeting
    /// </summary>
    /// <param name="greeting">Greeting</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateGreetingAsync(AlloGreeting greeting)
    {
        await _greetingRepository.UpdateAsync(greeting);
    }

    #endregion
}
