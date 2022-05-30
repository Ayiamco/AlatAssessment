using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlatAssessment.DTOs
{
    public class CustomerDTO
    {
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public int StateId { get; set; }

        public int LgaId { get; set; }

        public string State { get; set; }

        public string Lga { get; set; }

        public bool IsVerified { get; set; }

        public  DateTime CreatedAt { get; set; }
    }
}
