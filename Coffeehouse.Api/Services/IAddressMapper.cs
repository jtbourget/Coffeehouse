using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Defines mapping operations between user address entities and data transfer objects.
/// </summary>
public interface IAddressMapper
{
    AddressResponseDto ToAddressDto(UserAddress address);
    UserAddress ToEntity(SaveAddressRequestDto request);
}
