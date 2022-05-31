using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DTOs;
using AlatAssessment.Helpers;
using AlatAssessment.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace AlatAssessment.Controllers
{
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ModelStateValidator))]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            this.logger = logger;
        }

        [HttpPost("Customer")]
        public async Task<IActionResult> AddCustomer(AddCustomerDTO model)
        {
            try
            {
                var resp = await _customerService.AddCustomer(model);
                if (resp.Code == ResponseCodes.Success)
                    return Ok(new APIResponse(resp.Code, resp.Description));

                if (resp.Code == ResponseCodes.ClientFailure)
                    return BadRequest(new APIResponse(resp.Code, resp.Description));

                return StatusCode(500, new APIResponse(resp.Code, resp.Description));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new APIResponse(ResponseCodes.ServerError, "Error: could not process request."));
            }
        }

        [HttpGet("Customer")]
        public async Task<IActionResult> GetAllCustomers(int? pageSize, int? page)
        {
            try
            {
                var resp = _customerService.GetAllCustomer(pageSize ?? 10, page ?? 1);
                if (resp == null)
                    return StatusCode(500, new APIResponse(ResponseCodes.ServerError, "Error: could not process request."));

                return Ok(new APIResponse<PaginationHelper.PagedList<CustomerDTO>>(ResponseCodes.Success, "", resp));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new APIResponse(ResponseCodes.ServerError, "Error: could not process request."));
            }
        }
    }
}
