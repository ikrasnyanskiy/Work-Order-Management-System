using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WorkOrderManagementSystem.Models
{
  // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
  public class ApplicationUser : IdentityUser
  {
    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
      // Add custom user claims here
      return userIdentity;
    }
  }

  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public ApplicationDbContext()
        : base("DefaultConnection", throwIfV1Schema: false)
    {
    }

    // Entity collections
    public DbSet<Mechanic> Mechanics { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Bicycle> Bicycles { get; set; }
    public DbSet<ServiceItem> ServiceItems { get; set; }
    public DbSet<WorkOrderLine> WorkOrderLines { get; set; }
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<RoleClaim> RoleClaims { get; set; }
    public DbSet<WorkLog> WorkLogs { get; set; }
    public DbSet<LogEntry> LogEntries { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceLine> InvoiceLines { get; set; }


    public static ApplicationDbContext Create()
    {
      return new ApplicationDbContext();
    }
    //TODO: figure out if delete cascades are an issue (we want to delete the bridge table when deleting a work order)
    // --fluent api configurations go here if necessary--
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      //NOTE differences in fluent API notations EF for mvc 5 uses "HasOptional" while EF for Asp.Net Core uses "HasOne"
      modelBuilder.Entity<WorkLog>().HasKey(w => new { w.MechanicId, w.WorkOrderId });
      modelBuilder.Entity<WorkLog>().HasRequired(w => w.Mechanic).WithMany(m => m.WorkLogs);//TODO: MUST ENFORCE THAT ONLY ONE MECHANIC CAN BE ACTIVE ON A WORKORDER!
      modelBuilder.Entity<WorkLog>().HasRequired(w => w.WorkOrder).WithMany(wo => wo.WorkLogs);//TODO: MUST ENFORCE all workLog.IsActiveOnOrder being set false when workorder set to completed


      //modelBuilder.Entity<WorkLog>().HasForeignKey(w=> new { w.MechanicId, w.WorkOrderId})HasKey(w => w.Mechanic.);//may just be able to use a nav property?

      modelBuilder.Entity<WorkOrder>().HasMany(w => w.WorkLogs).WithRequired(wl => wl.WorkOrder);//TODO: .WillCascadeOnDelete()?, also the withRequired is probably redundant


      modelBuilder.Entity<Mechanic>().HasMany(m => m.WorkLogs).WithRequired(wl => wl.Mechanic);

    }
  }
}