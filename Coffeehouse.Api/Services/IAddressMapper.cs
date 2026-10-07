using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Defines mapping operations between user address entities and data transfer objects.
/// </summary>
public interface IAddressMapper
{
    AddressResponseDto ToAddressDto(UserAddress address);
    UserAddress ToEntity(SaveAddressRequestDto request);
}
