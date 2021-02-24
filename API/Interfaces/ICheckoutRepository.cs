using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ICheckoutRepository
    {
        Task<IEnumerable<Checkout>> GetCheckouts(bool status, string userName, bool includeSalespersonInfo = false);
        Task<Checkout> GetCheckoutWithId(int checkoutId);
        Checkout EditCheckout(Checkout checkout);
        Task<Checkout> CreateCheckout(Checkout checkout);
        Task<IEnumerable<Checkout>> GetSalespersonCheckouts(string userId);
    }
}
