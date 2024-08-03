using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.PostDto
{
    public class PostDto
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public string PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public int SolarProductId { get; set; }
        public double Price { get; set; }
        public string ProductImageUrl { get; set; }
        public string Brand { get; set; }
        public double Capacity { get; set; }
        public string MeasuringUnit { get; set; }
        public double? Volt { get; set; }
        public string CategoryName { get; set; }
        public bool IsUsed { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string CreatedOn { get; set; }
        public string? Description { get; set; }
        public string Capacity_Unit { get; set; }
        public string PriceStr { get; set; }
        public string? VoltStr { get; set; }
        public string CapacityStr { get; set; }
        public string CompositeLocation { get; set; }
    }
}
