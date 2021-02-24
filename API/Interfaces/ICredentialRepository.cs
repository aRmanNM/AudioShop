using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
    public interface ICredentialRepository
    {
        Task<SalespersonCredential> CreateCredential(SalespersonCredential salespersonCredential);
        SalespersonCredential UpdateCredetial(SalespersonCredential salespersonCredential);
        Task<SalespersonCredential> GetSalespersonCredetial(string userId, bool withTracking = false);
    }
}