using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// SQL-based implementation of the address repository using Entity Framework Core.
/// Uses IServiceScopeFactory so it can safely be registered as a Singleton.
/// </summary>
public class SqlAddressRepository : IAddressRepository
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SqlAddressRepository(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<UserAddress?> GetAddressAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.UserAddresses
            .OrderByDescending(a => a.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<UserAddress> SaveAddressAsync(UserAddress address)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var existingAddress = await context.UserAddresses.FirstOrDefaultAsync();

        if (existingAddress == null)
        {
            context.UserAddresses.Add(address);
            existingAddress = address;
        }
        else
        {
            existingAddress.Street = address.Street;
            existingAddress.City = address.City;
            existingAddress.State = address.State;
            existingAddress.ZipCode = address.ZipCode;
        }

        await context.SaveChangesAsync();
        return existingAddress;
    }
}
