using AdminSite.Manager;
using AdminSite.Policies;
using Internals.Database;
using Internals.Models;
using Internals.Models.Enum;
using Internals.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
namespace AdminSite;

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
        services.AddCookiePolicy(options =>
        {
            options.CheckConsentNeeded = _ => true;  
            options.MinimumSameSitePolicy = SameSiteMode.None;  
        });
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        

        // Add DI
        services.AddScoped<IRepository<Admin, int>, AdminRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        
        services.AddScoped<IRepository<Category, int>, CategoryRepository>();
        
        services.AddScoped<IRepository<Phone, int>, PhoneRepository>();
        services.AddScoped<IPhoneRepository, PhoneRepository>();
        
        services.AddScoped<IRepository<User, int>, UserRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped<IRepository<PhoneDetails, int>, PhoneDetailRepository>();
        services.AddScoped<IPhoneDetailRepository, PhoneDetailRepository>();
        
        services.AddScoped<ImageService>();
        services.AddScoped<IImageRepository,ImageRepository>();

        services.AddScoped<IRepository<Role, int>, RoleRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Manage_Model.Manage_Admin.ToString(), policy => policy.Requirements.Add(new ManageModelRequirement(Manage_Model.Manage_Admin)));
            options.AddPolicy(Manage_Model.Manage_Category.ToString(), policy => policy.Requirements.Add(new ManageModelRequirement(Manage_Model.Manage_Category)));
            options.AddPolicy(Manage_Model.Manage_Notify.ToString(), policy => policy.Requirements.Add(new ManageModelRequirement(Manage_Model.Manage_Notify)));
            options.AddPolicy(Manage_Model.Manage_Order.ToString(), policy => policy.Requirements.Add(new ManageModelRequirement(Manage_Model.Manage_Order)));
            options.AddPolicy(Manage_Model.Manage_Phone.ToString(), policy => policy.Requirements.Add(new ManageModelRequirement(Manage_Model.Manage_Phone)));
            options.AddPolicy(Manage_Model.Manage_Promotion.ToString(), policy => policy.Requirements.Add(new ManageModelRequirement(Manage_Model.Manage_Promotion)));
            options.AddPolicy(Manage_Model.Manage_User.ToString(), policy => policy.Requirements.Add(new ManageModelRequirement(Manage_Model.Manage_User)));
        });
        
        services.AddScoped<IAuthorizationHandler, ManageModelHandler>();
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
        app.UseStatusCodePagesWithRedirects("/Home/Error");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseCookiePolicy();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Account}/{action=Index}/{id?}");
            endpoints.MapControllerRoute(
                "Home",
                "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapControllerRoute(
                "Admin",
                "{controller=Admin}/{action=Index}/{id?}");
            endpoints.MapControllerRoute(
                "User",
                "{controller=User}/{action=Index}/{id?}");
            endpoints.MapControllerRoute(
                "Phone",
                "{controller=Phone}/{action=Index}/{id?}");
            endpoints.MapControllerRoute(
                "PhoneDetail",
                "{controller=PhoneDetail}/{action=Index}/{id?}");
        });
    }
}