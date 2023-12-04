namespace ApiService.Domain.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<Employee>? Employees { get; set; }
}
