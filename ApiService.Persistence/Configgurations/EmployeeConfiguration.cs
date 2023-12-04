using ApiService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiService.Persistence.Configgurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(e => e.Title)
            .IsRequired();
    }
}
