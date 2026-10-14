using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<AddressController> _logger;

        /// <summary>
        /// Initializes a new instance of the AddressController.
        /// </summary>
        /// <param name="service">The address service.</param>
        /// <param name="logger">The logger instance.</param>
        public AddressController(IAddressService service, ILogger<AddressController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the most recent saved address.
        /// </summary>
        /// <returns>The address DTO if found; otherwise, 404 Not Found.</returns>
        [HttpGet]
        public async Task<ActionResult<AddressResponseDto>> GetAddress()
        {
            _logger.LogInformation("Handling GET /api/address request.");
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
            _logger.LogInformation("Handling POST /api/address request for {City}, {State}.", request.City, request.State);
            var savedAddress = await _service.SaveAddressAsync(request);
            return Ok(savedAddress);
        }
    }
}
