using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace SolarEase.Models.Domain
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public double SystemSize { get; set; }

        [ForeignKey(nameof(Profile))]
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        [ForeignKey(nameof(Account))]
        public string AccountId { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<FavoritePost> FavoritePosts { get; set; }
        public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; }
        public virtual ICollection<ChatbotMessage> ChatbotMessages { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

    }
}
