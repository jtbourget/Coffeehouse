using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

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
