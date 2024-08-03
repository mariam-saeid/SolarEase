namespace SolarEase.Models.DTO.SolarProductDto
{
    public class SolarProductDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Capacity_Unit { get; set; }
        public double Capacity { get; set; }
        public string MeasuringUnit { get; set; }
        public double? Volt { get; set; }
        public double Price { get; set; }
        public double? TotalPrice { get; set; }
        public string PriceStr { get; set; }
        public string? TotalPriceStr { get; set; }
        public string ImageUrl { get; set; }
        public bool IsProductPost { get; set; }
       public string CategoryName { get; set; }
        public bool IsFavorite { get; set; }
    }
}
