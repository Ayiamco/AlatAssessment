using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlatAssessment.Services.Interfaces
{
    public  interface INotificationService
    {
        bool SendOtp(string phoneNumber);

        bool VerifyOtp(string phoneNumber, string otp);
    }
}
