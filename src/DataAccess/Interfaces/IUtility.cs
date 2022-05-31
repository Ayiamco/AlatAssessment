using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Entities;

namespace AlatAssessment.DataAccess.Interfaces
{
    public interface ICountryStateRepo :IGenericRepository< CountryState,int>
    {

    }

    public interface ILgaRepo : IGenericRepository<Lga, int>
    {

    }
}
