using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.Domain
{
    public class Account : IdentityUser
    {
      public string Type {  get; set; }
      public string? ValidationCode {  get; set; }
      public DateTime? ValidationCodeExpiration {  get; set; }
       
    }
}
