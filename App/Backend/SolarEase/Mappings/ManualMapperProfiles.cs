using SolarEase.Models.Domain;
using SolarEase.Models.DTO.AccountDto;
using SolarEase.Models.DTO.AdminDto;
using SolarEase.Models.DTO.PersonDto;
using SolarEase.Models.DTO.ProfileDto;
using SolarEase.Models.DTO.PostDto;
using System.IO;
using System;
using SolarEase.Models.DTO.FavoritePostDto;
using SolarEase.Models.DTO.FavoriteDto;
using SolarEase.Models.DTO.SolarProductDto;
using SolarEase.Models.DTO.PeakHourDto;
using Microsoft.Extensions.Hosting;
using SolarEase.Models.DTO.CalculatorDto;
using SolarEase.Models.DTO.EnergyPredictionDto;
using SolarEase.Models.DTO.FAQDto;
using SolarEase.Models.DTO.MessageDto;
using SolarEase.Models.DTO.ChatbotMessageDto;

namespace SolarEase.Mappings
{
    public static class ManualMapperProfiles
    {
        public static Person PersonRegister(this RegisterPersonDto registerPersonDto)
        {
            return new Person
            {
                Name = registerPersonDto.Name,
                Location = registerPersonDto.Location,
                City = registerPersonDto.City,
                PhoneNumber = registerPersonDto.PhoneNumber
            };
        }

