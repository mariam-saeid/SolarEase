using SolarEase.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SolarEase.Data
{
    public class SolarEaseAuthDbContext : IdentityDbContext<Account>
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<SolarInstaller> SolarInstallers { get; set; }
        public DbSet<SolarProduct> SolarProducts { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<FavoritePost> FavoritePosts { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public DbSet<PeakHour> PeakHours { get; set; }
        public DbSet<Calculator> Calculators { get; set; }
        public DbSet<DailyAllSkyPrediction> DailyAllSkyPredictions { get; set; }
        public DbSet<HourlyAllSkyPrediction> HourlyAllSkyPredictions { get; set; }
        public DbSet<ElectricityConsumption> ElectricityConsumptions { get; set; }
        public DbSet<ElectricityConsumptionSubrange> ElectricityConsumptionSubranges { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<FAQCategory> FAQCategories { get; set; }
        public DbSet<ChatbotMessage> ChatbotMessages { get; set; }
        public DbSet<Message> Messages { get; set; }
        public SolarEaseAuthDbContext(DbContextOptions<SolarEaseAuthDbContext> options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Adding Roles To Db
            var adminRoleId = "f954e17c-10b1-4b70-a554-752911f56e58";
            var UserRoleId = "287adf1f-3821-42f9-a5f2-4ce4eabe7b55";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id=adminRoleId,
                    ConcurrencyStamp=adminRoleId,
                    Name="Admin",
                    NormalizedName="Admin".ToUpper(),
                },
                new IdentityRole
                {
                    Id=UserRoleId,
                    ConcurrencyStamp=UserRoleId,
                    Name="Person",
                    NormalizedName="Person".ToUpper(),
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            //----------------------------------------------------------------------
            // FavoritePost Table
            builder.Entity<FavoritePost>()
                .HasKey(f => new { f.PersonId, f.PostId });

            builder.Entity<FavoritePost>()
                .HasOne(f => f.Person)
                .WithMany(p => p.FavoritePosts)
                .HasForeignKey(p => p.PersonId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FavoritePost>()
                .HasOne(f => f.Post)
                .WithMany(p => p.FavoritePosts)
                .HasForeignKey(f => f.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            //----------------------------------------------------------------------
            // FavoriteProduct Table
            builder.Entity<FavoriteProduct>()
                .HasKey(f => new { f.PersonId, f.ProductId });

            builder.Entity<FavoriteProduct>()
                .HasOne(f => f.Person)
                .WithMany(j => j.FavoriteProducts)
                .HasForeignKey(f => f.PersonId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FavoriteProduct>()
                .HasOne(f => f.Product)
                .WithMany(p => p.FavoriteProducts)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
