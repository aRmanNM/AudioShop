using System;
using System.Linq;
using API.Interfaces;
using Microsoft.Extensions.Logging;

namespace API.Services
{
    public class FakeSMSService : ISMSService
    {
        private readonly ILogger<FakeSMSService> _logger;

        public FakeSMSService(ILogger<FakeSMSService> logger)
        {
            _logger = logger;
        }

        public string GenerateAuthToken()
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool SendVerificationSMS(string receptor, string authToken)
        {
            _logger.LogInformation(authToken);
            return true;
        }

        public bool SendMessageSMS(string receptor, string message)
        {
            _logger.LogInformation(message);
            return true;
        }
    }
}