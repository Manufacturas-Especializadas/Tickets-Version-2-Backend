using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Status> Statuses => Set<Status>();

        public DbSet<Classification> Classifications => Set<Classification>();

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<User> Users => Set<User>();

        public DbSet<Ticket> Tickets => Set<Ticket>();

        public DbSet<TicketAttachment> TicketAttachments => Set<TicketAttachment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("TkCategory");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(70).IsRequired();
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("TkStatus");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Classification>(entity =>
            {
                entity.ToTable("TkClassification");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);
                entity.Property(e => e.PayRollNumber).HasColumnName("payRollNumber").IsRequired();
                entity.Property(e => e.RolId).HasColumnName("rolId").IsRequired();
                entity.Property(e => e.PasswordHash).HasColumnName("passwordHash").HasMaxLength(256).IsRequired();
                entity.Property(e => e.RefreshToken).HasColumnName("refreshToken").HasMaxLength(256);
                entity.Property(e => e.RefreshTokenExpiryTime).HasColumnName("refreshTokenExpiryTime");

                entity.HasIndex(e => new { e.Name, e.PayRollNumber })
                      .IsUnique()
                      .HasDatabaseName("UQ_Users_Name_PayRollNumber");

                entity.HasOne(e => e.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(e => e.RolId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Tickets");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");
                entity.Property(e => e.StatusId).HasColumnName("statusId");
                entity.Property(e => e.UserId).HasColumnName("userId");
                entity.Property(e => e.ClassificationId).HasColumnName("classificationId");

                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(70).IsRequired();
                entity.Property(e => e.Department).HasColumnName("department").HasMaxLength(60);
                entity.Property(e => e.Affair).HasColumnName("affair").HasMaxLength(80);
                entity.Property(e => e.ProblemDescription).HasColumnName("problemDescription").HasMaxLength(300);
                entity.Property(e => e.Solution).HasColumnName("solution");

                entity.Property(e => e.RegistrationDate)
                      .HasColumnName("registrationDate")
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ResolutionDate).HasColumnName("resolutionDate");

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Tickets)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Status)
                      .WithMany(s => s.Tickets)
                      .HasForeignKey(e => e.StatusId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Tickets)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Classification)
                      .WithMany(c => c.Tickets)
                      .HasForeignKey(e => e.ClassificationId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<TicketAttachment>(entity =>
            {
                entity.ToTable("TicketAttachments");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TicketId).HasColumnName("ticketId").IsRequired();
                entity.Property(e => e.FileName).HasColumnName("fileName").HasMaxLength(255);
                entity.Property(e => e.FileUrl).HasColumnName("fileUrl").HasMaxLength(2048).IsRequired();

                entity.Property(e => e.UploadedDate)
                      .HasColumnName("uploadedDate")
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Ticket)
                      .WithMany(t => t.Attachments)
                      .HasForeignKey(e => e.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}