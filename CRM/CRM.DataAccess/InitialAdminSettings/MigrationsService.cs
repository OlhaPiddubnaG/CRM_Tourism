using CRM.Domain.Entities;
using CRM.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Task = System.Threading.Tasks.Task;

namespace CRM.DataAccess.InitialAdminSettings;

public class MigrationsService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly InitialAdminSettings _initialAdminSettings;

    public MigrationsService(IServiceProvider serviceProvider,
        ILogger<MigrationsService> logger, 
        IOptions<InitialAdminSettings> initialAdminSettings)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _initialAdminSettings = initialAdminSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
    {
        using var scope = _serviceProvider.CreateScope();
        await using var context = scope.ServiceProvider.GetService<AppDbContext>()!;
        try
        {
            await context.Database.MigrateAsync(stoppingToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unhandled exceptions occurred while migrating database");
            return;
        }

        await SeedCompanyAndAdmin(context, stoppingToken);
    }
    
    private async Task SeedCompanyAndAdmin(AppDbContext context, CancellationToken cancellationToken)
    {
        var company = await SeedCompany(context, cancellationToken);
        await SeedAdmin(context, company.Id, cancellationToken);
    }

    private async Task<Company> SeedCompany(AppDbContext context, CancellationToken cancellationToken)
    {
        var existingCompany = await context.Companies
            .FirstOrDefaultAsync(x => x.Name == _initialAdminSettings.CompanyName, cancellationToken);

        if (existingCompany != null)
        {
            return existingCompany;
        }

        var company = new Company
        {
            Name = _initialAdminSettings.CompanyName,
        };

        context.Companies.Add(company);
        await context.SaveChangesAsync(cancellationToken);

        return company;
    }

    private async Task SeedAdmin(AppDbContext context, Guid companyId, CancellationToken cancellationToken)
    {
        var adminExists = await context.Users.AnyAsync(x => x.Email == _initialAdminSettings.Email, cancellationToken);

        if (adminExists)
        {
            return;
        }

        var admin = new User
        {
            Email = _initialAdminSettings.Email,
            Name = _initialAdminSettings.Name,
            Surname = _initialAdminSettings.Surname,
            Password = _initialAdminSettings.Password,
            CompanyId = companyId 
        };

        context.Users.Add(admin);

        var role = await context.Roles.FirstOrDefaultAsync(x => x.RoleType == RoleType.Admin, cancellationToken);
        if (role == null)
        {
            role = new Role
            {
                RoleType = RoleType.Admin,
                UserId = admin.Id,
                User = admin
            };
            context.Roles.Add(role);
        }
        else
        {
            role.UserId = admin.Id;
            role.User = admin;
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
