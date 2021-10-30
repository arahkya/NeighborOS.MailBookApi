using System;
using System.ComponentModel.DataAnnotations;

namespace MailBookApi.Data.Entities
{
    public class PackageEntity
    {
        [Key]
        public int Id { get; internal set; }

        [Required]
        [MaxLength(20)]
        [MinLength(6)]
        public string PackageNumber { get; internal set; }

        public DateTime ArrivedDate { get; internal set; }

        public int DeliverCompanyId { get; internal set; }

        public DeliverCompanyEntity DeliverCompany { get; internal set; }
    }
}