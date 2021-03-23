using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ICredentialRepository
    {
        Task<SalespersonCredential> CreateCredentialAsync(SalespersonCredential salespersonCredential);
        SalespersonCredential UpdateCredential(SalespersonCredential salespersonCredential);
        Task<SalespersonCredential> GetSalespersonCredentialAsync(string userId, bool withTracking = false);
    }
}