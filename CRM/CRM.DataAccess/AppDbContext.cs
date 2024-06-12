using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRM.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public  DbSet<Company> Companies { get; set; }
    
    public  DbSet<City> Cities { get; set; }
    
    public  DbSet<Client> Clients { get; set; }
    
    public  DbSet<ClientPrivateData> ClientPrivateDatas { get; set; }
    
    public  DbSet<Country> Countries { get; set; }
    
    public  DbSet<Meals> Meals { get; set; }
    
    public  DbSet<NumberOfPeople> NumberOfPeople { get; set; }
    
    public  DbSet<Order> Orders { get; set; }
    
    public  DbSet<OrderStatusHistory> OrderStatusHistory { get; set; }
    
    public  DbSet<PassportInfo> PassportInfo { get; set; }
    
    public  DbSet<Payment> Payments { get; set; }
    
    public  DbSet<UserRoles> UserRoles { get; set; }
    
    public  DbSet<UserTasks> UserTasks { get; set; }
    
    public  DbSet<Touroperator> Touroperators { get; set; }
    
    public  DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.Entity<Client>()
            .HasIndex(c => c.CountryId)
            .IsUnique(false);
    }
}
