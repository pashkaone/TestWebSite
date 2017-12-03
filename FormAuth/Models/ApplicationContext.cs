using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using FormAuth.Models;

public class ApplicationContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationContext() : base("IdentityDb") { }

    public static ApplicationContext Create()
    {
        return new ApplicationContext();
    }

    public DbSet<Price> Prices { get; set; }

    public DbSet<Order> Orders { get; set; }
}