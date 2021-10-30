using System.Collections.Generic;
using System.Threading.Tasks;
using MailBookApi.Models;

namespace MailBookApi.Repos
{
    public interface IMailBookRepository
    {
        Task<IEnumerable<DeliverCompanyModel>> ListDeliverCompanyAsync();

        Task CreatePackageEntryAsync(CreatePackageModel modelEntry);

        Task<List<PackageModel>> ListPackageAsync();
    }
}