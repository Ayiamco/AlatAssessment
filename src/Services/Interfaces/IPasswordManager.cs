using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlatAssessment.Services.Interfaces
{
    public interface IPasswordManager
    {
        string GetHash(string password);
    }
}
