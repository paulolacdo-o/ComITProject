using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OurFiction.Models;

namespace OurFiction.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Entry> Entries { get; set; }
        public DbSet<EntryCycle> EntryCycles { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryFragment> StoryFragments { get; set; }
    }
}
