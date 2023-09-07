
using DockingAdminPanel.Data;
using DockingAdminPanel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DockingAdminPanel.Data;

public class BookingWebAppContext : IdentityDbContext<User>
{
    public BookingWebAppContext(DbContextOptions<BookingWebAppContext> options)
        : base(options)
    {
    }

    //public DbSet<BoatTypes> boatTypes { get; set; }

    //public DbSet<bookingTypes> bookingTypes { get; set; }

    public DbSet<Products> products { get; set; }
    public DbSet<User> users { get; set; }
    public DbSet<Doctor> doctors { get; set; }
    public DbSet<Patient> patients { get; set; }
    //public DbSet<Category> category { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
