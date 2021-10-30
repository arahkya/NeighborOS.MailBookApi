using System;
using System.ComponentModel.DataAnnotations;

namespace MailBookApi.Models
{
    public class CreatePackageModel
    {        
        [Required]
        [MaxLength(20)]
        [MinLength(6)]
        public string PackageNumber { get; set; }

        public DateTime ArrivedDate { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string DeliverCompanyId { get; set; }

    }
}