using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Entities;
using AlatAssessment.DTOs;

namespace AlatAssessment.DataAccess.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer,Guid>
    {
        Task<List<CustomerDTO>> GetAllCustomers(int pageSize, int page);
    }
}
