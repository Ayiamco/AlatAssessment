using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.DTOs;

namespace AlatAssessment.Services.Interfaces
{
    public interface IWemaInternal
    {
        Task<List<BankDto>> GetBanks();
    }
}
