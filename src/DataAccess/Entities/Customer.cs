using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlatAssessment.DataAccess.Entities
{
    public class Customer
    {
        public  Guid Id { get; set; }

        public Lga Lga { get; set; }
        public int LgaId { get; set; }

        [StringLength(100)]
        public  string Email { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public bool IsVerified { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
