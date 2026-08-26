using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KASHOP.DAL;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .HasOne(product => product.CreatedBy)
            .WithMany()
            .HasForeignKey(product => product.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(product => product.UpdatedBy)
            .WithMany()
            .HasForeignKey(product => product.UpdatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
