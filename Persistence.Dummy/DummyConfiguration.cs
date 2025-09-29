using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Dummy;

public class DummyConfiguration : IEntityTypeConfiguration<Dummy>
{
    public void Configure(
        EntityTypeBuilder<Dummy> builder)
    {
        builder.HasKey(_ => _.ID);
    }
}
