using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Contexts;
using AlatAssessment.DataAccess.Interfaces;
using AlatAssessment.DataAccess.Repositories;

namespace AlatAssessment.DataAccess.UnitOfWork
{
    public class AppUnitOfWork : IUnitOfWork
    {
        private readonly  AppDbContext _context;
        public AppUnitOfWork(AppDbContext context)
        {
            this._context = context;
        }

        public ICustomerRepository CustomerRepo => new CustomerRepository(_context);

        public ICountryStateRepo CountryStateRepo => new CountryStateRepository(_context);

        public  ILgaRepo LgaRepo => new LgaRepository(_context);

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
