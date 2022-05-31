using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Entities;
using AlatAssessment.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AlatAssessment.DataAccess.Contexts
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public AppDbContext() 
        {
        }

        public DbSet<CountryState> CountryStates { get; set; }

        public DbSet<Lga> Lgas { get; set; }

        public DbSet<Customer> Customers { get; set; }
    }
}
