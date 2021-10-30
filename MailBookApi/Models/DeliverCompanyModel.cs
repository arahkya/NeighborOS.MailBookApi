using System.ComponentModel.DataAnnotations;

namespace MailBookApi.Models
{
    public class DeliverCompanyModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string Name { get; set; }
    }
}