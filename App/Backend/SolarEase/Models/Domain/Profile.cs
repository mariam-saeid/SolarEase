using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.Domain
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string Type { get; set; }
    }
}
