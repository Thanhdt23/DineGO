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
    public DbSet<Payment> payments { get; set; }
    public DbSet<RestaurantOwner> restaurantOwners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Define Primary Keys
        modelBuilder.Entity<Admin>().HasKey(a => a.ad_id);
        modelBuilder.Entity<Customer>().HasKey(c => c.cus_id);
        modelBuilder.Entity<Restaurant>().HasKey(r => r.res_id);
        modelBuilder.Entity<Category>().HasKey(c => c.cate_id);
        modelBuilder.Entity<Reservation>().HasKey(r => r.reser_id);
        modelBuilder.Entity<Notification>().HasKey(n => n.noti_id);
        modelBuilder.Entity<Blog>().HasKey(b => b.blog_id);
        modelBuilder.Entity<Payment>().HasKey(p => p.pay_id);
        modelBuilder.Entity<RestaurantOwner>().HasKey(ro => ro.resOwner_id);

        // Define Foreign Keys
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.customer)
            .WithMany(c => c.reservations)
            .HasForeignKey(r => r.cus_id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.restaurant)
            .WithMany(r => r.reservations)
            .HasForeignKey(r => r.res_id)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Notification>()
            .HasOne(n => n.customer)
            .WithMany(c => c.notifications)
            .HasForeignKey(n => n.cus_id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Blog>()
            .HasOne(b => b.restaurantOwner)
            .WithMany(r => r.blogs)
            .HasForeignKey(b => b.resOwner_id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Restaurant>()
            .HasOne(r => r.category)
            .WithMany(c => c.restaurants)
            .HasForeignKey(r => r.cate_id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Restaurant>()
            .HasOne(r => r.restaurantOwner)
            .WithMany(ro => ro.restaurants)
            .HasForeignKey(r => r.resOwner_id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RestaurantOwner>()
            .HasOne(ro => ro.customer)
            .WithMany(c => c.restaurantOwners)
            .HasForeignKey(ro => ro.cus_id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.customer)
            .WithMany(c => c.payments)
            .HasForeignKey(p => p.cus_id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.reservation)
            .WithMany(r => r.payments)
            .HasForeignKey(p => p.reser_id)
            .OnDelete(DeleteBehavior.NoAction);


        // Configure res_images as JSON column
        modelBuilder.Entity<Restaurant>()
            .Property(r => r.res_images)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),  // Convert List<string> to JSON
                v => JsonConvert.DeserializeObject<List<string>>(v)); // Convert JSON to List<string>
    }
}
