using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.SolarInstallerDto
{
    public class SolarInstallerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public double DistanceFromUser { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}
