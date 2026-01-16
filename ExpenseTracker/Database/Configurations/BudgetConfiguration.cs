using ExpenseTracker.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Api.Database.Configurations
{
    public sealed class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.HasKey(b =>  b.Id);
            builder.Property(b => b.Id)
                   .HasMaxLength(50);

            builder.Property(b => b.UserId)
                   .HasMaxLength(450)
                   .IsRequired();

            builder.Property(b => b.CategoryId)
                   .HasMaxLength(50)
                   .IsRequired();

            //в валидации проверять год и месяц
            builder.Property(b => b.Month)
                   .IsRequired();

            builder.Property(b => b.Year)
                   .IsRequired();

            builder.Property(b => b.Amount)
                   .IsRequired();

            builder.HasOne(b => b.User)
                   .WithMany(u => u.Budgets)
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Category)
                   .WithMany()
                   .HasForeignKey(b => b.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
