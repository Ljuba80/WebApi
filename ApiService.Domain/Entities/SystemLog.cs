namespace ApiService.Domain.Entities;

public enum Event
{
    Create,
    Update
}

public enum ResourceType
{
    Employee,
    Company
}

public class SystemLog
{
    public int Id { get; set; }
    public ResourceType ResourceType { get; set; }
    public int ResourceId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Event Event { get; set; }
    //resource attributes
    public string Comment { get; set; } = string.Empty;
}
