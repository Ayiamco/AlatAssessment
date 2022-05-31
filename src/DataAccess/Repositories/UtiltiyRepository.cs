using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Contexts;
using AlatAssessment.DataAccess.Entities;
using AlatAssessment.DataAccess.Interfaces;

namespace AlatAssessment.DataAccess.Repositories
{
    public class CountryStateRepository: GenericRepository<CountryState,int>, ICountryStateRepo
    {
        public CountryStateRepository(AppDbContext context):base(context)
        {
                
        }
    }


    public class LgaRepository : GenericRepository<Lga, int>, ILgaRepo
    {
        public LgaRepository(AppDbContext context) : base(context)
        {

        }
    }
}
