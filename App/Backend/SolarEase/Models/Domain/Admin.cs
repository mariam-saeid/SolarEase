using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarEase.Models.Domain
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string City { get; set; }

        [ForeignKey(nameof(Profile))]
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        [ForeignKey(nameof(Account))]
        public string AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
