using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ICheckoutRepository
    {
        Task<IEnumerable<Checkout>> GetCheckoutsAsync(bool status, string userName, bool includeSalespersonInfo = false);
        Task<Checkout> GetCheckoutWithIdAsync(int checkoutId);
        Checkout EditCheckout(Checkout checkout);
        Task<Checkout> CreateCheckoutAsync(Checkout checkout);
        Task<IEnumerable<Checkout>> GetSalespersonCheckoutsAsync(string userId);
    }
}
