using SolarEase.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.Domain
{
    public class FavoriteProduct
    {
        public DateTime AddedDate { get; set; }
        public int PersonId { get; set; }
        public int ProductId { get; set; }
        public virtual Person Person { get; set; }
        public virtual SolarProduct Product { get; set; }
    }
}
