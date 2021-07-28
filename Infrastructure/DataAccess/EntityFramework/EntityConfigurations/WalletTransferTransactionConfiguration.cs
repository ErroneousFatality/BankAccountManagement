using Domain.Entities.WalletTransferTransactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityFramework.EntityConfigurations
{
    internal class WalletTransferTransactionConfiguration : IEntityTypeConfiguration<WalletTransferTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransferTransaction> builder)
        {
            builder.Property(p => p.Reason)
                .IsRequired(false)
                .HasMaxLength(WalletTransferTransaction.ReasonMaxLength);
        }
    }
}
