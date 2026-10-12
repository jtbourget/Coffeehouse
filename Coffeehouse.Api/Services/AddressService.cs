namespace Coffeehouse.Api.Services;

using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Repositories;

/// <summary>
/// Scoped service for user address operations.
/// Depends on the IAddressRepository interface.
/// </summary>
public class AddressService : IAddressService
{
    private readonly IAddressRepository _repository;
    private readonly IAddressMapper _mapper;

    public AddressService(IAddressRepository repository, IAddressMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AddressResponseDto?> GetAddressAsync()
    {
        var address = await _repository.GetAddressAsync();
        return address == null ? null : _mapper.ToAddressDto(address);
    }

    public async Task<AddressResponseDto> SaveAddressAsync(SaveAddressRequestDto request)
    {
        var entity = _mapper.ToEntity(request);
        var savedAddress = await _repository.SaveAddressAsync(entity);
        return _mapper.ToAddressDto(savedAddress);
    }
}
