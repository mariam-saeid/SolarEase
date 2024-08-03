using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.FavoriteProductDto
{
    public class CreateFavoriteProductDto
    {
        [Required]
        public int PersonId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
