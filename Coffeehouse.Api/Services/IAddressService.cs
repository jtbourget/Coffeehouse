namespace Coffeehouse.Api.Services;

using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;

/// <summary>
/// Service interface for managing user address operations.
/// </summary>
public interface IAddressService
{
    Task<AddressResponseDto?> GetAddressAsync();
    Task<AddressResponseDto> SaveAddressAsync(SaveAddressRequestDto request);
}
