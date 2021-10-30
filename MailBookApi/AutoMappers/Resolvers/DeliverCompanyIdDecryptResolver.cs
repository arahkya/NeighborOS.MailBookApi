using MailBookApi.Configure;

namespace MailBookApi.AutoMappers.Resolves
{
    public class DeliverCompanyIdDecryptResolver : DecryptResolver
    {
        protected override string propertyName => "DeliverCompanyId";



        public DeliverCompanyIdDecryptResolver(SecurityKeyConfigureOption securityKey) : base(securityKey)
        {
            
        }
    }
}