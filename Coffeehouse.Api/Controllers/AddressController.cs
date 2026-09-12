using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Controllers
{
    /// <summary>
    /// Controller for managing the user's address.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the AddressController.
        /// </summary>
        /// <param name="context">The database context.</param>
        public AddressController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves the most recent saved address.
        /// </summary>
        /// <returns>The address if found; otherwise, 404 Not Found.</returns>
        [HttpGet]
        public async Task<ActionResult<UserAddress>> GetAddress()
        {
            var address = await _context.UserAddresses
                .OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }

        /// <summary>
        /// Saves a new address or updates the existing one.
        /// </summary>
        /// <param name="address">The address to save.</param>
        /// <returns>The saved address.</returns>
        [HttpPost]
        public async Task<ActionResult<UserAddress>> SaveAddress(UserAddress address)
        {
            var existingAddress = await _context.UserAddresses.FirstOrDefaultAsync();

            if (existingAddress == null)
            {
                _context.UserAddresses.Add(address);
            }
            else
            {
                existingAddress.Street = address.Street;
                existingAddress.City = address.City;
                existingAddress.State = address.State;
                existingAddress.ZipCode = address.ZipCode;
            }

            await _context.SaveChangesAsync();
            
            var savedAddress = existingAddress ?? address;
            return Ok(savedAddress);
        }
    }
}
