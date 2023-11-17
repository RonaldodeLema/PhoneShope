using System.Security.Cryptography;
using System.Text;
using Internals.Database;
using Internals.Models;
using Internals.Repository;
using Internals.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using UserSite.Services;
using Order = Internals.Models.Order;

namespace UserSite;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // connect mySQL
        services.AddDbContext<DbPhoneStoreContext>(options =>
            options.UseMySQL(Configuration.GetConnectionString("MySQLConnection") ?? string.Empty));
        services.AddControllersWithViews();
        services.AddHttpContextAccessor();
        // config jwt with JwtBearer
        SiteKeys.Configure(Configuration.GetSection("AppSettings"));
        var key = Encoding.ASCII.GetBytes(SiteKeys.Token);
        
        // Add the session.
        var cookieName = Configuration["Session:CookieName"];
        var idleTimeout = TimeSpan.FromMinutes(int.Parse(Configuration["Session:IdleTimeout"]));
        services.AddSession(options =>
        {
            options.Cookie.Name = cookieName;
            options.IdleTimeout = idleTimeout;
        });
        services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(token =>
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = true;
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = SiteKeys.WebSiteDomain,
                    ValidateAudience = true,
                    ValidAudience = SiteKeys.WebSiteDomain,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        // connect redis db 
        var redisConfigurationOptions = ConfigurationOptions.Parse(Configuration["Redis:Configuration"]);
        services.AddStackExchangeRedisCache(redisCacheConfig =>
        {
            redisCacheConfig.ConfigurationOptions = redisConfigurationOptions;
        });
        // Add DI
        
        services.AddScoped<IRepository<Category, int>, CategoryRepository>();
        
        services.AddScoped<IRepository<Phone, int>, PhoneRepository>();
        services.AddScoped<IPhoneRepository, PhoneRepository>();
        
        services.AddScoped<IRepository<User, int>, UserRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<UserRepository>();
        
        services.AddScoped<IRepository<PhoneDetails, int>, PhoneDetailRepository>();
        services.AddScoped<IPhoneDetailRepository, PhoneDetailRepository>();
        
        services.AddScoped<IRepository<Order, int>, OrderRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<OrderRepository>();

        services.AddScoped<IRepository<Payment, int>, PaymentRepository>();
        
        services.AddScoped<IRepository<Promotion, int>, PromotionRepository>();
        
        services.AddScoped<IRepository<Payment, int>, PaymentRepository>();
        
        services.AddScoped<PromotionRepository>();
        
        
        // Service DI
        services.AddScoped<IPhoneDetailService, PhoneDetailService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<RedisService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPromotionService, PromotionService>();
        services.AddScoped<IOrderService,OrderService>();
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        
        // app.Use(async (context, next) =>
        // {
        //     await next();
        //     if (context.Response.StatusCode == 404)
        //     {
        //         context.Request.Path = "/HomePage/Error";
        //         await next();
        //     }
        //     if (context.Response.StatusCode == 401)
        //     {
        //         context.Request.Path = "/Account/Denied";
        //         await next();
        //     }
        // });
        app.UseCors();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();
        app.UseRouting();
        app.UseSession();
        app.Use(async (context, next) =>
        {
            var jwToken = context.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(jwToken))
            {
                context.Request.Headers.Add("Authorization", "Bearer " + jwToken);
            }
            await next();
        });
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=HomePage}/{action=Index}/{id?}");
        });
    }
}