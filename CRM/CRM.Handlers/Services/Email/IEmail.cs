namespace CRM.Handlers.Services;

public interface IEmail
{
    Task SendEmailAsync(string toEmail, string subject, string message);
}