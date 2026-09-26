using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Coffeehouse.Api.Data
{
    public static class WiCandidateSeeder
    {
        public static void SeedWisconsinElections(AppDbContext context)
        {
            var electionName = "2026 Wisconsin General Election";
            var election = context.Elections.Include(e => e.Contests).ThenInclude(c => c.Candidates)
                .FirstOrDefault(e => e.Name == electionName);
            
            bool isNew = false;
            if (election == null)
            {
                election = new Election { Name = electionName, ElectionDate = new System.DateTime(2026, 11, 3), Contests = new List<Contest>() };
                isNew = true;
            }

            var incomingContests = new List<Contest>
            {
                new Contest
                {
                    OfficeName = "GOVERNOR",
                    OcdId = "ocd-division/country:us/state:wi",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Tom Tiffany / David Varnam", Party = "Republican", PhotoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d6/Tom_Tiffany.jpg/960px-Tom_Tiffany.jpg", CampaignWebsite = "https://tomtiffany.com/" },
                        new Candidate { Name = "David Crowley / Sarah Godlewski", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=839", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "ATTORNEY GENERAL",
                    OcdId = "ocd-division/country:us/state:wi",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Eric Toney", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=821", CampaignWebsite = "" },
                        new Candidate { Name = "Josh Kaul", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=426", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "SECRETARY OF STATE",
                    OcdId = "ocd-division/country:us/state:wi",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Jay Schroeder", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=733", CampaignWebsite = "" },
                        new Candidate { Name = "JoCasta Zamarripa", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=140", CampaignWebsite = "" },
                        new Candidate { Name = "Pete Karas", Party = "Wisconsin", PhotoUrl = "https://picsum.photos/200?random=626", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE TREASURER",
                    OcdId = "ocd-division/country:us/state:wi",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "John S. Leiber", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=965", CampaignWebsite = "" },
                        new Candidate { Name = "Yee Leng Xiong", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=326", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE IN CONGRESS DISTRICT 1",
                    OcdId = "ocd-division/country:us/state:wi/cd:1",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Bryan Steil", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=578", CampaignWebsite = "" },
                        new Candidate { Name = "Mitchell Berman", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=207", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE IN CONGRESS DISTRICT 2",
                    OcdId = "ocd-division/country:us/state:wi/cd:2",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Mark Pocan", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=110", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE IN CONGRESS DISTRICT 3",
                    OcdId = "ocd-division/country:us/state:wi/cd:3",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Alexander Valiensi Kent", Party = "Independent", PhotoUrl = "https://picsum.photos/200?random=648", CampaignWebsite = "" },
                        new Candidate { Name = "Rustin Provance", Party = "Independent", PhotoUrl = "https://picsum.photos/200?random=140", CampaignWebsite = "" },
                        new Candidate { Name = "Derrick Van Orden", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=548", CampaignWebsite = "" },
                        new Candidate { Name = "Rebecca Cooke", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=115", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE IN CONGRESS DISTRICT 4",
                    OcdId = "ocd-division/country:us/state:wi/cd:4",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Arthur Burks", Party = "Independent", PhotoUrl = "https://picsum.photos/200?random=773", CampaignWebsite = "" },
                        new Candidate { Name = "Tim Rogers", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=927", CampaignWebsite = "" },
                        new Candidate { Name = "Gwen Moore", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=182", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE IN CONGRESS DISTRICT 5",
                    OcdId = "ocd-division/country:us/state:wi/cd:5",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Scott Fitzgerald", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=203", CampaignWebsite = "" },
                        new Candidate { Name = "Andrew Beck", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=943", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE IN CONGRESS DISTRICT 6",
                    OcdId = "ocd-division/country:us/state:wi/cd:6",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Elizabeth Anne Fitzgibbon", Party = "Independent", PhotoUrl = "https://picsum.photos/200?random=974", CampaignWebsite = "" },
                        new Candidate { Name = "Mike Thurow", Party = "Independent", PhotoUrl = "https://picsum.photos/200?random=434", CampaignWebsite = "" },
                        new Candidate { Name = "Glenn Grothman", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=971", CampaignWebsite = "" },
                        new Candidate { Name = "Brad Smith", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=428", CampaignWebsite = "" },
                        new Candidate { Name = "Matthew Arndt", Party = "Wisconsin", PhotoUrl = "https://picsum.photos/200?random=384", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE IN CONGRESS DISTRICT 7",
                    OcdId = "ocd-division/country:us/state:wi/cd:7",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Michael Alfonso", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=388", CampaignWebsite = "" },
                        new Candidate { Name = "Fred Clark", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=717", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE IN CONGRESS DISTRICT 8",
                    OcdId = "ocd-division/country:us/state:wi/cd:8",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Tony Wied", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=979", CampaignWebsite = "" },
                        new Candidate { Name = "Rick Crosson", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=478", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 1",
                    OcdId = "ocd-division/country:us/state:wi/sldu:1",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Mark Becker", Party = "Independent", PhotoUrl = "https://picsum.photos/200?random=363", CampaignWebsite = "" },
                        new Candidate { Name = "Nic Cravillion", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=252", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 3",
                    OcdId = "ocd-division/country:us/state:wi/sldu:3",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Tim Carpenter", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=710", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 5",
                    OcdId = "ocd-division/country:us/state:wi/sldu:5",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Mike Roberts", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=827", CampaignWebsite = "" },
                        new Candidate { Name = "Robyn Vining", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=271", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 7",
                    OcdId = "ocd-division/country:us/state:wi/sldu:7",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Mike Moeller", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=770", CampaignWebsite = "" },
                        new Candidate { Name = "Chris J. Larson", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=964", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 9",
                    OcdId = "ocd-division/country:us/state:wi/sldu:9",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Christian Ellis", Party = "Independent", PhotoUrl = "https://picsum.photos/200?random=479", CampaignWebsite = "" },
                        new Candidate { Name = "Amy Binsfeld", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=533", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 11",
                    OcdId = "ocd-division/country:us/state:wi/sldu:11",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Ellen Schutt", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=303", CampaignWebsite = "" },
                        new Candidate { Name = "Adam Duda", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=987", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 13",
                    OcdId = "ocd-division/country:us/state:wi/sldu:13",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "John Jagler", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=405", CampaignWebsite = "" },
                        new Candidate { Name = "Sasha Ripley", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=363", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 15",
                    OcdId = "ocd-division/country:us/state:wi/sldu:15",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Christopher Dean", Party = "Serving People Not Parties", PhotoUrl = "https://picsum.photos/200?random=787", CampaignWebsite = "" },
                        new Candidate { Name = "Scott Fleming", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=436", CampaignWebsite = "" },
                        new Candidate { Name = "Mark Spreitzer", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=227", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 17",
                    OcdId = "ocd-division/country:us/state:wi/sldu:17",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Howard Marklein", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=173", CampaignWebsite = "" },
                        new Candidate { Name = "Jenna Jacobson", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=298", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 19",
                    OcdId = "ocd-division/country:us/state:wi/sldu:19",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Rachael Ann Cabral-Guevara", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=216", CampaignWebsite = "" },
                        new Candidate { Name = "Emily Daniels Tseffos", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=112", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 21",
                    OcdId = "ocd-division/country:us/state:wi/sldu:21",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Jim Croft", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=321", CampaignWebsite = "" },
                        new Candidate { Name = "Trevor Jung", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=624", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 23",
                    OcdId = "ocd-division/country:us/state:wi/sldu:23",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Romaine Robert Quinn", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=681", CampaignWebsite = "" },
                        new Candidate { Name = "Jeff Foster", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=681", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 25",
                    OcdId = "ocd-division/country:us/state:wi/sldu:25",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Erik Severson", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=55", CampaignWebsite = "" },
                        new Candidate { Name = "Charly Ray", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=848", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 27",
                    OcdId = "ocd-division/country:us/state:wi/sldu:27",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Dianne Hesselbein", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=149", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 29",
                    OcdId = "ocd-division/country:us/state:wi/sldu:29",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Cory Tomczyk", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=474", CampaignWebsite = "" },
                        new Candidate { Name = "Gillian Battino", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=424", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 31",
                    OcdId = "ocd-division/country:us/state:wi/sldu:31",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Michele Magadance Skinner", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=834", CampaignWebsite = "" },
                        new Candidate { Name = "Jeff Smith", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=354", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "STATE SENATOR DISTRICT 33",
                    OcdId = "ocd-division/country:us/state:wi/sldu:33",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Chris Kapenga", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=382", CampaignWebsite = "" },
                        new Candidate { Name = "Mike Van Someren", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=684", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 1",
                    OcdId = "ocd-division/country:us/state:wi/sldl:1",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Joel Kitchens", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=353", CampaignWebsite = "" },
                        new Candidate { Name = "Renee A. Paplham", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=886", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 2",
                    OcdId = "ocd-division/country:us/state:wi/sldl:2",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Shae Sortwell", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=340", CampaignWebsite = "" },
                        new Candidate { Name = "Alicia Saunders", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=458", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 3",
                    OcdId = "ocd-division/country:us/state:wi/sldl:3",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Ron Tusler", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=83", CampaignWebsite = "" },
                        new Candidate { Name = "Michael J. Goodwin", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=852", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 4",
                    OcdId = "ocd-division/country:us/state:wi/sldl:4",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "David Steffen", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=130", CampaignWebsite = "" },
                        new Candidate { Name = "Alexia Unertl", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=584", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 5",
                    OcdId = "ocd-division/country:us/state:wi/sldl:5",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Joy Goeben", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=935", CampaignWebsite = "" },
                        new Candidate { Name = "Justin Schumacher", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=991", CampaignWebsite = "" },
                        new Candidate { Name = "David Schupbach", Party = "Wisconsin", PhotoUrl = "https://picsum.photos/200?random=408", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 6",
                    OcdId = "ocd-division/country:us/state:wi/sldl:6",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Elijah Behnke", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=371", CampaignWebsite = "" },
                        new Candidate { Name = "Shirley Hinze", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=32", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 7",
                    OcdId = "ocd-division/country:us/state:wi/sldl:7",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Lee Whiting", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=406", CampaignWebsite = "" },
                        new Candidate { Name = "Karen Kirsch", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=321", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 8",
                    OcdId = "ocd-division/country:us/state:wi/sldl:8",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Angel Sanchez", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=562", CampaignWebsite = "" },
                        new Candidate { Name = "Ismael Luna", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=724", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 9",
                    OcdId = "ocd-division/country:us/state:wi/sldl:9",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Sam Guerrero", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=160", CampaignWebsite = "" },
                        new Candidate { Name = "Priscilla A. Prado", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=783", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 10",
                    OcdId = "ocd-division/country:us/state:wi/sldl:10",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Darrin Madison", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=269", CampaignWebsite = "" },
                        new Candidate { Name = "Robert Longwell-Grice", Party = "Wisconsin", PhotoUrl = "https://picsum.photos/200?random=537", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 11",
                    OcdId = "ocd-division/country:us/state:wi/sldl:11",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Shandowlyon Reaves", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=755", CampaignWebsite = "" },
                        new Candidate { Name = "Sequanna Taylor", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=748", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 12",
                    OcdId = "ocd-division/country:us/state:wi/sldl:12",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Russell Antonio Goodwin, Sr.", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=784", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 13",
                    OcdId = "ocd-division/country:us/state:wi/sldl:13",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Mike Morgan", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=893", CampaignWebsite = "" },
                        new Candidate { Name = "Amy Zimmerman", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=301", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 14",
                    OcdId = "ocd-division/country:us/state:wi/sldl:14",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "AmyRose Murphy", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=679", CampaignWebsite = "" },
                        new Candidate { Name = "Angelito Tenorio", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=316", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 15",
                    OcdId = "ocd-division/country:us/state:wi/sldl:15",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Adam Neylon", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=375", CampaignWebsite = "" },
                        new Candidate { Name = "Stephen Tryon", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=948", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 16",
                    OcdId = "ocd-division/country:us/state:wi/sldl:16",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Alciro Deacon", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=850", CampaignWebsite = "" },
                        new Candidate { Name = "Kalan Haywood", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=171", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 17",
                    OcdId = "ocd-division/country:us/state:wi/sldl:17",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Charlene Abughrin", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=965", CampaignWebsite = "" },
                        new Candidate { Name = "Supreme Moore Omokunde", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=247", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 18",
                    OcdId = "ocd-division/country:us/state:wi/sldl:18",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Joel Richmond", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=209", CampaignWebsite = "" },
                        new Candidate { Name = "Margaret Arney", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=599", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 19",
                    OcdId = "ocd-division/country:us/state:wi/sldl:19",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Yasmine B. Outlaw", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=259", CampaignWebsite = "" },
                        new Candidate { Name = "Ryan Clancy", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=854", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 20",
                    OcdId = "ocd-division/country:us/state:wi/sldl:20",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Kyle Cleary", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=529", CampaignWebsite = "" },
                        new Candidate { Name = "Christine M. Sinicki", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=369", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 21",
                    OcdId = "ocd-division/country:us/state:wi/sldl:21",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Dylan Pfaffenbach", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=432", CampaignWebsite = "" },
                        new Candidate { Name = "Daniel J. Bukiewicz", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=134", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 22",
                    OcdId = "ocd-division/country:us/state:wi/sldl:22",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Paul Melotik", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=556", CampaignWebsite = "" },
                        new Candidate { Name = "Dana Glasstein", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=4", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 23",
                    OcdId = "ocd-division/country:us/state:wi/sldl:23",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Aleaner Pabonnie", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=208", CampaignWebsite = "" },
                        new Candidate { Name = "Deb Andraca", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=297", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 24",
                    OcdId = "ocd-division/country:us/state:wi/sldl:24",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Dan Knodl", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=749", CampaignWebsite = "" },
                        new Candidate { Name = "Matt Brown", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=309", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 25",
                    OcdId = "ocd-division/country:us/state:wi/sldl:25",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Paul Tittl", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=312", CampaignWebsite = "" },
                        new Candidate { Name = "Christopher Able", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=704", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 26",
                    OcdId = "ocd-division/country:us/state:wi/sldl:26",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "John Belanger", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=108", CampaignWebsite = "" },
                        new Candidate { Name = "Joe Sheehan", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=236", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 27",
                    OcdId = "ocd-division/country:us/state:wi/sldl:27",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Lindee Brill", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=449", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 28",
                    OcdId = "ocd-division/country:us/state:wi/sldl:28",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Rob Kreibich", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=932", CampaignWebsite = "" },
                        new Candidate { Name = "Robin Lillesve", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=533", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 29",
                    OcdId = "ocd-division/country:us/state:wi/sldl:29",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Treig Pronschinske", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=35", CampaignWebsite = "" },
                        new Candidate { Name = "Chris Danou", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=517", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 30",
                    OcdId = "ocd-division/country:us/state:wi/sldl:30",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Shannon Zimmerman", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=637", CampaignWebsite = "" },
                        new Candidate { Name = "Kevin Knoke", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=61", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 31",
                    OcdId = "ocd-division/country:us/state:wi/sldl:31",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Tyler August", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=813", CampaignWebsite = "" },
                        new Candidate { Name = "John Perryman", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=5", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 32",
                    OcdId = "ocd-division/country:us/state:wi/sldl:32",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Amanda Nedweski", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=867", CampaignWebsite = "" },
                        new Candidate { Name = "Greg Miller", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=754", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 33",
                    OcdId = "ocd-division/country:us/state:wi/sldl:33",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Steve Wicklund", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=750", CampaignWebsite = "" },
                        new Candidate { Name = "Maria Elena Bisabarros", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=23", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 34",
                    OcdId = "ocd-division/country:us/state:wi/sldl:34",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Rob Swearingen", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=921", CampaignWebsite = "" },
                        new Candidate { Name = "Merlin Van Buren", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=38", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 35",
                    OcdId = "ocd-division/country:us/state:wi/sldl:35",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Calvin Callahan", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=635", CampaignWebsite = "" },
                        new Candidate { Name = "Elizabeth McCrank", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=196", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 36",
                    OcdId = "ocd-division/country:us/state:wi/sldl:36",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Shena Chapman", Party = "Independent", PhotoUrl = "https://picsum.photos/200?random=255", CampaignWebsite = "" },
                        new Candidate { Name = "Jeffrey L. Mursau", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=792", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 37",
                    OcdId = "ocd-division/country:us/state:wi/sldl:37",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Mark Born", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=395", CampaignWebsite = "" },
                        new Candidate { Name = "LaToya Bates", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=740", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 38",
                    OcdId = "ocd-division/country:us/state:wi/sldl:38",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "William Penterman", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=880", CampaignWebsite = "" },
                        new Candidate { Name = "Terri Wenkman", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=653", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 39",
                    OcdId = "ocd-division/country:us/state:wi/sldl:39",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Alex Dallman", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=981", CampaignWebsite = "" },
                        new Candidate { Name = "Michael Skivington", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=56", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 40",
                    OcdId = "ocd-division/country:us/state:wi/sldl:40",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Julie Helmer", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=570", CampaignWebsite = "" },
                        new Candidate { Name = "Karen DeSanto", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=485", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 41",
                    OcdId = "ocd-division/country:us/state:wi/sldl:41",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Tony Kurtz", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=643", CampaignWebsite = "" },
                        new Candidate { Name = "Zach Commons", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=230", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 42",
                    OcdId = "ocd-division/country:us/state:wi/sldl:42",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Keith F. Miller", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=637", CampaignWebsite = "" },
                        new Candidate { Name = "Maureen McCarville", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=738", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 43",
                    OcdId = "ocd-division/country:us/state:wi/sldl:43",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Paul McGraw", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=125", CampaignWebsite = "" },
                        new Candidate { Name = "Brienne Brown", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=502", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 44",
                    OcdId = "ocd-division/country:us/state:wi/sldl:44",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Ron Woodman", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=881", CampaignWebsite = "" },
                        new Candidate { Name = "Ann Roe", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=999", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 45",
                    OcdId = "ocd-division/country:us/state:wi/sldl:45",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Jocelyn Jordan", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=925", CampaignWebsite = "" },
                        new Candidate { Name = "Clinton Anderson", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=706", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 46",
                    OcdId = "ocd-division/country:us/state:wi/sldl:46",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "John Donohue", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=951", CampaignWebsite = "" },
                        new Candidate { Name = "Joan Fitzgerald", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=540", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 47",
                    OcdId = "ocd-division/country:us/state:wi/sldl:47",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Sandy Bakk", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=831", CampaignWebsite = "" },
                        new Candidate { Name = "Randy Udell", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=698", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 48",
                    OcdId = "ocd-division/country:us/state:wi/sldl:48",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Mark Kjorlie", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=458", CampaignWebsite = "" },
                        new Candidate { Name = "Andrew Hysell", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=78", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 49",
                    OcdId = "ocd-division/country:us/state:wi/sldl:49",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Travis Tranel", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=69", CampaignWebsite = "" },
                        new Candidate { Name = "John Rindy", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=541", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 50",
                    OcdId = "ocd-division/country:us/state:wi/sldl:50",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Jon Aleckson", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=770", CampaignWebsite = "" },
                        new Candidate { Name = "Bill Oemichen", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=581", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 51",
                    OcdId = "ocd-division/country:us/state:wi/sldl:51",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Nathan Tataje", Party = "American", PhotoUrl = "https://picsum.photos/200?random=701", CampaignWebsite = "" },
                        new Candidate { Name = "Todd Novak", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=601", CampaignWebsite = "" },
                        new Candidate { Name = "Ben Gruber", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=810", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 52",
                    OcdId = "ocd-division/country:us/state:wi/sldl:52",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Reive Pullen", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=169", CampaignWebsite = "" },
                        new Candidate { Name = "Lee Snodgrass", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=834", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 53",
                    OcdId = "ocd-division/country:us/state:wi/sldl:53",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Rachael Dowling", Party = "Independent", PhotoUrl = "https://picsum.photos/200?random=601", CampaignWebsite = "" },
                        new Candidate { Name = "David Daniels", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=939", CampaignWebsite = "" },
                        new Candidate { Name = "Becky Nichols", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=966", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 54",
                    OcdId = "ocd-division/country:us/state:wi/sldl:54",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Tim Paterson", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=303", CampaignWebsite = "" },
                        new Candidate { Name = "Lori Palmeri", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=221", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 55",
                    OcdId = "ocd-division/country:us/state:wi/sldl:55",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Nate Gustafson", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=844", CampaignWebsite = "" },
                        new Candidate { Name = "Alex Corrigan", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=610", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 56",
                    OcdId = "ocd-division/country:us/state:wi/sldl:56",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Anthony W. Phillips", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=686", CampaignWebsite = "" },
                        new Candidate { Name = "Grace Abitz", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=946", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 57",
                    OcdId = "ocd-division/country:us/state:wi/sldl:57",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Kevin Krentz", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=11", CampaignWebsite = "" },
                        new Candidate { Name = "Joey Marschall", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=80", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 58",
                    OcdId = "ocd-division/country:us/state:wi/sldl:58",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Bernie Newman", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=930", CampaignWebsite = "" },
                        new Candidate { Name = "Dennis D. Degenhardt", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=505", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 59",
                    OcdId = "ocd-division/country:us/state:wi/sldl:59",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Bradley Petersen", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=915", CampaignWebsite = "" },
                        new Candidate { Name = "Jack Holzman", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=200", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 60",
                    OcdId = "ocd-division/country:us/state:wi/sldl:60",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Tiffany Brault", Party = "Independent", PhotoUrl = "https://picsum.photos/200?random=384", CampaignWebsite = "" },
                        new Candidate { Name = "Marty Ryan", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=455", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 61",
                    OcdId = "ocd-division/country:us/state:wi/sldl:61",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Bob Donovan", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=739", CampaignWebsite = "" },
                        new Candidate { Name = "Ben Brist", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=155", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 62",
                    OcdId = "ocd-division/country:us/state:wi/sldl:62",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Mike Bellagio", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=743", CampaignWebsite = "" },
                        new Candidate { Name = "Angelina M. Cruz", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=17", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 63",
                    OcdId = "ocd-division/country:us/state:wi/sldl:63",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Robert Wittke", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=366", CampaignWebsite = "" },
                        new Candidate { Name = "Eddie Phanichkul", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=243", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 64",
                    OcdId = "ocd-division/country:us/state:wi/sldl:64",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Ed Hibsch", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=371", CampaignWebsite = "" },
                        new Candidate { Name = "Tip McGuire", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=120", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 65",
                    OcdId = "ocd-division/country:us/state:wi/sldl:65",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Valerie Kretchmer", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=936", CampaignWebsite = "" },
                        new Candidate { Name = "Ben DeSmidt", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=757", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 66",
                    OcdId = "ocd-division/country:us/state:wi/sldl:66",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Gina Cefalu Paulick", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=306", CampaignWebsite = "" },
                        new Candidate { Name = "Greta Neubauer", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=923", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 67",
                    OcdId = "ocd-division/country:us/state:wi/sldl:67",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "David Armstrong", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=642", CampaignWebsite = "" },
                        new Candidate { Name = "Indiana Thompson", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=486", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 68",
                    OcdId = "ocd-division/country:us/state:wi/sldl:68",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Rob Summerfield", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=592", CampaignWebsite = "" },
                        new Candidate { Name = "Elisha King", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=781", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 69",
                    OcdId = "ocd-division/country:us/state:wi/sldl:69",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Josh Kelley", Party = "Independent", PhotoUrl = "https://picsum.photos/200?random=299", CampaignWebsite = "" },
                        new Candidate { Name = "Karen Hurd", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=163", CampaignWebsite = "" },
                        new Candidate { Name = "Roger Halls", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=479", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 70",
                    OcdId = "ocd-division/country:us/state:wi/sldl:70",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Nancy VanderMeer", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=103", CampaignWebsite = "" },
                        new Candidate { Name = "Stephanie Stuve Bodeen", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=776", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 71",
                    OcdId = "ocd-division/country:us/state:wi/sldl:71",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Jeff Disher", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=968", CampaignWebsite = "" },
                        new Candidate { Name = "Vinnie Miresse", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=43", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 72",
                    OcdId = "ocd-division/country:us/state:wi/sldl:72",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Scott Krug", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=814", CampaignWebsite = "" },
                        new Candidate { Name = "Christine Maltese", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=89", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 73",
                    OcdId = "ocd-division/country:us/state:wi/sldl:73",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Frank Kostka", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=179", CampaignWebsite = "" },
                        new Candidate { Name = "Angela Stroud", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=841", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 74",
                    OcdId = "ocd-division/country:us/state:wi/sldl:74",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Chanz Green", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=932", CampaignWebsite = "" },
                        new Candidate { Name = "Paul Johnson", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=613", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 75",
                    OcdId = "ocd-division/country:us/state:wi/sldl:75",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Duke Tucker", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=311", CampaignWebsite = "" },
                        new Candidate { Name = "Keith Mogel", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=875", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 76",
                    OcdId = "ocd-division/country:us/state:wi/sldl:76",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Nina Chat", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=656", CampaignWebsite = "" },
                        new Candidate { Name = "Dina Nina Martinez-Rutherford", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=979", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 77",
                    OcdId = "ocd-division/country:us/state:wi/sldl:77",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Jane McCormick", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=598", CampaignWebsite = "" },
                        new Candidate { Name = "Renuka Mayadev", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=361", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 78",
                    OcdId = "ocd-division/country:us/state:wi/sldl:78",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Henry Johnson", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=674", CampaignWebsite = "" },
                        new Candidate { Name = "Shelia Stubbs", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=660", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 79",
                    OcdId = "ocd-division/country:us/state:wi/sldl:79",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "John Fons", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=569", CampaignWebsite = "" },
                        new Candidate { Name = "Lisa Subeck", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=688", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 80",
                    OcdId = "ocd-division/country:us/state:wi/sldl:80",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Simran Arora", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=238", CampaignWebsite = "" },
                        new Candidate { Name = "Mike Bare", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=178", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 81",
                    OcdId = "ocd-division/country:us/state:wi/sldl:81",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Mark S. Maier", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=976", CampaignWebsite = "" },
                        new Candidate { Name = "Alex Joers", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=504", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 82",
                    OcdId = "ocd-division/country:us/state:wi/sldl:82",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Bryson Reyes", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=803", CampaignWebsite = "" },
                        new Candidate { Name = "Rico Camacho", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=585", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 83",
                    OcdId = "ocd-division/country:us/state:wi/sldl:83",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Dave Maxey", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=320", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 84",
                    OcdId = "ocd-division/country:us/state:wi/sldl:84",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Chuck Wichgers", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=601", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 85",
                    OcdId = "ocd-division/country:us/state:wi/sldl:85",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Patrick Snyder", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=134", CampaignWebsite = "" },
                        new Candidate { Name = "John Kroll", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=37", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 86",
                    OcdId = "ocd-division/country:us/state:wi/sldl:86",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "John Spiros", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=308", CampaignWebsite = "" },
                        new Candidate { Name = "Andy Wuethrich", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=323", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 87",
                    OcdId = "ocd-division/country:us/state:wi/sldl:87",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Brent Jacobson", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=281", CampaignWebsite = "" },
                        new Candidate { Name = "Bob Look", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=437", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 88",
                    OcdId = "ocd-division/country:us/state:wi/sldl:88",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Ben Franklin", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=905", CampaignWebsite = "" },
                        new Candidate { Name = "Brandy Tollefson", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=931", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 89",
                    OcdId = "ocd-division/country:us/state:wi/sldl:89",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Bobby R. Lindsey", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=197", CampaignWebsite = "" },
                        new Candidate { Name = "Ryan Spaude", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=471", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 90",
                    OcdId = "ocd-division/country:us/state:wi/sldl:90",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Jessica Henderson", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=563", CampaignWebsite = "" },
                        new Candidate { Name = "Amaad Rivera-Wagner", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=726", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 91",
                    OcdId = "ocd-division/country:us/state:wi/sldl:91",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Bruce Stabenow", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=99", CampaignWebsite = "" },
                        new Candidate { Name = "Jodi Emerson", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=852", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 92",
                    OcdId = "ocd-division/country:us/state:wi/sldl:92",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Clint Moses", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=594", CampaignWebsite = "" },
                        new Candidate { Name = "Jeremiah Fredrickson", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=328", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 93",
                    OcdId = "ocd-division/country:us/state:wi/sldl:93",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Michael Ayala", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=41", CampaignWebsite = "" },
                        new Candidate { Name = "Christian Phelps", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=932", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 94",
                    OcdId = "ocd-division/country:us/state:wi/sldl:94",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Keith Purnell", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=751", CampaignWebsite = "" },
                        new Candidate { Name = "Steve Doyle", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=729", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 95",
                    OcdId = "ocd-division/country:us/state:wi/sldl:95",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Paul Michael Weber", Party = "Independent", PhotoUrl = "https://picsum.photos/200?random=698", CampaignWebsite = "" },
                        new Candidate { Name = "Cedric Schnitzler", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=31", CampaignWebsite = "" },
                        new Candidate { Name = "Jill Billings", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=664", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 96",
                    OcdId = "ocd-division/country:us/state:wi/sldl:96",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Jim Green", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=811", CampaignWebsite = "" },
                        new Candidate { Name = "Tara Johnson", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=531", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 97",
                    OcdId = "ocd-division/country:us/state:wi/sldl:97",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Cindi Duchow", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=839", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 98",
                    OcdId = "ocd-division/country:us/state:wi/sldl:98",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Jim Piwowarczyk", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=834", CampaignWebsite = "" },
                        new Candidate { Name = "Matt Philibert", Party = "Democratic", PhotoUrl = "https://picsum.photos/200?random=179", CampaignWebsite = "" }
                    }
                },
                new Contest
                {
                    OfficeName = "REPRESENTATIVE TO THE ASSEMBLY DISTRICT 99",
                    OcdId = "ocd-division/country:us/state:wi/sldl:99",
                    Candidates = new List<Candidate>
                    {
                        new Candidate { Name = "Barbara Dittrich", Party = "Republican", PhotoUrl = "https://picsum.photos/200?random=727", CampaignWebsite = "" }
                    }
                }
            };

            foreach (var incContest in incomingContests)
            {
                var existingContest = election.Contests.FirstOrDefault(c => c.OfficeName == incContest.OfficeName);
                if (existingContest == null)
                {
                    election.Contests.Add(incContest);
                }
                else
                {
                    foreach (var incCand in incContest.Candidates)
                    {
                        var existingCand = existingContest.Candidates.FirstOrDefault(c => c.Name == incCand.Name);
                        if (existingCand == null)
                        {
                            existingContest.Candidates.Add(incCand);
                        }
                        else
                        {
                            // Upsert logic: Update the picture, party, and website if changed
                            existingCand.PhotoUrl = incCand.PhotoUrl;
                            existingCand.Party = incCand.Party;
                            existingCand.CampaignWebsite = incCand.CampaignWebsite;
                        }
                    }
                }
            }

            if (isNew) context.Elections.Add(election);
            context.SaveChanges();
        }
    }
}
