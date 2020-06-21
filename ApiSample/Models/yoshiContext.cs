using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiSample.Models
{
    public partial class yoshiContext : DbContext
    {
        public yoshiContext()
        {
        }

        public yoshiContext(DbContextOptions<yoshiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Gosc> Gosc { get; set; }
        public virtual DbSet<Kategoria> Kategoria { get; set; }
        public virtual DbSet<Pokoj> Pokoj { get; set; }
        public virtual DbSet<Rezerwacja> Rezerwacja { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=db-mssql;Initial Catalog=yoshi;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gosc>(entity =>
            {
                entity.HasKey(e => e.IdGosc)
                    .HasName("PK__Gosc__8126AB6DC54B166F");

                entity.Property(e => e.IdGosc).ValueGeneratedNever();

                entity.Property(e => e.Imie)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Nazwisko)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ProcentRabatu).HasColumnName("Procent_rabatu");
            });

            modelBuilder.Entity<Kategoria>(entity =>
            {
                entity.HasKey(e => e.IdKategoria)
                    .HasName("PK__Kategori__31412B26CA098E64");

                entity.Property(e => e.IdKategoria).ValueGeneratedNever();

                entity.Property(e => e.Cena).HasColumnType("numeric(8, 2)");

                entity.Property(e => e.Nazwa)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pokoj>(entity =>
            {
                entity.HasKey(e => e.NrPokoju)
                    .HasName("PK__Pokoj__18804ABE7E490587");

                entity.Property(e => e.NrPokoju).ValueGeneratedNever();

                entity.Property(e => e.LiczbaMiejsc).HasColumnName("Liczba_miejsc");

                entity.HasOne(d => d.IdKategoriaNavigation)
                    .WithMany(p => p.Pokoj)
                    .HasForeignKey(d => d.IdKategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Pokoj__IdKategor__4D94879B");
            });

            modelBuilder.Entity<Rezerwacja>(entity =>
            {
                entity.HasKey(e => e.IdRezerwacja)
                    .HasName("PK__Rezerwac__68F5E1863EF04854");

                entity.Property(e => e.IdRezerwacja).ValueGeneratedNever();

                entity.Property(e => e.DataDo).HasColumnType("datetime");

                entity.Property(e => e.DataOd).HasColumnType("datetime");

                entity.HasOne(d => d.IdGoscNavigation)
                    .WithMany(p => p.Rezerwacja)
                    .HasForeignKey(d => d.IdGosc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Rezerwacj__IdGos__5070F446");

                entity.HasOne(d => d.NrPokojuNavigation)
                    .WithMany(p => p.Rezerwacja)
                    .HasForeignKey(d => d.NrPokoju)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Rezerwacj__NrPok__5165187F");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
