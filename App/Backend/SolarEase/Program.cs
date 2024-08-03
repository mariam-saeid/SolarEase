using SolarEase.Data;
using SolarEase.Models.Domain;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using SolarEase.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SolarEase.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<SolarEaseAuthDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("SolarEaseConnectionString")
    ));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "SolarEase API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                },
                Scheme="Oauth2",
                Name=JwtBearerDefaults.AuthenticationScheme,
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ISolarInstallerRepository, SolarInstallerRepository>();
builder.Services.AddScoped<ISolarProductRepository, SolarProductRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IFavoritePostRepository, FavoritePostRepository>();
builder.Services.AddScoped<IFavoriteProductRepository, FavoriteProductRepository>();
builder.Services.AddScoped<IPeakHourRepository, PeakHourRepository>();
builder.Services.AddScoped<ICalculatorRepository, CalculatorRepository>();
builder.Services.AddScoped<IDailyAllSkyPredictionRepository, DailyAllSkyPredictionRepository>();
builder.Services.AddScoped<IHourlyAllSkyPredictionRepository, HourlyAllSkyPredictionRepository>();
builder.Services.AddScoped<IElectricityConsumptionRepository, ElectricityConsumptionRepository>();
builder.Services.AddScoped<IFAQRepository, FAQRepository>();
builder.Services.AddScoped<IFAQCategoryRepository, FAQCategoryRepository>();
builder.Services.AddScoped<IChatbotMessageRepository, ChatbotMessageRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<CalculatorService>();
builder.Services.AddScoped<GoogleMapsApiService>();
builder.Services.AddScoped<EmailService>();

// Register the hosted service

builder.Services.AddHostedService<PredictionTaskService>();
builder.Services.AddHttpClient(); // Add HttpClientFactory
builder.Services.AddScoped<HourlyEnergyPredictionController>(); // Register your controller
builder.Services.AddScoped<DailyEnergyPredictionController>(); // Register your controller




builder.Services.AddAutoMapper(typeof(Program)); //automapper config


//set up (inject) identity inside our solution
builder.Services.AddIdentityCore<Account>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<Account>>("SolarEase")
    .AddEntityFrameworkStores<SolarEaseAuthDbContext>().
    AddDefaultTokenProviders();

//options of the password we want to configure
builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});

//add authentication to our sevices
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // This line is crucial to serve static files

app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
