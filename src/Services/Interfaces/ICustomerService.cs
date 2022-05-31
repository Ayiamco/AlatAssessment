using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DTOs;
using AlatAssessment.Helpers;

namespace AlatAssessment.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<ServiceResp> AddCustomer(AddCustomerDTO addCustomerDto);

        PaginationHelper.PagedList<CustomerDTO> GetAllCustomer(int pageSize, int page);
    }
}
