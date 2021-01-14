using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using Kavenegar;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class SMSService : ISMSService
    {
        private readonly SMSOptions _options;
        private readonly ILogger<SMSService> _logger;
        private readonly StoreContext _storeContext;

        public SMSService(IOptions<SMSOptions> options, ILogger<SMSService> logger, StoreContext storeContext)
        {
            _storeContext = storeContext;
            _logger = logger;
            _options = options.Value;
        }

        public async Task<User> FindUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _storeContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public string GenerateAuthToken()
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool SendSMS(string receptor, string authToken)
        {
            try
            {
                var sender = _options.SMSSender;
                var message = $"سلام\nکد عضویت در نرم افزار:\n{authToken}";
                var api = new KavenegarApi(_options.SMSAPIKey);
                api.Send(sender, receptor, message);
            }
            catch (Kavenegar.Exceptions.ApiException ex)
            {
                _logger.LogError("warning", ex.Message);
                return false;
            }
            catch (Kavenegar.Exceptions.HttpException ex)
            {
                _logger.LogError("warning", ex.Message);
                return false;
            }

            _logger.LogInformation(authToken);

            return true;
        }
    }
}