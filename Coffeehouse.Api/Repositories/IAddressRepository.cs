namespace Coffeehouse.Api.Repositories;

using Coffeehouse.Api.Models;

/// <summary>
/// Defines repository operations for managing user address data.
/// </summary>
public interface IAddressRepository
{
    Task<UserAddress?> GetAddressAsync();
    Task<UserAddress> SaveAddressAsync(UserAddress address);
}
