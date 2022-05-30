using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlatAssessment.Helpers
{
    public class AppSettings
    {
        public static  string ConnectionString { get; set; }

        public static  string SubscriptionKey { get; set; }

        public static string WemaInternalUrl { get; set; }
    }
}
