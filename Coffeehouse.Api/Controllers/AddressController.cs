using Coffeehouse.Api.Models.DTOs;
using Coffeehouse.Api.Repositories;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Coffeehouse.Api.Controllers
{
    /// <summary>
    /// Controller for managing the user's address.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _repository;
        private readonly IAddressMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the AddressController.
        /// </summary>
        /// <param name="repository">The address repository.</param>
        /// <param name="mapper">The address mapper.</param>
        public AddressController(IAddressRepository repository, IAddressMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves the most recent saved address.
        /// </summary>
        /// <returns>The address if found; otherwise, 404 Not Found.</returns>
        [HttpGet]
        public async Task<ActionResult<AddressResponseDto>> GetAddress()
        {
            var address = await _repository.GetAddressAsync();

            if (address == null)
            {
                return NotFound();
            }

            return Ok(_mapper.ToAddressDto(address));
        }

        /// <summary>
        /// Saves a new address or updates the existing one.
        /// </summary>
        /// <param name="request">The address to save.</param>
        /// <returns>The saved address.</returns>
        [HttpPost]
        public async Task<ActionResult<AddressResponseDto>> SaveAddress(SaveAddressRequestDto request)
        {
            var entity = _mapper.ToEntity(request);
            var savedAddress = await _repository.SaveAddressAsync(entity);
            
            return Ok(_mapper.ToAddressDto(savedAddress));
        }
    }
}
