using FinanceSystem.Domain.Bills;
using FinanceSystem.Domain.Users;

namespace FinanceSystem.Persistance.Mappings;

public class BillMapping() : EntityBaseMap<Bill, long>("Bills")
{
    override protected void ConfigureMap(EntityTypeBuilder<Bill> builder)
    {
        builder.HasKey(b => b.Id);

        builder
            .Property(b => b.Id)
            .HasColumnName("Id")
            .UseHiLo("bill_hilo_seq");

        builder
            .Property(b => b.ServiceId)
            .HasColumnName("ServiceId");

        builder
            .Property(b => b.ReferenceId)
            .HasColumnName("ReferenceId");

        builder
            .Property(b => b.Amount)
            .HasColumnName("Amount");

        builder
            .Property(b => b.UserId)
            .HasColumnName("UserId");

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Bills_Users_UserId");

        builder.Ignore(b => b.Publisher);
    }
}
