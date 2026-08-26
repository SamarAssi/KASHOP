using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KASHOP.DAL;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasOne(category => category.CreatedBy) // A Category has one CreatedBy user
            .WithMany() // One User can be associated with many Categories
            .HasForeignKey(category => category.CreatedById) // The foreign key that connects Category to User
            .OnDelete(DeleteBehavior.Restrict); // A User cannot be deleted while Categories reference that User

        builder
            .HasOne(category => category.UpdatedBy)
            .WithMany()
            .HasForeignKey(category => category.UpdatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
