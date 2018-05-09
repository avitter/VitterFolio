using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VitterFolio.DataServices.Models
{
    public partial class VitterFolioContext : DbContext
    {
        public virtual DbSet<Asset> Asset { get; set; }

        public VitterFolioContext(DbContextOptions<VitterFolioContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol).HasColumnType("char(5)");
            });
        }
    }
}
