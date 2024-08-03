using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SolarEase.Models.Domain
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public bool IsUsed { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }

        [ForeignKey(nameof(Person))]
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }

        [ForeignKey(nameof(SolarProduct))]
        public int SolarProductId { get; set; }
        public virtual SolarProduct SolarProduct { get; set; }
        public virtual ICollection<FavoritePost> FavoritePosts { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

    }
}
