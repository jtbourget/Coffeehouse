using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

public interface IAddressMapper
{
    AddressResponseDto ToAddressDto(UserAddress address);
    UserAddress ToEntity(SaveAddressRequestDto request);
}
