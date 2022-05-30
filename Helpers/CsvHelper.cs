using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Contexts;
using AlatAssessment.DataAccess.Entities;
using AlatAssessment.DTOs;
using CsvHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AlatAssessment.Helpers
{
    public class CsvHelper
    {
        private class StateComparer : IEqualityComparer<CountryState>
        {
            public bool Equals(CountryState x, CountryState y)
            {
                return x?.Name == y?.Name;
            }

            public int GetHashCode([DisallowNull] CountryState obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        private static List<LGAData> GetLgaData()
        {
            using var reader = new StreamReader("LGA.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<LGAData>().ToList();
            return records;
        }

        private static async  Task SeedCountryState(AppDbContext context )
        {
            var data = GetLgaData();
            var countryStates= data.Select(x => new CountryState{Name = x.State})
                .ToList().Distinct(new StateComparer());
            await  context.CountryStates.AddRangeAsync(countryStates);
            await context.SaveChangesAsync();
        }

        private static async Task SeedLga(AppDbContext context)
        {
            var data = GetLgaData();
            var states=context.CountryStates.ToList();

            var lgas = data.Select(x => new Lga
            {
                Name = x.Lga, 
                StateId = states.First(y => y.Name == x.State).Id
            });
            await context.Lgas.AddRangeAsync(lgas);
            await context.SaveChangesAsync();
        }

        public static async Task EnsureSeedDataPopulated(IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices.CreateScope().ServiceProvider;
            await using (var context = (AppDbContext)serviceProvider.GetService(typeof(AppDbContext)))
            {
                if (context == null)
                    throw new Exception("Could not find AppDbContext in injection container");

                if (context.Lgas.Any())
                    return;
                
                if (!context.CountryStates.Any())
                    await SeedCountryState(context);

                if(!context.Lgas.Any())
                   await  SeedLga(context);

            }
        }
    }
}
