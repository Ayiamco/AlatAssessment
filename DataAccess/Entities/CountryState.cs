using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlatAssessment.DataAccess.Entities
{
    public class CountryState
    {
        
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
