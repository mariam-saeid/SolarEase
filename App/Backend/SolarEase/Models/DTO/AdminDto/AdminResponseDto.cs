namespace SolarEase.Models.DTO.AdminDto
{
    public class AdminResponseDto
    {
        public string JwtToken { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string AccountId { get; set; }
        public string Email { get; set; }
        public int ProfileId { get; set; }
        public string? ImageUrl { get; set; }
    }
}
