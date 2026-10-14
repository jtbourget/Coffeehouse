namespace Coffeehouse.Api.Services;

using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Repositories;
using Microsoft.Extensions.Logging;

/// <summary>
/// Scoped service for user address operations.
/// Depends on the IAddressRepository interface.
/// </summary>
public class AddressService : IAddressService
{
    private readonly IAddressRepository _repository;
    private readonly IAddressMapper _mapper;
    private readonly ILogger<AddressService> _logger;

    public AddressService(IAddressRepository repository, IAddressMapper mapper, ILogger<AddressService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AddressResponseDto?> GetAddressAsync()
    {
        _logger.LogDebug("Retrieving saved user address.");
        var address = await _repository.GetAddressAsync();
        if (address == null)
        {
            _logger.LogInformation("No saved address found.");
            return null;
        }

        return _mapper.ToAddressDto(address);
    }

    public async Task<AddressResponseDto> SaveAddressAsync(SaveAddressRequestDto request)
    {
        _logger.LogInformation("Saving user address: {Street}, {City}, {State} {ZipCode}", request.Street, request.City, request.State, request.ZipCode);
        var entity = _mapper.ToEntity(request);
        var savedAddress = await _repository.SaveAddressAsync(entity);
        _logger.LogInformation("User address saved successfully with ID {Id}.", savedAddress.Id);
        return _mapper.ToAddressDto(savedAddress);
    }
}
