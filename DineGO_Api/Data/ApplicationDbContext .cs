using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using DineGO_Api.Model;
using System.Collections.Generic;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Admin> admins { get; set; }
    public DbSet<Customer> customers { get; set; }
    public DbSet<Restaurant> restaurants { get; set; }
    public DbSet<Category> categories { get; set; }
    public DbSet<Reservation> reservations { get; set; }
    public DbSet<Notification> notifications { get; set; }
    public DbSet<Blog> blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Define Primary Keys
        modelBuilder.Entity<Admin>().HasKey(a => a.ad_id);
        modelBuilder.Entity<Customer>().HasKey(c => c.cus_id);
        modelBuilder.Entity<Restaurant>().HasKey(r => r.res_id);
        modelBuilder.Entity<Category>().HasKey(c => c.cate_id);
        modelBuilder.Entity<Reservation>().HasKey(r => r.re_id);
        modelBuilder.Entity<Notification>().HasKey(n => n.noti_id);
        modelBuilder.Entity<Blog>().HasKey(b => b.blog_id);

        // Define Foreign Keys
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.customer)
            .WithMany()
            .HasForeignKey(r => r.cus_id);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.restaurant)
            .WithMany()
            .HasForeignKey(r => r.res_id);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.customer)
            .WithMany(c => c.notifications)
            .HasForeignKey(n => n.cus_id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.reservation)
            .WithMany(r => r.notifications)
            .HasForeignKey(n => n.re_id)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Blog>()
            .HasOne(b => b.restaurant)
            .WithMany()
            .HasForeignKey(b => b.res_id);

        modelBuilder.Entity<Restaurant>()
            .HasOne(r => r.category)
            .WithMany(c => c.restaurants)
            .HasForeignKey(r => r.cate_id)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure res_images as JSON column
        modelBuilder.Entity<Restaurant>()
            .Property(r => r.res_images)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),  // Convert List<string> to JSON
                v => JsonConvert.DeserializeObject<List<string>>(v) ?? new List<string>() // Convert JSON back to List<string>
            );
    }
}
