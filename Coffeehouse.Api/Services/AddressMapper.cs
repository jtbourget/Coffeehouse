using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Provides mapping functionality between user address entities and data transfer objects.
/// </summary>
public class AddressMapper : IAddressMapper
{
    public AddressResponseDto ToAddressDto(UserAddress address)
    {
        return new AddressResponseDto
        {
            Id = address.Id,
            Street = address.Street ?? string.Empty,
            City = address.City ?? string.Empty,
            State = address.State ?? string.Empty,
            ZipCode = address.ZipCode ?? string.Empty
        };
    }

    public UserAddress ToEntity(SaveAddressRequestDto request)
    {
        return new UserAddress
        {
            Street = request.Street,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode
        };
    }
}
