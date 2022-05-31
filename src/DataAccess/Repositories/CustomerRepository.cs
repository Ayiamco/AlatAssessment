using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Contexts;
using AlatAssessment.DataAccess.Entities;
using AlatAssessment.DataAccess.Interfaces;
using AlatAssessment.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AlatAssessment.DataAccess.Repositories
{
    public class CustomerRepository : GenericRepository<Customer,Guid> , ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {
             
        }

        public async Task<List<CustomerDTO>> GetAllCustomers(int pageSize, int page)
        {
            return await (from customers in context.Customers.AsNoTracking()
                              join lga in context.Lgas.AsNoTracking()
                                  on customers.LgaId equals  lga.Id
                              join states in context.CountryStates.AsNoTracking()
                                  on lga.StateId equals states.Id
                              select new CustomerDTO
                              {
                                  CreatedAt = customers.CreatedAt,
                                  Email = customers.Email,
                                  Lga = lga.Name,
                                  State = states.Name,
                                  LgaId = lga.Id,
                                  StateId = states.Id,
                                  IsVerified = customers.IsVerified,
                                  PhoneNumber = customers.PhoneNumber
                              }).ToListAsync();
        }
    }
}
