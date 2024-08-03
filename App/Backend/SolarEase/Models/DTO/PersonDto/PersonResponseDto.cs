namespace SolarEase.Models.DTO.PersonDto
{
    public class PersonResponseDto
    {
        public string JwtToken { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public double SystemSize { get; set; }
        public string AccountId { get; set; }
        public string Email { get; set; }
        public int ProfileId { get; set; }
        public string? ImageUrl { get; set; }
    }
}
