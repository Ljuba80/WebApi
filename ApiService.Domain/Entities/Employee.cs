namespace ApiService.Domain.Entities;

public enum Title
{
    Developer,
    Manager,
    Tester
}
public class Employee
{
    public int Id { get; set; }
    public Title Title { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<Company>? Companies { get; set; }
}
