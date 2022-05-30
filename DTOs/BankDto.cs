using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace AlatAssessment.DTOs
{
    public class BankDto
    {
        public string bankName { get; set; }
        public string bankCode { get; set; }
    }

    public class BankDtoFull : BankDto
    {
        public List<BankDto> result { get; set; }

        public string errorMessage { get; set; }

        public object errorMessages { get; set; }

        public bool hasError { get; set; }

        public DateTime timeGenerated {get; set; }
    }
}
