using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class SMSService : ISMSService
    {
        private readonly SMSOptions _options;
        private readonly ILogger<SMSService> _logger;

        public SMSService(IOptions<SMSOptions> options, ILogger<SMSService> logger)
        {
            _logger = logger;
            _options = options.Value;
        }

        public string GenerateAuthToken()
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool SendSMS(string receptor, string authToken)
        {
            // try
            // {
            //     var sender = _options.SMSSender;
            //     var message = $"سلام\nکد عضویت در نرم افزار:\n{authToken}";
            //     var api = new KavenegarApi(_options.SMSAPIKey);
            //     api.Send(sender, receptor, message);
            // }
            // catch (Kavenegar.Exceptions.ApiException ex)
            // {
            //     _logger.LogError("warning", ex.Message);
            //     return false;
            // }
            // catch (Kavenegar.Exceptions.HttpException ex)
            // {
            //     _logger.LogError("warning", ex.Message);
            //     return false;
            // }

            _logger.LogInformation(authToken);

            return true;
        }
    }
}