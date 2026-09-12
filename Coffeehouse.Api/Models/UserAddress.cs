using System.ComponentModel.DataAnnotations;

namespace Coffeehouse.Api.Models
{
    /// <summary>
    /// Stores the user's address, which is used to look up their local ballot.
    /// Only one address is stored at a time (the most recent entry).
    /// </summary>
    public class UserAddress
    {
        /// <summary>
        /// Unique identifier for the address record.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Street address (e.g., "123 Main St").
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// City name.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// State abbreviation or full name (e.g., "TX" or "Texas").
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// ZIP or postal code.
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string ZipCode { get; set; } = string.Empty;
    }
}
