using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityFramework.EntityConfigurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.UniqueMasterCitizenNumber)
                .IsRequired()
                .HasMaxLength(User.UniqueMasterCitizenNumberLength)
                .IsFixedLength();

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(User.FirstNameMaxLength);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(User.LastNameMaxLength);

            builder.HasMany(user => user.WalletTransferDepositTransactions)
                .WithOne(walletTransferTransaction => walletTransferTransaction.DestinationUser)
                .HasForeignKey(walletTransferTransaction => walletTransferTransaction.DestinationUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
