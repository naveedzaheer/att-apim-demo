using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MoviesREST.Models
{
    public partial class FilmyContext : DbContext
    {
        public virtual DbSet<Movie> Movies { get; set; }

        public FilmyContext(DbContextOptions<FilmyContext> options) : base(options)
        { }
        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlServer(@"Server=.\sqlexpress;Database=Filmy;Trusted_Connection=True;");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.FilmId);

                entity.Property(e => e.FilmId).HasColumnName("FilmID");

                entity.Property(e => e.Certificate).HasMaxLength(255);

                entity.Property(e => e.CountryName).HasMaxLength(255);

                entity.Property(e => e.DirectorName).HasMaxLength(255);

                entity.Property(e => e.FilmName).HasMaxLength(255);

                entity.Property(e => e.FilmReleaseDate).HasColumnType("datetime2(3)");

                entity.Property(e => e.FilmSynopsis).HasColumnType("ntext");

                entity.Property(e => e.Language).HasMaxLength(255);

                entity.Property(e => e.StudioName).HasMaxLength(255);
            });
        }
    }
}
