using Internals.Database;
using Internals.Models;
using Internals.Repository;
using Internals.Services;
using Microsoft.EntityFrameworkCore;
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
        
        // connect redis db 
        var redisConfigurationOptions = ConfigurationOptions.Parse(Configuration["Redis:Configuration"]);
        services.AddStackExchangeRedisCache(redisCacheConfig =>
        {
            redisCacheConfig.ConfigurationOptions = redisConfigurationOptions;
        });
        // Add the session.
        var cookieName = Configuration["Session:CookieName"];
        var idleTimeout = TimeSpan.FromMinutes(int.Parse(Configuration["Session:IdleTimeout"]));
        services.AddSession(options =>
        {
            options.Cookie.Name = cookieName;
            options.IdleTimeout = idleTimeout;
        });
        // Add DI
        
        services.AddScoped<IRepository<Category, int>, CategoryRepository>();
        
        services.AddScoped<IRepository<Phone, int>, PhoneRepository>();
        services.AddScoped<IPhoneRepository, PhoneRepository>();
        
        services.AddScoped<IRepository<User, int>, UserRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped<IRepository<PhoneDetails, int>, PhoneDetailRepository>();
        services.AddScoped<IPhoneDetailRepository, PhoneDetailRepository>();
        
        services.AddScoped<IRepository<Order, int>, OrderRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        
        services.AddScoped<IRepository<Payment, int>, PaymentRepository>();
        
        services.AddScoped<IRepository<Promotion, int>, PromotionRepository>();
        
        
        // Service DI
        services.AddScoped<IPhoneDetailService, PhoneDetailService>();
        services.AddScoped<RedisService>();
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
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseCookiePolicy();
        app.UseRouting();
        app.UseSession();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=HomePage}/{action=Index}/{id?}");
        });
    }
}