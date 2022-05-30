using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Interfaces;
using AlatAssessment.DataAccess.Repositories;

namespace AlatAssessment.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        public ICustomerRepository CustomerRepo { get;  }

        public ICountryStateRepo CountryStateRepo { get;  }

        public ILgaRepo LgaRepo { get; }

        public int Save();

        public Task<int> SaveAsync();
    }
}
