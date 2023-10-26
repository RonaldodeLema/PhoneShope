using Internals.Models;
using Internals.Models.Enum;

namespace Internals.Database
{
    public static class DbPhoneStoreInit
    {
        [Obsolete("Obsolete")]
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
                    Description = "The latest and greatest iPhone from Apple.",
                    Image = "http://res.cloudinary.com/drabm29xb/image/upload/v1697284330/wslxavkrhbiigshaxjk1.png",
                    Brand = BrandPhone.Apple, CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "vanthao",
                    UpdatedBy = "vanthao"
                },
                new Phone
                {
                    CategoryId = 1, Name = "iPhone 13 Pro Max",
                    Description = "The latest and greatest Android phone from Samsung.",
                    Image = "http://res.cloudinary.com/drabm29xb/image/upload/v1697284330/wslxavkrhbiigshaxjk1.png",
                    Brand = BrandPhone.Apple, CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "vanthao",
                    UpdatedBy = "vanthao"
                },
                new Phone
                {
                    CategoryId = 2, Name = "SamSung Z Fold 4",
                    Description = "The latest and greatest Pixel phone from Google.",
                    Image = "http://res.cloudinary.com/drabm29xb/image/upload/v1697284330/wslxavkrhbiigshaxjk1.png",
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
                Image = "http://res.cloudinary.com/drabm29xb/image/upload/v1697284330/wslxavkrhbiigshaxjk1.png",
                Size = Size._6p5_inch,
                Color = Color.Red,
                RAM = RAM._6_GB,
                Storage = Storage._1_TB,
                Quantity = 100,
                Price = 30000000,
                PhoneId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "vanthao",
                UpdatedBy = "vanthao"
            };
            var phoneDetails2 = new PhoneDetails()
            {
                Image = "http://res.cloudinary.com/drabm29xb/image/upload/v1697284330/wslxavkrhbiigshaxjk1.png",
                Size = Size._5p42_inch,
                Color = Color.Black,
                RAM = RAM._8_GB,
                Storage = Storage._256_GB,
                Quantity = 100,
                Price = 35000000,
                PhoneId = 3,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "vanthao",
                UpdatedBy = "vanthao"
            };
            var phoneDetails1 = new PhoneDetails()
            {
                Image = "http://res.cloudinary.com/drabm29xb/image/upload/v1697284330/wslxavkrhbiigshaxjk1.png",
                Size = Size._6p7_inch,
                Color = Color.White,
                RAM = RAM._6_GB,
                Storage = Storage._512_GB,
                Quantity = 100,
                Price = 40000000,
                PhoneId = 2,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "vanthao",
                UpdatedBy = "vanthao"
            };
            var phoneDetails3 = new PhoneDetails()
            {
                Image = "http://res.cloudinary.com/drabm29xb/image/upload/v1697284330/wslxavkrhbiigshaxjk1.png",
                Size = Size._6p7_inch,
                Color = Color.Blue,
                RAM = RAM._6_GB,
                Storage = Storage._512_GB,
                Quantity = 100,
                Price = 45000000,
                PhoneId = 2,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "vanthao",
                UpdatedBy = "vanthao"
            };
            context.PhoneDetails.Add(phoneDetails);
            context.PhoneDetails.Add(phoneDetails1);
            context.PhoneDetails.Add(phoneDetails2);
            context.PhoneDetails.Add(phoneDetails3);
            context.SaveChanges();
            for (int i = 0; i <= 40; i++)
            {
                context.PhoneDetails.Add(new PhoneDetails()
                {
                    Image = "http://res.cloudinary.com/drabm29xb/image/upload/v1697284330/wslxavkrhbiigshaxjk1.png",
                    Size = (Size)(i%10),
                    Color = (Color)(i%15),
                    RAM = (RAM)(i%7),
                    Storage = (Storage)(i%7),
                    Quantity = 100,
                    Price = 30000000+i*200000,
                    PhoneId = i%3+1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = "vanthao",
                    UpdatedBy = "vanthao"
                });
            }
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
                ManageModel = ManageModel.Manage_Admin
            };
            var roleClaim2 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = ManageModel.Manage_Category
            };
            var roleClaim3 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = ManageModel.Manage_Notify
            };
            var roleClaim4 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = ManageModel.Manage_Order
            };
            var roleClaim5 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = ManageModel.Manage_Phone
            };
            var roleClaim6 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = ManageModel.Manage_Promotion
            };

            var roleClaim7 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = ManageModel.Manage_User
            };
            var roleClaim8 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = ManageModel.Manage_Phone_Detail
            };
            var roleClaim9 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = ManageModel.Manage_Role
            };
            var roleClaim10 = new RoleClaim()
            {
                RoleId = 1,
                ManageModel = ManageModel.Export_DataCsv
            };
            context.RoleClaims.Add(roleClaim1);
            context.RoleClaims.Add(roleClaim2);
            context.RoleClaims.Add(roleClaim3);
            context.RoleClaims.Add(roleClaim4);
            context.RoleClaims.Add(roleClaim5);
            context.RoleClaims.Add(roleClaim6);
            context.RoleClaims.Add(roleClaim7);
            context.RoleClaims.Add(roleClaim8);
            context.RoleClaims.Add(roleClaim9);
            context.RoleClaims.Add(roleClaim10);
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
                PhoneNumber = "0911452692",
                Address = "19 duong 85 tan quy quan 7",
                Email = "user@gmail.com",
                Avatar = "avatar.png",
                Username = "user",
                Password = "123456",
                DeviceToken =
                    "eh5de4GSo67fIneQoHF62Y:APA91bGIFAvHrfZa6oANuZzZR6Vfty2B7z0ATxn4YFndcFi4auEdR-WlPb6IEjWq5zj_3Dd7iFQuxzHdfp1MtuQvfgKGkDai-NvorIOevJup-oNcfOx2zXG3xdMKbe0tND3P0ZI3iPTN",
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
                PhoneNumber = "0911452692",
                Address = "19 duong 85 tan quy quan 7",
                Email = "user1@gmail.com",
                Avatar = "avatar.png",
                Username = "user1",
                Password = "123456",
                IsBlocked = false,
                DeviceToken =
                    "fzyRM6Y6OClBQYCA3MzwjM:APA91bFwNg89lbklDByO_t7DgLCsr18UzPIEisfTTIcKqtdm0gSDyy45RsKlTngfi0tMGPTpGeBZS5mtqsHxHwmtXqK3TW7k8aikmjAjGtOC1wnPj5ueEYc5gEYPPZA7NhJm3AXP_2r9",
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
            {
                OrderId = 1, PhoneDetailsId = 1, Price = phoneDetails.Price, Quantity = 2, CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var orderItem2 = new OrderItem()
            {
                OrderId = 1, PhoneDetailsId = 2, Price = phoneDetails1.Price, Quantity = 1, CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var orderItem3 = new OrderItem()
            {
                OrderId = 2, PhoneDetailsId = 1, Price = phoneDetails2.Price, Quantity = 2, CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var orderItem4 = new OrderItem()
            {
                OrderId = 3, PhoneDetailsId = 3, Price = phoneDetails2.Price, Quantity = 2, CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            context.OrderItems.Add(orderItem1);
            context.OrderItems.Add(orderItem2);
            context.OrderItems.Add(orderItem3);
            context.OrderItems.Add(orderItem4);
            context.SaveChanges();

            var payment = new Payment()
            {
                Owner = "TRAN VAN THAO",
                NumberCard = "0911452692",
                Method = Method.Momo,
                QRCode = "http://res.cloudinary.com/drabm29xb/image/upload/v1697284330/wslxavkrhbiigshaxjk1.png"
            };
            context.Payments.Add(payment);
            context.SaveChanges();

            var promotion = new Promotion
            {
                CategoryId = 1,
                UserId = 1,
                Name = "Sale off 20/10/2023",
                Code = "AJSHASDAK",
                IsUsed = false,
                Description = "sale of 20/10 have many promotion and deep sale",
                StartDate = new DateTime(2023, 10, 20),
                EndDate = new DateTime(2023, 10, 25),
                MinTotal = 30000000,
                MaxReduce = 2000000,
                Percentage = 0.1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "vanthao",
                UpdatedBy = "vanthao"
            };
            context.Promotions.Add(promotion);
            context.SaveChanges();
            var dict = new Dictionary<string, string>();
            dict.Add("Say", "Hello bruh");
            var notify = new Notify
            {
                Title = "Say hello all member in store",
                Body = "Hello bro",
                Data = dict,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "vanthao",
                UpdatedBy = "vanthao"
            };
            context.Notifications.Add(notify);
            context.SaveChanges();
        }
    }
}