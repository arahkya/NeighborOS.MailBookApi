using System.ComponentModel.DataAnnotations;

namespace MailBookApi.Data.Entities
{
    public class DeliverCompanyEntity
    {
        [Key]
        public int Id { get; internal set; }

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string Name { get; internal set; }
    }
}