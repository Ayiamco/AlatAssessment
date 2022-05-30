using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AlatAssessment.Helpers;

namespace AlatAssessment.DTOs
{
    public class AddCustomerDTO
    {
        [StringLength(11)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        public int StateId { get; set; }

        [LGAValidation]
        public int LgaId{ get; set; }
    }
}
