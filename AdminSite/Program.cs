using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Internals.Database;

namespace AdminSite;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        CreateDbIfNotExists(host);

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DbPhoneStoreContext>();
                DbPhoneStoreInit.InitializeDatabase(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "push-notify-87a05-firebase-adminsdk.json")),
        });
        host.Run();
    }

    private static void CreateDbIfNotExists(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DbPhoneStoreContext>();
            DbPhoneStoreInit.InitializeDatabase(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}