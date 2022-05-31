using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DTOs;

namespace AlatAssessment.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<ServiceResp> AddCustomer(AddCustomerDTO addCustomerDto);

        Task<List<CustomerDTO>> GetAllCustomer(int pageSize, int page);
    }
}
