using AutoMapper;
using MailBookApi.Data.Entities;
using MailBookApi.AutoMappers.Resolves;
using MailBookApi.Models;

namespace MailBookApi.AutoMappers
{
    public class DefaultMappingProfile : Profile
    {
        public DefaultMappingProfile()
        {

            CreateMap<DeliverCompanyEntity, DeliverCompanyModel>()
                .ForMember(p => p.Id, memberCfg =>
                    memberCfg.MapFrom<IdEncryptResolver>()
                );
            CreateMap<PackageEntity, PackageModel>()
                .ForMember(p => p.DeliverCompany, config =>
                    config.MapFrom(k => k.DeliverCompany.Name)
                ).
                ForMember(p => p.Id, memberCfg =>
                    memberCfg.MapFrom<IdEncryptResolver>()
                );
            CreateMap<CreatePackageModel, PackageEntity>()
                .ForMember(p => p.DeliverCompanyId, config =>
                    config.MapFrom<DeliverCompanyIdDecryptResolver>()
                )
                .ForMember(p => p.DeliverCompany, config => config.Ignore());
        }
    }
}