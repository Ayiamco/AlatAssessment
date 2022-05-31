using System;

using AlatAssessment.Services.Interfaces;

namespace AlatAssessment.Services
{
    public class PasswordManager : IPasswordManager
    {
        public string GetHash(string password)
        {
            if(string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

        public static PasswordManager CreateManager() => new();
    }
}
