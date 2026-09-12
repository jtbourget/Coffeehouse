namespace Coffeehouse.Models
{
    /// <summary>
    /// Represents the user's address used to look up their local ballot.
    /// </summary>
    public class UserAddress
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Street address.
        /// </summary>
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// City.
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// State.
        /// </summary>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// ZIP code.
        /// </summary>
        public string ZipCode { get; set; } = string.Empty;
    }
}
