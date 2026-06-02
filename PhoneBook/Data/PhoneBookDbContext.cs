using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PhoneBook.Data;

public partial class PhoneBookDbContext : DbContext
{
    public PhoneBookDbContext()
    {
    }

    public PhoneBookDbContext(DbContextOptions<PhoneBookDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ContactEntity> Contacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=PhoneBookDB;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contacts__3214EC07206E0DB5");

            entity.HasIndex(e => e.Phone, "UQ__Contacts__5C7E359E7CAC01B9").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
