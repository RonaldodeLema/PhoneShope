using Microsoft.Extensions.Configuration;

namespace UserSite.Services;

using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string email, string resetToken, string callbackUrl);
    Task SendActiveEmailAsync(string email, string activeToken, string callbackUrl);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendPasswordResetEmailAsync(string email, string resetToken, string callbackUrl)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Reset your password", "your-email@example.com"));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Password Reset";
        var smtpSettings = _configuration.GetSection("SmtpSettings");
        callbackUrl = smtpSettings["WebSiteDomain"] + callbackUrl;
        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = $"<p>Click <a href=\"{callbackUrl}\">here</a> to reset your password.</p>";
        message.Body = bodyBuilder.ToMessageBody();
        using var client = new SmtpClient();
        await client.ConnectAsync(smtpSettings["SmtpServer"], int.Parse(smtpSettings["Port"]), bool.Parse(smtpSettings["EnableSsl"]));
        await client.AuthenticateAsync(smtpSettings["Username"], smtpSettings["Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendActiveEmailAsync(string email, string activeToken, string callbackUrl)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Active your account", "your-email@example.com"));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Active your account";
        var smtpSettings = _configuration.GetSection("SmtpSettings");
        callbackUrl = smtpSettings["WebSiteDomain"] + callbackUrl;
        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = $"<p>Click <a href=\"{callbackUrl}\">here</a> to active your account.</p>";
        message.Body = bodyBuilder.ToMessageBody();
        using var client = new SmtpClient();
        await client.ConnectAsync(smtpSettings["SmtpServer"], int.Parse(smtpSettings["Port"]), bool.Parse(smtpSettings["EnableSsl"]));
        await client.AuthenticateAsync(smtpSettings["Username"], smtpSettings["Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
