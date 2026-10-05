namespace Coffeehouse.Api.Repositories;

using Coffeehouse.Api.Models;

public interface IAddressRepository
{
    Task<UserAddress?> GetAddressAsync();
    Task<UserAddress> SaveAddressAsync(UserAddress address);
}
