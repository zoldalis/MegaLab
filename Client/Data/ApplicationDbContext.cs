using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder
        //        .Entity<Controller>().HasNoKey()
        //        .Property(e => e.Values)
        //        .HasConversion(
        //            v => string.Join(',', v),
        //        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

        //}

    }
}
