using MailBookApi.Configure;

namespace MailBookApi.AutoMappers.Resolves
{
    public class IdEncryptResolver : EncryptResolver
    {
        protected override string propertyName => "Id";

        public IdEncryptResolver(SecurityKeyConfigureOption securityKey) : base(securityKey)
        {
            
        }
    }
}