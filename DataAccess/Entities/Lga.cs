using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlatAssessment.DataAccess.Entities
{
    public class Lga
    {
        public int Id { get; set; }

        public int StateId { get; set; }

        public string Name { get; set; }

        public CountryState State { get; set; }
    }
}
