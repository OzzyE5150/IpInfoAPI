using Microsoft.EntityFrameworkCore;

namespace IpInfoAPI.Models
{
    public partial class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext() { }
        public DbContext(DbContextOptions<DbContext> options) : base() { }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<IpAddress> IpAddresses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost; Database=db; Trusted_Connection = True; Encrypt=False;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getutcdate())");

                entity.Property(e => e.Name)
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .IsFixedLength();

                entity.Property(e => e.ThreeLetterCode)
                   .HasMaxLength(3)
                   .IsUnicode(false)
                   .IsFixedLength();

                entity.Property(e => e.TwoLetterCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();
            });
            modelBuilder.Entity<IpAddress>(entity =>
            {
                entity.ToTable("IPAddresses");

                entity.HasIndex(e => e.Ip, "IX_IPAddresses").IsUnique();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Ip)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("IP");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

                entity.HasOne(o => o.Country)
                .WithMany(m => m.IpAddressCollection)
                .HasForeignKey(o => o.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IPAddresses_Countries");
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
