using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Coffeehouse.Api.Controllers
{
    /// <summary>
    /// Controller for managing the user's address.
    /// Depends on the IAddressService interface. Never returns domain models directly.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _service;

        /// <summary>
        /// Initializes a new instance of the AddressController.
        /// </summary>
        /// <param name="service">The address service.</param>
        public AddressController(IAddressService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves the most recent saved address.
        /// </summary>
        /// <returns>The address DTO if found; otherwise, 404 Not Found.</returns>
        [HttpGet]
        public async Task<ActionResult<AddressResponseDto>> GetAddress()
        {
            var address = await _service.GetAddressAsync();

            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        /// <summary>
        /// Saves a new address or updates the existing one.
        /// </summary>
        /// <param name="request">The address request DTO to save.</param>
        /// <returns>The saved address response DTO.</returns>
        [HttpPost]
        public async Task<ActionResult<AddressResponseDto>> SaveAddress(SaveAddressRequestDto request)
        {
            var savedAddress = await _service.SaveAddressAsync(request);
            return Ok(savedAddress);
        }
    }
}
