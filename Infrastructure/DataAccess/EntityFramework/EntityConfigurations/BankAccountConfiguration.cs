using Domain.Entities.BankAccounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityFramework.EntityConfigurations
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.Property(e => e.Number)
                 .IsRequired()
                 .HasMaxLength(BankAccount.NumberLength)
                 .IsFixedLength();

            builder.Property(e => e.Pin)
                 .IsRequired()
                 .HasMaxLength(BankAccount.PinLength)
                 .IsFixedLength();

            builder.HasIndex(e => new { e.UserId, e.BankId, e.Number })
                .IsUnique();
        }
    }
}
