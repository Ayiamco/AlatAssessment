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
        [Required]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 8)]
        [Required]
        public string Password { get; set; }

        [Required]
        public int StateId { get; set; }

        [LgaValidation]
        [Required]
        public int LgaId{ get; set; }
    }
}