        public static List<PersonDto> PersonsInfo(this List<Person> persons)
        {
            var PersonList = new List<PersonDto>();
            foreach (var person in persons)
            {
                PersonList.Add(
                    new PersonDto
                    {
                        Id = person.Id,
                        Name = person.Name,
                        Location = person.Location,
                        City = person.City,
                        PhoneNumber = person.PhoneNumber,
                        SystemSize = person.SystemSize,
                        AccountId = person.Account.Id,
                        Email = person.Account.Email,
                        ProfileId = person.Profile.Id,
                        ImageUrl = person?.Profile?.ImageUrl != null
                        ? person.Profile.ImageUrl.Substring(person.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/')
                        : "ProfileImages/profile.png"
                    }
                    );
            }
            return PersonList;
        }

        public static PersonDto PersonInfo(this Person person)
        {
            return new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                Location = person.Location,
                City = person.City,
                PhoneNumber = person.PhoneNumber,
                SystemSize = person.SystemSize,
                AccountId = person.Account.Id,
                Email = person.Account.Email,
                ProfileId = person.Profile.Id,
                ImageUrl = person?.Profile?.ImageUrl != null
                        ? person.Profile.ImageUrl.Substring(person.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/')
                        : "ProfileImages/profile.png"
            };
        }

        public static PersonResponseDto PersonResponseInfo(this Person person)
        {
            return new PersonResponseDto
            {
                Id = person.Id,
                Name = person.Name,
                Location = person.Location,
                City = person.City,
                PhoneNumber = person.PhoneNumber,
                SystemSize = person.SystemSize,
                AccountId = person.Account.Id,
                Email = person.Account.Email,
                ProfileId = person.Profile.Id,
                ImageUrl = person?.Profile?.ImageUrl != null
                        ? person.Profile.ImageUrl.Substring(person.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/')
                        : "ProfileImages/profile.png"
            };
        }

        public static Person PersonUpdateInfo(this UpdatePersonInfoDto updatePersonInfoDto)
        {
            return new Person
            {
                Name = updatePersonInfoDto.Name,
                Location = updatePersonInfoDto.Location,
                City = updatePersonInfoDto.City,
                PhoneNumber = updatePersonInfoDto.PhoneNumber,
                SystemSize=updatePersonInfoDto.SystemSize
            };
        }

        //**************************************************************************************************
        public static Admin AdminCreate(this CreateAdminDto createAdminDto)
        {
            return new Admin
            {
                Name = createAdminDto.Name,
                PhoneNumber = createAdminDto.PhoneNumber,
                Location = createAdminDto.Location,
                City= createAdminDto.City,
            };
        }

        public static List<AdminDto> AdminsInfo(this List<Admin> admins)
        {
            var AdminList = new List<AdminDto>();
            foreach (var admin in admins)
            {
                AdminList.Add(
                    new AdminDto
                    {
                        Id = admin.Id,
                        Name = admin.Name,
                        PhoneNumber = admin.PhoneNumber,
                        Location = admin.Location,
                        City = admin.City,
                        AccountId = admin.Account.Id,
                        Email = admin.Account.Email,
                        ProfileId = admin.Profile.Id,
                        ImageUrl = admin?.Profile?.ImageUrl != null
                        ? admin.Profile.ImageUrl.Substring(admin.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/') 
                        : "ProfileImages/profile.png"
                    }
                    );
            }
            return AdminList;
        }
       
        public static AdminDto AdminInfo(this Admin admin)
        {
            return new AdminDto
            {
                Id = admin.Id,
                Name = admin.Name,
                PhoneNumber = admin.PhoneNumber,
                Location = admin.Location,
                City = admin.City,
                AccountId = admin.Account.Id,
                Email = admin.Account.Email,
                ProfileId = admin.Profile.Id,
                ImageUrl = admin?.Profile?.ImageUrl != null
                        ? admin.Profile.ImageUrl.Substring(admin.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/')
                        : "ProfileImages/profile.png"
            };
        }

        public static AdminResponseDto AdminResponseInfo(this Admin admin)
        {
            return new AdminResponseDto
            {
                Id = admin.Id,
                Name = admin.Name,
                PhoneNumber = admin.PhoneNumber,
                Location = admin.Location,
                City = admin.City,
                AccountId = admin.Account.Id,
                Email = admin.Account.Email,
                ProfileId = admin.Profile.Id,
                ImageUrl = admin?.Profile?.ImageUrl != null
                        ? admin.Profile.ImageUrl.Substring(admin.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/')
                        : "ProfileImages/profile.png"
            };
        }
        
        public static Admin AdminUpdateInfo(this UpdateAdminInfoDto updateAdminInfo2Dto)
        {
            return new Admin
            {
                Name = updateAdminInfo2Dto.Name,
                Location = updateAdminInfo2Dto.Location,
                City = updateAdminInfo2Dto.City,
                PhoneNumber = updateAdminInfo2Dto.PhoneNumber,
            };
        }

        //**************************************************************************************************
        public static Post CreatePost(this CreatePostDto createPostDto)
        {
            return new Post
            {
                IsUsed = createPostDto.IsUsed,
                Location = createPostDto.Location,
                City = createPostDto.City,
                CreatedOn = DateTime.Now,
                Description = createPostDto.Description,
                Active = false,
            };
        }

        public static Post UpdatePost(this UpdatePostDto updatePostDto)
        {
            return new Post
            {
                IsUsed = updatePostDto.IsUsed,
                Location = updatePostDto.Location,
                City = updatePostDto.City,
                Description = updatePostDto.Description,
            };
        }

        public static List<SmallPostDto> SmallPostDtoInfo(this List<Post> posts)
        {
            var PostList = new List<SmallPostDto>();
            foreach (var post in posts)
            {
                string categoryName;
                if (post.SolarProduct.ProductCategory.Name == "Solar Panel")
                {
                    categoryName = "Panel";
                }
                else
                {
                    categoryName = post.SolarProduct.ProductCategory.Name;
                }
                string capacity_unit;
                if (post.SolarProduct.ProductCategory.Name == "Battery")
                {
                    capacity_unit = post.SolarProduct.Volt + "V-" +
                        post.SolarProduct.Capacity.ToString() + post.SolarProduct.MeasuringUnit;
                }
                else
                {
                    capacity_unit = post.SolarProduct.Capacity.ToString() + post.SolarProduct.MeasuringUnit;
                }
                PostList.Add(
                    new SmallPostDto
                    {
                        Id = post.Id,
                        PersonName = post.Person.Name,
                        ProfileImageUrl = post.Person?.Profile?.ImageUrl != null
                        ? post.Person.Profile.ImageUrl.Substring(post.Person.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/')
                        : "ProfileImages/profile.png",
                        ProductImageUrl = post.SolarProduct.ImageUrl.Substring(post.SolarProduct.ImageUrl.IndexOf("SolarProductImages")).Replace('\\', '/'),
                        CompositeName = post.SolarProduct.Brand + " " + categoryName + " " + capacity_unit,
                        PriceStr =Math.Round(post.SolarProduct.Price, 2).ToString()+ " EGP",
                        City = post.City,
                        CreatedOn = post.CreatedOn.ToString("dd/MM/yyyy")
                    }
                    );
            }
            return PostList;
        }

        public static PostDto PostInfo(this Post post)
        {
            string capacity_unit;
            string? voltstr;
            if (post.SolarProduct.ProductCategory.Name == "Battery")
            {
                capacity_unit = post.SolarProduct.Volt + "V-" +
                    post.SolarProduct.Capacity.ToString() + post.SolarProduct.MeasuringUnit;

                voltstr = post.SolarProduct.Volt + "V";
            }
            else
            {
                capacity_unit = post.SolarProduct.Capacity.ToString() + post.SolarProduct.MeasuringUnit;
                voltstr = null;
            }
            return new PostDto
            {
                Id = post.Id,
                Active = post.Active,
                PersonId = post.PersonId,
                PersonName = post.Person.Name,
                PhoneNumber = post.Person.PhoneNumber,
                ProfileImageUrl = post.Person?.Profile?.ImageUrl != null
                        ? post.Person.Profile.ImageUrl.Substring(post.Person.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/')
                        : "ProfileImages/profile.png",
                SolarProductId = post.SolarProductId,
                Price = post.SolarProduct.Price,
                ProductImageUrl = post.SolarProduct.ImageUrl.Substring(post.SolarProduct.ImageUrl.IndexOf("SolarProductImages")).Replace('\\', '/'),
                Brand = post.SolarProduct.Brand,
                Capacity = post.SolarProduct.Capacity,
                MeasuringUnit = post.SolarProduct.MeasuringUnit,
                Volt = post.SolarProduct.Volt,
                CategoryName = post.SolarProduct.ProductCategory.Name,
                IsUsed = post.IsUsed,
                Location = post.Location,
                City = post.City,
                CreatedOn = post.CreatedOn.ToString("dd/MM/yyyy"),
                Description = post.Description,
                Capacity_Unit = capacity_unit,
                PriceStr = Math.Round(post.SolarProduct.Price, 2).ToString() + " EGP",
                CapacityStr = post.SolarProduct.Capacity.ToString() + post.SolarProduct.MeasuringUnit,
                VoltStr = voltstr,
                CompositeLocation = post.Location+" "+post.City
            };
        }
        
        public static List<PostDto> PostsInfo(this List<Post> posts)
        {
            var PostList = new List<PostDto>();
            foreach (var post in posts)
            {
                string capacity_unit;
                string? voltstr;
                if (post.SolarProduct.ProductCategory.Name == "Battery")
                {
                    capacity_unit = post.SolarProduct.Volt + "V-" +
                        post.SolarProduct.Capacity.ToString() + post.SolarProduct.MeasuringUnit;

                    voltstr = post.SolarProduct.Volt + "V";
                }
                else
                {
                    capacity_unit = post.SolarProduct.Capacity.ToString() + post.SolarProduct.MeasuringUnit;
                    voltstr = null;
                }
                PostList.Add(
                    new PostDto
                    {
                        Id = post.Id,
                        Active = post.Active,
                        PersonId = post.PersonId,
                        PersonName = post.Person.Name,
                        PhoneNumber = post.Person.PhoneNumber,
                        ProfileImageUrl = post.Person?.Profile?.ImageUrl != null
                        ? post.Person.Profile.ImageUrl.Substring(post.Person.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/')
                        : "ProfileImages/profile.png",
                        SolarProductId = post.SolarProductId,
                        Price = post.SolarProduct.Price,
                        ProductImageUrl = post.SolarProduct.ImageUrl.Substring(post.SolarProduct.ImageUrl.IndexOf("SolarProductImages")).Replace('\\', '/'),
                        Brand = post.SolarProduct.Brand,
                        Capacity = post.SolarProduct.Capacity,
                        MeasuringUnit = post.SolarProduct.MeasuringUnit,
                        Volt = post.SolarProduct.Volt,
                        CategoryName = post.SolarProduct.ProductCategory.Name,
                        IsUsed = post.IsUsed,
                        Location = post.Location,
                        City = post.City,
                        CreatedOn = post.CreatedOn.ToString("dd/MM/yyyy"),
                        Description = post.Description,
                        Capacity_Unit = capacity_unit,
                        PriceStr = Math.Round(post.SolarProduct.Price, 2).ToString() + " EGP",
                        CapacityStr = post.SolarProduct.Capacity.ToString() + post.SolarProduct.MeasuringUnit,
                        VoltStr = voltstr,
                        CompositeLocation = post.Location + " " + post.City
                    }
                    );
            }
            return PostList;
        }

        //**************************************************************************************************
        public static List<FavoritePostDto> FavoritePostsInfo(this List<FavoritePost> favoritePosts)
        {
            var PostList = new List<FavoritePostDto>();
            foreach (var favoritePost in favoritePosts)
            {
                string capacity_unit;
                string? voltstr;
                if (favoritePost.Post.SolarProduct.ProductCategory.Name == "Battery")
                {
                    capacity_unit = favoritePost.Post.SolarProduct.Volt + "V-" +
                        favoritePost.Post.SolarProduct.Capacity.ToString() + favoritePost.Post.SolarProduct.MeasuringUnit;
                    
                    voltstr = favoritePost.Post.SolarProduct.Volt + "V";
                }
                else
                {
                    capacity_unit = favoritePost.Post.SolarProduct.Capacity.ToString() + favoritePost.Post.SolarProduct.MeasuringUnit;
                    voltstr = null;
                }
                PostList.Add(
                    new FavoritePostDto
                    {
                        PersonId = favoritePost.PersonId,
                        PostId = favoritePost.PostId,
                        AddedDate = favoritePost.AddedDate,
                        Active = favoritePost.Post.Active,
                        PersonName = favoritePost.Person.Name,
                        PhoneNumber = favoritePost.Person.PhoneNumber,
                        ProfileImageUrl = favoritePost.Person?.Profile?.ImageUrl != null
                        ? favoritePost.Person.Profile.ImageUrl.Substring(favoritePost.Person.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/')
                        : "ProfileImages/profile.png",
                        SolarProductId = favoritePost.Post.SolarProductId,
                        Price = favoritePost.Post.SolarProduct.Price,
                        ProductImageUrl = favoritePost.Post.SolarProduct.ImageUrl.Substring(favoritePost.Post.SolarProduct.ImageUrl.IndexOf("SolarProductImages")).Replace('\\', '/'),
                        Brand = favoritePost.Post.SolarProduct.Brand,
                        Capacity = favoritePost.Post.SolarProduct.Capacity,
                        MeasuringUnit = favoritePost.Post.SolarProduct.MeasuringUnit,
                        CategoryName = favoritePost.Post.SolarProduct.ProductCategory.Name,
                        IsUsed = favoritePost.Post.IsUsed,
                        Location = favoritePost.Post.Location,
                        City = favoritePost.Post.City,
                        CreatedOn = favoritePost.Post.CreatedOn.ToString("dd/MM/yyyy"),
                        Description = favoritePost.Post.Description,
                        Capacity_Unit = capacity_unit,
                        PriceStr = Math.Round(favoritePost.Post.SolarProduct.Price, 2).ToString() + " EGP",
                        CapacityStr = favoritePost.Post.SolarProduct.Capacity.ToString() + favoritePost.Post.SolarProduct.MeasuringUnit,
                        VoltStr = voltstr,
                        CompositeLocation = favoritePost.Post.Location + " " + favoritePost.Post.City
                    }
                    );
            }
            return PostList;
        }

        public static FavoritePostDto FavoritePostInfo(this FavoritePost favoritePost)
        {
            string capacity_unit;
            string? voltstr;
            if (favoritePost.Post.SolarProduct.ProductCategory.Name == "Battery")
            {
                capacity_unit = favoritePost.Post.SolarProduct.Volt + "V-" +
                    favoritePost.Post.SolarProduct.Capacity.ToString() + favoritePost.Post.SolarProduct.MeasuringUnit;

                voltstr = favoritePost.Post.SolarProduct.Volt + "V";
            }
            else
            {
                capacity_unit = favoritePost.Post.SolarProduct.Capacity.ToString() + favoritePost.Post.SolarProduct.MeasuringUnit;
                voltstr = null;
            }
            return new FavoritePostDto
            {
                PersonId = favoritePost.PersonId,
                PostId = favoritePost.PostId,
                AddedDate = favoritePost.AddedDate,
                Active = favoritePost.Post.Active,
                PersonName = favoritePost.Person.Name,
                ProfileImageUrl = favoritePost.Person?.Profile?.ImageUrl != null
                        ? favoritePost.Person.Profile.ImageUrl.Substring(favoritePost.Person.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/')
                        : "ProfileImages/profile.png",
                SolarProductId = favoritePost.Post.SolarProductId,
                Price = favoritePost.Post.SolarProduct.Price,
                ProductImageUrl = favoritePost.Post.SolarProduct.ImageUrl.Substring(favoritePost.Post.SolarProduct.ImageUrl.IndexOf("SolarProductImages")).Replace('\\', '/'),
                Brand = favoritePost.Post.SolarProduct.Brand,
                Capacity = favoritePost.Post.SolarProduct.Capacity,
                MeasuringUnit = favoritePost.Post.SolarProduct.MeasuringUnit,
                CategoryName = favoritePost.Post.SolarProduct.ProductCategory.Name,
                IsUsed = favoritePost.Post.IsUsed,
                Location = favoritePost.Post.Location,
                City = favoritePost.Post.City,
                CreatedOn = favoritePost.Post.CreatedOn.ToString("dd/MM/yyyy"),
                Description = favoritePost.Post.Description,
                Capacity_Unit = capacity_unit,
                PriceStr = Math.Round(favoritePost.Post.SolarProduct.Price, 2).ToString() + " EGP",
                CapacityStr = favoritePost.Post.SolarProduct.Capacity.ToString() + favoritePost.Post.SolarProduct.MeasuringUnit,
                VoltStr = voltstr,
                CompositeLocation = favoritePost.Post.Location + " " + favoritePost.Post.City
            };
        }

        //**************************************************************************************************
        public static SolarProductDto ProductInfo(this SolarProduct solarProduct)
        {
            string capacity_unit;
            if (solarProduct.ProductCategory.Name == "Battery")
            {
                capacity_unit = solarProduct.Volt + "V-" +
                    solarProduct.Capacity.ToString() + solarProduct.MeasuringUnit;
            }
            else
            {
                capacity_unit = solarProduct.Capacity.ToString() + solarProduct.MeasuringUnit;
            }
            double? total_price;
            string? total_price_str;
            string unit = "";
            if (solarProduct.ProductCategory.Name == "Solar Panel")
            {
                total_price = Math.Round(solarProduct.Capacity * solarProduct.Price, 2);
                total_price_str = total_price.ToString() + " EGP";
                unit += "/W";
            }
            else
            {
                total_price = null;
                total_price_str = null;
            }
            return new SolarProductDto
            {
                Id = solarProduct.Id,
                CategoryName = solarProduct.ProductCategory.Name,
                Price = solarProduct.Price,
                Capacity = solarProduct.Capacity,
                TotalPrice = total_price,
                PriceStr = Math.Round(solarProduct.Price, 2).ToString() + " EGP"+ unit,
                TotalPriceStr = total_price_str,
                MeasuringUnit = solarProduct.MeasuringUnit,
                Capacity_Unit = capacity_unit,
                Brand = solarProduct.Brand,
                ImageUrl = solarProduct.ImageUrl.Substring(solarProduct.ImageUrl.IndexOf("SolarProductImages")).Replace('\\', '/'),
                IsProductPost = solarProduct.IsProductPost,
            };
        }

        public static List<SolarProductDto> ProductsInfo(this List<SolarProduct> solarProducts)
        {

            var ProductsDtoList = new List<SolarProductDto>();
            foreach (var solarProduct in solarProducts)
            {
                string capacity_unit;
                if (solarProduct.ProductCategory.Name == "Battery")
                {
                    capacity_unit = solarProduct.Volt + "V-" +
                        solarProduct.Capacity.ToString() + solarProduct.MeasuringUnit;
                }
                else
                {
                    capacity_unit = solarProduct.Capacity.ToString() + solarProduct.MeasuringUnit;
                }
                double? total_price;
                string? total_price_str;
                string unit = "";
                if (solarProduct.ProductCategory.Name == "Solar Panel")
                {
                    total_price = Math.Round(solarProduct.Capacity * solarProduct.Price, 2);
                    total_price_str = total_price.ToString() + " EGP";
                    unit += "/W";
                }
                else
                {
                    total_price = null;
                    total_price_str = null;
                }
                ProductsDtoList.Add(
                    new SolarProductDto
                    {
                        Id = solarProduct.Id,
                        CategoryName = solarProduct.ProductCategory.Name,
                        Price = solarProduct.Price,
                        Capacity = solarProduct.Capacity,
                        MeasuringUnit = solarProduct.MeasuringUnit,
                        TotalPrice = total_price,
                        PriceStr = Math.Round(solarProduct.Price, 2).ToString() + " EGP"+unit,
                        TotalPriceStr = total_price_str,
                        Capacity_Unit = capacity_unit,
                        Brand = solarProduct.Brand,
                        ImageUrl = solarProduct.ImageUrl.Substring(solarProduct.ImageUrl.IndexOf("SolarProductImages")).Replace('\\', '/'),
                        IsProductPost = solarProduct.IsProductPost,
                    }
                    );
            }
            return ProductsDtoList;
        }

        //**************************************************************************************************
        public static FavoriteProductDto FavoriteProductInfo(this FavoriteProduct favoriteProduct)
        {
            string capacity_unit;
            if (favoriteProduct.Product.ProductCategory.Name == "Battery")
            {
                capacity_unit = favoriteProduct.Product.Volt + "V-" +
                    favoriteProduct.Product.Capacity.ToString() + favoriteProduct.Product.MeasuringUnit;
            }
            else
            {
                capacity_unit = favoriteProduct.Product.Capacity.ToString() + favoriteProduct.Product.MeasuringUnit;
            }
            double? total_price;
            string? total_price_str;
            string unit = "";
            if (favoriteProduct.Product.ProductCategory.Name == "Solar Panel")
            {
                total_price = Math.Round(favoriteProduct.Product.Capacity * favoriteProduct.Product.Price, 2);
                total_price_str = total_price.ToString() + " EGP";
                unit += "/W";
            }
            else
            {
                total_price = null;
                total_price_str = null;
            }
            return new FavoriteProductDto
            {
                PersonId = favoriteProduct.PersonId,
                ProductId = favoriteProduct.ProductId,
                AddedDate = favoriteProduct.AddedDate,
                personName = favoriteProduct.Person.Name,
                ProfileImageUrl = favoriteProduct.Person?.Profile?.ImageUrl != null
                        ? favoriteProduct.Person.Profile.ImageUrl.Substring(favoriteProduct.Person.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/') 
                        : "ProfileImages/profile.png",
                Price = favoriteProduct.Product.Price,
                ProductImageUrl = favoriteProduct.Product.ImageUrl.Substring(favoriteProduct.Product.ImageUrl.IndexOf("SolarProductImages")).Replace('\\', '/'),
                Brand = favoriteProduct.Product.Brand,
                Capacity = favoriteProduct.Product.Capacity,
                MeasuringUnit= favoriteProduct.Product.MeasuringUnit,
                Capacity_Unit=capacity_unit,
                PriceStr = Math.Round(favoriteProduct.Product.Price, 2).ToString() + " EGP"+unit,
                TotalPriceStr = total_price_str,
                TotalPrice = total_price,
                IsProductPost = favoriteProduct.Product.IsProductPost,
                CategoryName = favoriteProduct.Product.ProductCategory.Name,
            };
        }

        public static List<FavoriteProductDto> FavoriteProductsInfo(this List<FavoriteProduct> favoriteProducts)
        {

            var FavoriteProductsDtoList = new List<FavoriteProductDto>();
            foreach (var favoriteProduct in favoriteProducts)
            {
                string capacity_unit;
                if (favoriteProduct.Product.ProductCategory.Name == "Battery")
                {
                    capacity_unit = favoriteProduct.Product.Volt + "V-" +
                        favoriteProduct.Product.Capacity.ToString() + favoriteProduct.Product.MeasuringUnit;
                }
                else
                {
                    capacity_unit = favoriteProduct.Product.Capacity.ToString() + favoriteProduct.Product.MeasuringUnit;
                }
                double? total_price;
                string? total_price_str;
                string unit = "";
                if (favoriteProduct.Product.ProductCategory.Name == "Solar Panel")
                {
                    total_price = Math.Round(favoriteProduct.Product.Capacity * favoriteProduct.Product.Price, 2);
                    total_price_str = total_price.ToString() + " EGP";
                    unit += "/W";
                }
                else
                {
                    total_price = null;
                    total_price_str = null;
                }
                FavoriteProductsDtoList.Add(
                    new FavoriteProductDto
                    {
                        PersonId = favoriteProduct.PersonId,
                        ProductId = favoriteProduct.ProductId,
                        AddedDate = favoriteProduct.AddedDate,
                        personName = favoriteProduct.Person.Name,
                        ProfileImageUrl = favoriteProduct.Person?.Profile?.ImageUrl != null
                        ? favoriteProduct.Person.Profile.ImageUrl.Substring(favoriteProduct.Person.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/') 
                        : "ProfileImages/profile.png",
                        Price = favoriteProduct.Product.Price,
                        ProductImageUrl = favoriteProduct.Product.ImageUrl.Substring(favoriteProduct.Product.ImageUrl.IndexOf("SolarProductImages")).Replace('\\', '/'),
                        Brand = favoriteProduct.Product.Brand,
                        Capacity = favoriteProduct.Product.Capacity,
                        MeasuringUnit= favoriteProduct.Product.MeasuringUnit,
                        Capacity_Unit = capacity_unit,
                        TotalPrice = total_price,
                        PriceStr = Math.Round(favoriteProduct.Product.Price, 2).ToString() + " EGP"+unit,
                        TotalPriceStr = total_price_str,
                        IsProductPost = favoriteProduct.Product.IsProductPost,
                        CategoryName = favoriteProduct.Product.ProductCategory.Name,
                    }
                    );
            }
            return FavoriteProductsDtoList;
        }

        //**************************************************************************************************
        public static Profile ProfileCreate(this CreateProfileDto createProfileDto)
        {
            return new Profile
            {
                Type = createProfileDto.Type,
            };
        }

        public static Account AccountUpdate(this UpdateAccountDto updateAccountDto)
        {
            return new Account
            {
                UserName = updateAccountDto.Email,
                Email = updateAccountDto.Email

            };
        }

        public static Account AddAccount(this AddAccountDto addAccountDto)
        {
            return new Account
            {
                UserName = addAccountDto.Email,
                Email = addAccountDto.Email,
                Type = addAccountDto.Type
            };
        }
        //*******************************************************************************
        public static CalculatorDto CalculatorInfo(this Calculator calculator)
        {
            var calculatorDto = new CalculatorDto
            {
                Id = calculator.Id,
                PersonId = calculator.PersonId,
                SystemSize = calculator.SystemSize.ToString(),
                RoofSpace = calculator.RoofSpace.ToString(),
                PanelsCapacity = "(" + calculator.MiniPanelsCapacity.ToString() + "-" + calculator.MaxPanelsCapacity.ToString() + ") Watt",
                NumofPanels = "(" + calculator.MiniNumofPanels.ToString() + "-" + calculator.MaxNumofPanels.ToString() + ")",
                PanelsPrice = "(" + calculator.MiniPanelsPrice.ToString() + "-" + calculator.MaxPanelsPrice.ToString() + ") EGP",
                InverterCapacity = calculator.InverterCapacity.ToString() + "KW",
                InverterPrice = Math.Round(calculator.InverterPrice, 2).ToString() + " EGP",
                TotalCost = Math.Round(calculator.TotalCost, 2).ToString() + " EGP",
                EnvironmentalBenefitMonthly = calculator.EnvironmentalBenefitMonthly.ToString() + " kg",
                EnvironmentalBenefitYearly = calculator.EnvironmentalBenefitYearly.ToString() + " kg",
                EnvironmentalBenefitTwentyFiveYear = calculator.EnvironmentalBenefitTwentyFiveYear.ToString() + " kg",
            };

            if(calculator.BatteryCapacity != null && calculator.NumofBatteries != null && calculator.BatteryPrice != null)
            {
                calculatorDto.BatteryCapacity = "12V-" + calculator.BatteryCapacity.ToString() + "A";
                calculatorDto.NumofBatteries = calculator.NumofBatteries.ToString();
                calculatorDto.BatteryPrice = calculator.BatteryPrice.ToString() + " EGP";
            }
            else
            {
                calculatorDto.FinancialSavingMonthly = Math.Round((double)calculator.FinancialSavingMonthly, 2).ToString() + " EGP";
                calculatorDto.FinancialSavingYearly = Math.Round((double)calculator.FinancialSavingYearly, 2).ToString() + " EGP";
                calculatorDto.FinancialSavingTwentyFiveYear = Math.Round((double)calculator.FinancialSavingTwentyFiveYear, 2).ToString() + " EGP";
                calculatorDto.PaybackPeriod = calculator.PaybackPeriod.ToString() + " Years";
            }
            return calculatorDto;
        }

        public static List<CalculatorDto> CalculatorsInfo(this List<Calculator> calculators)
        {
            var calculatorsDtoList = new List<CalculatorDto>();
            foreach (var calculator in calculators)
            {
                var calculatorDto = new CalculatorDto
                {
                    Id = calculator.Id,
                    PersonId = calculator.PersonId,
                    SystemSize = calculator.SystemSize.ToString(),
                    RoofSpace = calculator.RoofSpace.ToString(),
                    PanelsCapacity = "(" + calculator.MiniPanelsCapacity.ToString() + "-" + calculator.MaxPanelsCapacity.ToString() + ") Watt",
                    NumofPanels = "(" + calculator.MiniNumofPanels.ToString() + "-" + calculator.MaxNumofPanels.ToString() + ")",
                    PanelsPrice = "(" + calculator.MiniPanelsPrice.ToString() + "-" + calculator.MaxPanelsPrice.ToString() + ") EGP",
                    InverterCapacity = calculator.InverterCapacity.ToString() + "KW",
                    InverterPrice = Math.Round(calculator.InverterPrice, 2).ToString() + " EGP",
                    TotalCost = Math.Round(calculator.TotalCost, 2).ToString() + " EGP",
                    EnvironmentalBenefitMonthly = calculator.EnvironmentalBenefitMonthly.ToString() + " kg",
                    EnvironmentalBenefitYearly = calculator.EnvironmentalBenefitYearly.ToString() + " kg",
                    EnvironmentalBenefitTwentyFiveYear = calculator.EnvironmentalBenefitTwentyFiveYear.ToString() + " kg",
                };

                if (calculator.BatteryCapacity != null && calculator.NumofBatteries != null && calculator.BatteryPrice != null)
                {
                    calculatorDto.BatteryCapacity = "12V-" + calculator.BatteryCapacity.ToString() + "A";
                    calculatorDto.NumofBatteries = calculator.NumofBatteries.ToString();
                    calculatorDto.BatteryPrice = calculator.BatteryPrice.ToString() + " EGP";
                }
                else
                {
                    calculatorDto.FinancialSavingMonthly = Math.Round((double)calculator.FinancialSavingMonthly, 2).ToString() + " EGP";
                    calculatorDto.FinancialSavingYearly = Math.Round((double)calculator.FinancialSavingYearly, 2).ToString() + " EGP";
                    calculatorDto.FinancialSavingTwentyFiveYear = Math.Round((double)calculator.FinancialSavingTwentyFiveYear, 2).ToString() + " EGP";
                    calculatorDto.PaybackPeriod = calculator.PaybackPeriod.ToString() + " Years";
                }
                calculatorsDtoList.Add(calculatorDto);
            }
            return calculatorsDtoList;
        }

        //*******************************************************************************
        public static PeakHour PeakHourCreate(this CreatePeakHourDto createPeakHourDto)
        {
            return new PeakHour
            {
                City = createPeakHourDto.City,
                PeakSunlightHour = (createPeakHourDto.YearlyInPlaneIrradiation) / 365
            };
        }

        //*******************************************************************************
        public static List<DailyEnergyPredictionDto> DailyPredctionsInfo(this List<DailyAllSkyPrediction> dailyAllSkyPredictions)
        {
            var DailyEnergyPredictionDtoList = new List<DailyEnergyPredictionDto>();
            foreach (var dailyAllSkyPrediction in dailyAllSkyPredictions)
            {
                DailyEnergyPredictionDtoList.Add(
                    new DailyEnergyPredictionDto
                    {
                        Id = dailyAllSkyPrediction.Id,
                        City = dailyAllSkyPrediction.City,
                        Date = dailyAllSkyPrediction.Date.Substring(0, 3),
                        DayNum = dailyAllSkyPrediction.DayNum,
                        PredictedEnergy = dailyAllSkyPrediction.PredictedAllSky,
                       
                    }
                    );
            }
            return DailyEnergyPredictionDtoList;
        }

        public static DailyEnergyPredictionDto DailyPredctionInfo(this DailyAllSkyPrediction dailyAllSkyPrediction)
        {
            return new DailyEnergyPredictionDto
            {
                Id = dailyAllSkyPrediction.Id,
                City = dailyAllSkyPrediction.City,
                Date = dailyAllSkyPrediction.Date.Substring(0, 3),
                DayNum = dailyAllSkyPrediction.DayNum,
                PredictedEnergy = dailyAllSkyPrediction.PredictedAllSky,

            };
        }

        public static List<HourlyEnergyPredictionDto> HourlyPredctionsInfo(this List<HourlyAllSkyPrediction> hourlyAllSkyPredictions)
        {
            var HourlyEnergyPredictionDtoList = new List<HourlyEnergyPredictionDto>();
            foreach (var hourlyAllSkyPrediction in hourlyAllSkyPredictions)
            {
                HourlyEnergyPredictionDtoList.Add(
                    new HourlyEnergyPredictionDto
                    {
                        Id = hourlyAllSkyPrediction.Id,
                        City = hourlyAllSkyPrediction.City,
                        Date = hourlyAllSkyPrediction.Date.Substring(0, 3),
                        Hour = hourlyAllSkyPrediction.Hour,
                        DayNum = hourlyAllSkyPrediction.DayNum,
                        PredictedEnergy = hourlyAllSkyPrediction.PredictedAllSky,

                    }
                    );
            }
            return HourlyEnergyPredictionDtoList;
        }

        //*******************************************************************************
        public static FAQDto FAQInfo(this FAQ fAQ)
        {
            return new FAQDto
            {
                Id = fAQ.Id,
                Answer = fAQ.Answer,
                Question = fAQ.Question,
                Active = fAQ.Active,
                Order = fAQ.Order,
                FAQCategoryId = fAQ.FAQCategoryId,
                FAQCategoryName = fAQ.FAQCategory.Name
            };
        }

        public static List<FAQDto> FAQsInfo(this List<FAQ> fAQs)
        {
            var fQADtoList = new List<FAQDto>();
            foreach (var fAQ in fAQs)
            {
                fQADtoList.Add(
                    new FAQDto
                    {

                        Id = fAQ.Id,
                        Answer = fAQ.Answer,
                        Question = fAQ.Question,
                        Active = fAQ.Active,
                        Order = fAQ.Order,
                        FAQCategoryId = fAQ.FAQCategoryId,
                        FAQCategoryName = fAQ.FAQCategory.Name

                    }
                    );
            }
            return fQADtoList;
        }

        //*******************************************************************************

        public static ChatbotMessageDto ChatbotMessageInfo(this ChatbotMessage chatbotMessage)
        {
            return new ChatbotMessageDto
            {
                Id = chatbotMessage.Id,
                Date = chatbotMessage.Date,
                ImageUrl = chatbotMessage.Person?.Profile?.ImageUrl != null
                        ? chatbotMessage.Person.Profile.ImageUrl.Substring(chatbotMessage.Person.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/')
                        : "ProfileImages/profile.png",
                Answer = chatbotMessage.Answer,
                Question = chatbotMessage.Question,
                PersonId = chatbotMessage.PersonId,
            };
        }

        public static List<ChatbotMessageDto> ChatbotMessagesInfo(this List<ChatbotMessage> chatbotMessages)
        {
            var chatbotMessageDtoList = new List<ChatbotMessageDto>();
            foreach (var chatbotMessage in chatbotMessages)
            {
                chatbotMessageDtoList.Add(
                    new ChatbotMessageDto
                    {
                        Id = chatbotMessage.Id,
                        Date = chatbotMessage.Date,
                        ImageUrl = chatbotMessage.Person?.Profile?.ImageUrl != null
                        ? chatbotMessage.Person.Profile.ImageUrl.Substring(chatbotMessage.Person.Profile.ImageUrl.IndexOf("ProfileImages")).Replace('\\', '/')
                        : "ProfileImages/profile.png",
                        Answer = chatbotMessage.Answer,
                        Question = chatbotMessage.Question,
                        PersonId = chatbotMessage.PersonId,
                    }
                    );
            }
            return chatbotMessageDtoList;
        }

        //*******************************************************************************
        public static MessageDto MessageInfo(this Message message)
        {
            return new MessageDto
            {
                Id = message.Id,
                Body = message.Body,
                Title = message.Title,
                SentDate = message.SentDate.ToString("dd/MM/yyyy 'at' hh:mm tt")
            };
        }

        public static List<MessageDto> MessagesInfo(this List<Message> messages)
        {
            var messageDtoList = new List<MessageDto>();
            foreach (var message in messages)
            {
                messageDtoList.Add(
                    new MessageDto
                    {
                        Id = message.Id,
                        Body = message.Body,
                        Title = message.Title,
                        SentDate = message.SentDate.ToString("dd/MM/yyyy 'at' hh:mm tt")
                    }
                    );
            }
            return messageDtoList;
        }
    }
}
