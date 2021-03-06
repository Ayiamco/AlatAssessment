using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Entities;
using AlatAssessment.DataAccess.UnitOfWork;
using AlatAssessment.DTOs;
using AlatAssessment.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace AlatAssessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWemaInternalHttpProxy _wemaInternalHttpProxy;
        private readonly ILogger<UtilityController> logger;

        public UtilityController(IUnitOfWork unitOfWork ,IWemaInternalHttpProxy wemaInternalHttpProxy,ILogger<UtilityController> logger)
        {
            this.unitOfWork = unitOfWork;
            this._wemaInternalHttpProxy = wemaInternalHttpProxy;
            this.logger = logger;
        }

        [HttpGet("lga")]
        public async Task<IActionResult> GetLga()
        {
            try
            {
                var data = await unitOfWork.LgaRepo.ReadAll();
                return StatusCode(200, new APIResponse<List<Lga>>(ResponseCodes.Success, "", data));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new APIResponse(ResponseCodes.ServerError, "Error: could not process request."));
            }
            
        }

        [HttpGet("states")]
        public async Task<IActionResult> GetStates()
        {
            try
            {
                var data = await unitOfWork.CountryStateRepo.ReadAll();
                return StatusCode(200, new APIResponse<List<CountryState>>(ResponseCodes.Success, "",
                    data.OrderBy(x => x.Name).ToList()));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new APIResponse(ResponseCodes.ServerError, "Error: could not process request."));
            }
        }

        [HttpGet("banks")]
        public async Task<IActionResult> GetBank()
        {
            try
            {
                var data = await _wemaInternalHttpProxy.GetBanks();
                if (data == null)
                    return StatusCode(500, new APIResponse(ResponseCodes.ServerError, "Error: could not process request."));

                return Ok(new APIResponse<List<BankDto>>(ResponseCodes.Success, "", data));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, new APIResponse(ResponseCodes.ServerError, "Error: could not process request."));
            }

        }
    }
}
