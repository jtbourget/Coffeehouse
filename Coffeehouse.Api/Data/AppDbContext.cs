using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Data
{
    /// <summary>
    /// The database context for the Coffeehouse application.
    /// Manages the entity objects during runtime, which includes fetching data from the database
    /// and saving data back to the database.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the AppDbContext.
        /// </summary>
        /// <param name="options">The options to be used by a DbContext.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the Elections DbSet.
        /// </summary>
        public DbSet<Election> Elections { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Contests DbSet.
        /// </summary>
        public DbSet<Contest> Contests { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Candidates DbSet.
        /// </summary>
        public DbSet<Candidate> Candidates { get; set; } = null!;

        /// <summary>
        /// Gets or sets the FavoriteCandidates DbSet.
        /// </summary>
        public DbSet<FavoriteCandidate> FavoriteCandidates { get; set; } = null!;

        /// <summary>
        /// Gets or sets the UserAddresses DbSet.
        /// </summary>
        public DbSet<UserAddress> UserAddresses { get; set; } = null!;

        /// <summary>
        /// Configures the schema needed for the context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique index for FavoriteCandidate
            modelBuilder.Entity<FavoriteCandidate>()
                .HasIndex(f => f.ContestId)
                .IsUnique();

            // Configure relationships
            modelBuilder.Entity<Contest>()
                .HasOne(c => c.Election)
                .WithMany(e => e.Contests)
                .HasForeignKey(c => c.ElectionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Candidate>()
                .HasOne(c => c.Contest)
                .WithMany(co => co.Candidates)
                .HasForeignKey(c => c.ContestId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<FavoriteCandidate>()
                .HasOne(f => f.Contest)
                .WithMany()
                .HasForeignKey(f => f.ContestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FavoriteCandidate>()
                .HasOne(f => f.Candidate)
                .WithMany()
                .HasForeignKey(f => f.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
