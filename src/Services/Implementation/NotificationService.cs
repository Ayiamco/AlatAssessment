using AlatAssessment.Services.Interfaces;

namespace AlatAssessment.Services.Implementation
{
    public class NotificationService :INotificationService
    {
        public bool SendOtp(string phoneNumber)
        {
            return true;
        }

        public bool VerifyOtp(string phoneNumber, string otp)
        {
            return otp == "12345";
        }
    }
}
