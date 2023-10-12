using Internals.Models;
using Internals.Models.Enum;

namespace Internals.Database
{
    public static class DbPhoneStoreInit
    {
        public static void InitializeDatabase(DbPhoneStoreContext context)
        {
            context.Database.EnsureCreated();

            if (context.Admins.Any())
            {
                return; // DB has been seeded
            }

            // Create some book categories
            var category = new Category
            {
                Name = "IPhone",
                Description =
                    "A mobile device that can be used to make calls, send text messages, and access the internet.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "vanthao",
                UpdatedBy = "vanthao"
            };
            var category1 = new Category
            {
                Name = "SamSung",
                Description =
                    "A mobile device that can be used to make calls, send text messages, and access the internet.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "vanthao",
                UpdatedBy = "vanthao"
            };

            context.Categories.Add(category);

            context.Categories.Add(category1);
            context.SaveChanges();

            var phones = new List<Phone>
            {
                new Phone
                {
                    CategoryId = 1, Name = "iPhone 14 Pro Max",
                    Description = "The latest and greatest iPhone from Apple.", Image = "iphone-15-pro-max.png",
                    Brand = BrandPhone.Apple, CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "vanthao",
                    UpdatedBy = "vanthao"
                },
                new Phone
                {
                    CategoryId = 1, Name = "iPhone 13 Pro Max",
                    Description = "The latest and greatest Android phone from Samsung.",
                    Image = "iphone-15-pro-max.png", Brand = BrandPhone.Apple, CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "vanthao",
                    UpdatedBy = "vanthao"
                },
                new Phone
                {
                    CategoryId = 2, Name = "SamSung Z Fold 4",
                    Description = "The latest and greatest Pixel phone from Google.", Image = "iphone-15-pro-max.png",
                    Brand = BrandPhone.Samsung, CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "vanthao",
                    UpdatedBy = "vanthao"
                },
            };
            phones.ForEach(phone => context.Phones.Add(phone));
            context.SaveChanges();

            var phoneDetails = new PhoneDetails()
            {
                Size = Size._6p5_inch,
                Color = Color.Red,
                RAM = RAM._6_GB,
                Storage = Storage._1_TB,
                Quantity = 100,
                Price = 1000,
                PhoneId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "vanthao",
                UpdatedBy = "vanthao"
            };
            var phoneDetails2 = new PhoneDetails()
            {
                Size = Size._5p42_inch,
                Color = Color.Black,
                RAM = RAM._8_GB,
                Storage = Storage._256_GB,
                Quantity = 100,
                Price = 1000,
                PhoneId = 3,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "vanthao",
                UpdatedBy = "vanthao"
            };
            var phoneDetails1 = new PhoneDetails()
            {
                Size = Size._6p7_inch,
                Color = Color.White,
                RAM = RAM._6_GB,
                Storage = Storage._512_GB,
                Quantity = 100,
                Price = 1000,
                PhoneId = 2,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "vanthao",
                UpdatedBy = "vanthao"
            };

            context.PhoneDetails.Add(phoneDetails);
            context.PhoneDetails.Add(phoneDetails1);
            context.PhoneDetails.Add(phoneDetails2);
            context.SaveChanges();
            
            var role = new Role()
            {
                Name = "Root Admin"
            };
            context.Roles.Add(role);
            context.SaveChanges();
            var roleClaim1 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = Manage_Model.Manage_Admin
            };
            var roleClaim2 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = Manage_Model.Manage_Category
            };
            var roleClaim3 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = Manage_Model.Manage_Notify
            };
            var roleClaim4 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = Manage_Model.Manage_Order
            };
            var roleClaim5 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = Manage_Model.Manage_Phone
            };
            var roleClaim6 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = Manage_Model.Manage_Promotion
            };
            
            var roleClaim7 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = Manage_Model.Manage_User
            };
            context.RoleClaims.Add(roleClaim1);
            context.RoleClaims.Add(roleClaim2);
            context.RoleClaims.Add(roleClaim3);
            context.RoleClaims.Add(roleClaim4);
            context.RoleClaims.Add(roleClaim5);
            context.RoleClaims.Add(roleClaim6);
            context.RoleClaims.Add(roleClaim7);
            context.SaveChanges();
            var admin = new Admin()
            {
                Username = "vanthao",
                Password = "123456",
                RoleId = 1
            };
            admin.HashPassword();
            context.Admins.Add(admin);
            context.SaveChanges();
            
            var user = new User()
            {
                Name = "tran van thao",
                Email = "user@gmail.com",
                Avatar = "avatar.png",
                Username = "user",
                Password = "123456",
                IsBlocked = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            user.HashPassword();
            context.Users.Add(user);
            context.SaveChanges();
            user = new User()
            {
                Name = "tran van",
                Email = "user1@gmail.com",
                Avatar = "avatar.png",
                Username = "user1",
                Password = "123456",
                IsBlocked = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            user.HashPassword();
            context.Users.Add(user);
            context.SaveChanges();
            var order1 = new Order()
                { UserId = 1, Status = Status.Success, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            var order2 = new Order()
                { UserId = 1, Status = Status.Depending, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            var order3 = new Order()
                { UserId = 2, Status = Status.Success, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

            context.Orders.Add(order1);
            context.Orders.Add(order2);
            context.Orders.Add(order3);
            context.SaveChanges();
            var orderItem1 = new OrderItem()
                { OrderId = 1, PhoneDetailsId = 1, Quantity = 2, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            var orderItem2 = new OrderItem()
                { OrderId = 1, PhoneDetailsId = 2, Quantity = 1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            var orderItem3 = new OrderItem()
                { OrderId = 2, PhoneDetailsId = 1, Quantity = 2, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            var orderItem4 = new OrderItem()
                { OrderId = 3, PhoneDetailsId = 3, Quantity = 2, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };

            context.OrderItems.Add(orderItem1);
            context.OrderItems.Add(orderItem2);
            context.OrderItems.Add(orderItem3);
            context.OrderItems.Add(orderItem4);
            context.SaveChanges();
        }
    }
}