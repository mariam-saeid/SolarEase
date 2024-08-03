namespace SolarEase.Models.DTO.FavoriteDto
{
    public class FavoriteProductDto
    {
        public int PersonId { get; set; }
        public int ProductId { get; set; }
        public DateTime AddedDate { get; set; }
        //profile info
        public string personName { get; set; }
        public string? ProfileImageUrl { get; set; }
        //poduct info
        public double Price { get; set; }
        public string ProductImageUrl { get; set; }
        public string Brand { get; set; }
        public double Capacity { get; set; }
        public string MeasuringUnit { get; set; }
        public double? Volt { get; set; }
        public string Capacity_Unit { get; set; }
        public double? TotalPrice { get; set; }
        public string PriceStr { get; set; }
        public string? TotalPriceStr { get; set; }
        public bool IsProductPost { get; set; }
        public string CategoryName { get; set; }

    }
}
