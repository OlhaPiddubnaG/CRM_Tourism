using CRM.Domain.Entities.Base;

namespace CRM.Domain.Entities;

public class Order : Auditable
{
    public Guid UserId { get; set; } 
    public User? User { get; set; } 
    public Guid ClientId { get; set; } 
    public Client? Client { get; set; } 
    public Guid CompanyId { get; set; } 
    public Company? Company { get; set; }
    public Touroperator? Touroperator { get; set; } 
    public DateTime DateFrom { get; set; } 
    public DateTime DateTo { get; set; } 
    public Country? CountryFrom { get; set; } 
    public Country? CountryTo { get; set; } 
    public int NumberOfNights { get; set; } 
    public decimal Amount { get; set; }
    public string Comment { get; set; }
    public List<OrderStatusHistory> OrderStatusHistory { get; set; } = new(); 
    public List<Payment> Payments { get; set; } = new(); 
    public List<Stays> Stays { get; set; } = new(); 
    public List<NumberOfPeople> NumberOfPeople { get; set; } = new(); 
}