using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MailBookApi.Data;
using MailBookApi.Data.Entities;
using MailBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MailBookApi.Repos
{
    public class MailBookRepository : IMailBookRepository
    {
        private readonly MailBookDbContext _dbContext;

        public IMapper _mapper { get; }

        public MailBookRepository(MailBookDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DeliverCompanyModel>> ListDeliverCompanyAsync()
        {
            var entities = await _dbContext.DeliverCompanies.ToListAsync().ConfigureAwait(false);
            var models = _mapper.Map<IEnumerable<DeliverCompanyModel>>(entities);

            return models;
        }

        public async Task CreatePackageEntryAsync(CreatePackageModel modelEntry)
        {
            var entity = _mapper.Map<CreatePackageModel, PackageEntity>(modelEntry);
            
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<PackageModel>> ListPackageAsync()
        {
            var packageEntityList = await _dbContext.Packages.Include(p => p.DeliverCompany).ToListAsync();
            var packageModelList = _mapper.Map<List<PackageEntity>, List<PackageModel>>(packageEntityList);

            return packageModelList;
        }
    }
}