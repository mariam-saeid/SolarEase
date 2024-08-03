using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.FavoritePostDto
{
    public class CreateFavoritePostDto
    {
        [Required]
        public int PersonId { get; set; }
        [Required]
        public int PostId { get; set; }
    }
}
