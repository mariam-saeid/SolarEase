using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.PostDto
{
    public class SmallPostDto
    {
        public int Id { get; set; }
        public string PersonName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string ProductImageUrl { get; set; }
        public string CompositeName { get; set; }
        public string PriceStr { get; set; }
        public string City { get; set; }
        public string CreatedOn { get; set; }
        public bool IsFavorite { get; set; }

    }
}
