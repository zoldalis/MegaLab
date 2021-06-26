using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Controller> Controllers { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Controller>()
            //.Property<string>("ValueCollection")
            //.HasField("_values");

            modelBuilder
                .Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder
            .Entity<IdentityUserRole<string>>().HasNoKey();
            modelBuilder
            .Entity<IdentityUserToken<string>>().HasNoKey();
            modelBuilder
            .Entity<IdentityUser<string>>().HasNoKey();
            modelBuilder
            .Entity<IdentityUserClaim<string>>().HasNoKey();
            modelBuilder
            .Entity<IdentityRole<string>>().HasNoKey();
            modelBuilder
           .Entity<IdentityRoleClaim<string>>().HasNoKey();

            var valueComparer = new ValueComparer<string[]>(
    (c1, c2) => c1.SequenceEqual(c2),
    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
    c => c.ToList().ToArray());

            modelBuilder
    .Entity<Controller>()
    .Property(e => e.Values)
    .Metadata
    .SetValueComparer(valueComparer);

            modelBuilder
                .Entity<Controller>()
                .Property(e => e.Values)
                .HasConversion(
                    v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries), valueComparer);



        }

    }
}
