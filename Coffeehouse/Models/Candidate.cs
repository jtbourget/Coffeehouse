namespace Coffeehouse.Models
{
    /// <summary>
    /// Represents a candidate running for office.
    /// Contains their personal info, photo, campaign website, and biography.
    /// </summary>
    public class Candidate
    {
        /// <summary>
        /// Unique identifier for the candidate.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the contest this candidate is running in.
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// Full name of the candidate.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Political party (e.g., "Democratic", "Republican").
        /// </summary>
        public string Party { get; set; } = string.Empty;

        /// <summary>
        /// URL to the candidate's photo.
        /// </summary>
        public string PhotoUrl { get; set; } = string.Empty;

        /// <summary>
        /// URL to the candidate's campaign website.
        /// </summary>
        public string CampaignWebsite { get; set; } = string.Empty;

        /// <summary>
        /// Biographical info and platform summary.
        /// </summary>
        public string Bio { get; set; } = string.Empty;
    }
}
