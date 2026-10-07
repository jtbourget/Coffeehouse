using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// SQL-based implementation of the address repository using Entity Framework Core.
/// </summary>
public class SqlAddressRepository : IAddressRepository
{
    private readonly AppDbContext _context;

    public SqlAddressRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserAddress?> GetAddressAsync()
    {
        return await _context.UserAddresses
            .OrderByDescending(a => a.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<UserAddress> SaveAddressAsync(UserAddress address)
    {
        var existingAddress = await _context.UserAddresses.FirstOrDefaultAsync();

        if (existingAddress == null)
        {
            _context.UserAddresses.Add(address);
            existingAddress = address;
        }
        else
        {
            existingAddress.Street = address.Street;
            existingAddress.City = address.City;
            existingAddress.State = address.State;
            existingAddress.ZipCode = address.ZipCode;
        }

        await _context.SaveChangesAsync();
        return existingAddress;
    }
}
