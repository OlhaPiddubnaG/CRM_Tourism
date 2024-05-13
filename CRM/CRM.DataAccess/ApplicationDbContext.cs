using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Task = CRM.Domain.Entities.Task;

namespace CRM.DataAccess;

public class ApplicationDbContext : DbContext
{
    
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public  DbSet<City> Cities { get; set; }
    
    public  DbSet<Client> Clients { get; set; }
    
    public  DbSet<ClientPrivateData> ClientPrivateDatas { get; set; }
    
    public  DbSet<Company> Companies { get; set; }
    
    public  DbSet<Country> Countries { get; set; }
    
    public  DbSet<Meals> Meals { get; set; }
    
    public  DbSet<NumberOfPeople> NumberOfPeople { get; set; }
    
    public  DbSet<Order> Orders { get; set; }
    
    public  DbSet<OrderStatusHistory> OrderStatusHistory { get; set; }
    
    public  DbSet<PassportInfo> PassportInfo { get; set; }
    
    public  DbSet<Payment> Payments { get; set; }
    
    public  DbSet<Role> Roles { get; set; }
    
    public  DbSet<Task> Tasks { get; set; }
    
    public  DbSet<Touroperator> Touroperators { get; set; }
    
    public  DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
