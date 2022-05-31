using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Entities;
using AlatAssessment.DTOs;
using AlatAssessment.Helpers;

namespace AlatAssessment.DataAccess.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer,Guid>
    {
        PaginationHelper.PagedList<CustomerDTO> GetAllCustomers(int pageSize, int page);
    }
}
