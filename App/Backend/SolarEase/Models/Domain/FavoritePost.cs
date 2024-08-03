using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarEase.Models.Domain
{
    public class FavoritePost
    {
        public DateTime AddedDate { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
