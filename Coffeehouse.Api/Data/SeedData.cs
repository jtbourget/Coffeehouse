using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Data
{
    /// <summary>
    /// Provides data seeding functionality for the database.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Initializes the database with seed data if it's empty.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public static void Initialize(AppDbContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            if (context.Elections.Any())
            {
                // Fix broken placeholder URLs in existing DB
                var candidates = context.Set<Candidate>().ToList();
                bool modified = false;
                foreach (var c in candidates) 
                {
                    if (c.PhotoUrl != null)
                    {
                        if (c.PhotoUrl.Contains("via.placeholder.com") || c.PhotoUrl.Contains("dummyimage.com"))
                        {
                            c.PhotoUrl = "https://picsum.photos/200";
                            modified = true;
                        }
                    }
                }
                if (modified) context.SaveChanges();

                return;   // DB has been seeded
            }

            var elections = new List<Election>
            {
                new Election
                {
                    Name = "2026 General Election",
                    ElectionDate = new DateTime(2026, 11, 3),
                    VotingLocationName = "Lincoln Community Center",
                    VotingLocationAddress = "450 S Main St, Springfield, IL 62701",
                    Contests = new List<Contest>
                    {
                        new Contest
                        {
                            OfficeName = "Governor",
                            Candidates = new List<Candidate>
                            {
                                new Candidate { Name = "Maria Torres", Party = "Democratic", Bio = "Current Lt. Governor with 12 years of public service. Focused on education reform, clean energy, and expanding healthcare access.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/mariatorres" },
                                new Candidate { Name = "James Mitchell", Party = "Republican", Bio = "Former state senator and small business owner. Advocates for tax reform, economic growth, and infrastructure investment.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/jamesmitchell" },
                                new Candidate { Name = "Priya Patel", Party = "Independent", Bio = "Community organizer and nonprofit director. Champions campaign finance reform, affordable housing, and environmental justice.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/priyapatel" }
                            }
                        },
                        new Contest
                        {
                            OfficeName = "U.S. Senator",
                            Candidates = new List<Candidate>
                            {
                                new Candidate { Name = "David Chen", Party = "Democratic", Bio = "Two-term congressman known for bipartisan legislation on veterans' affairs and technology policy.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/davidchen" },
                                new Candidate { Name = "Sarah Williams", Party = "Republican", Bio = "Former state attorney general. Prioritizes border security, fiscal responsibility, and small business support.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/sarahwilliams" }
                            }
                        },
                        new Contest
                        {
                            OfficeName = "State Representative, District 5",
                            Candidates = new List<Candidate>
                            {
                                new Candidate { Name = "Marcus Johnson", Party = "Democratic", Bio = "High school teacher and union advocate. Focuses on public education funding and workers' rights.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/marcusjohnson" },
                                new Candidate { Name = "Elena Rodriguez", Party = "Republican", Bio = "City council member and local business owner. Supports property tax relief and public safety improvements.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/elenarodriguez" }
                            }
                        },
                        new Contest
                        {
                            OfficeName = "Mayor",
                            Candidates = new List<Candidate>
                            {
                                new Candidate { Name = "Robert Kim", Party = "Democratic", Bio = "Current city council president. Plans to expand public transit, invest in downtown revitalization, and address homelessness.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/robertkim" },
                                new Candidate { Name = "Angela Foster", Party = "Republican", Bio = "Real estate developer focused on economic development, reducing city debt, and streamlining local government.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/angelafoster" },
                                new Candidate { Name = "Jamal Wright", Party = "Independent", Bio = "Local pastor and community leader. Advocates for youth programs, police reform, and neighborhood investment.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/jamalwright" }
                            }
                        },
                        new Contest
                        {
                            OfficeName = "School Board, At-Large",
                            Candidates = new List<Candidate>
                            {
                                new Candidate { Name = "Patricia Nguyen", Party = "Nonpartisan", Bio = "Parent and former PTA president. Supports increased teacher pay, updated curricula, and school safety improvements.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/patricianguyen" },
                                new Candidate { Name = "Thomas Baker", Party = "Nonpartisan", Bio = "Retired principal with 30 years in education. Focuses on STEM programs, vocational training, and special education resources.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/thomasbaker" }
                            }
                        }
                    }
                },
                new Election
                {
                    Name = "2026 Primary Election",
                    ElectionDate = new DateTime(2026, 3, 10),
                    VotingLocationName = "Riverside High School Gymnasium",
                    VotingLocationAddress = "800 Oak Ave, Springfield, IL 62704",
                    Contests = new List<Contest>
                    {
                        new Contest
                        {
                            OfficeName = "Governor (Primary)",
                            Candidates = new List<Candidate>
                            {
                                new Candidate { Name = "Maria Torres", Party = "Democratic", Bio = "Current Lt. Governor with 12 years of public service. Focused on education reform, clean energy, and expanding healthcare access.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/mariatorres" },
                                new Candidate { Name = "Lisa Chang", Party = "Democratic", Bio = "State treasurer with a background in economics. Focuses on fiscal transparency and green infrastructure.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/lisachang" }
                            }
                        },
                        new Contest
                        {
                            OfficeName = "U.S. Senator (Primary)",
                            Candidates = new List<Candidate>
                            {
                                new Candidate { Name = "David Chen", Party = "Democratic", Bio = "Two-term congressman known for bipartisan legislation on veterans' affairs and technology policy.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/davidchen" },
                                new Candidate { Name = "Michael Brown", Party = "Democratic", Bio = "Labor union leader and veterans' advocate. Pushes for fair wages, healthcare expansion, and housing affordability.", PhotoUrl = "https://picsum.photos/200", CampaignWebsite = "https://example.com/michaelbrown" }
                            }
                        }
                    }
                }
            };

            context.Elections.AddRange(elections);
            context.SaveChanges();
        }
    }
}
