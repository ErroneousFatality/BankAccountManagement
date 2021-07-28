using Domain.Entities.ModerationActions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityFramework.EntityConfigurations
{
    public class ModerationActionConfiguration : IEntityTypeConfiguration<ModerationAction>
    {
        public void Configure(EntityTypeBuilder<ModerationAction> builder)
        {
            builder.Property(e => e.Reason)
                 .IsRequired()
                 .HasMaxLength(ModerationAction.ReasonMaxLength);
        }
    }
}
