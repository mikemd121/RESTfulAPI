using Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure
{
    public class CoreWebAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public CoreWebAppDbContext(DbContextOptions<CoreWebAppDbContext> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = true;
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Insurance>().HasNoKey();
            builder.Entity<CPMappings>().HasNoKey();
            builder.Entity<ARAPJDE>().HasNoKey();

            //builder.Entity<ARAPJDE>().HasData(
            //     new ARAPJDE { ACCode = 1, Description = "Oscar Montenegro", SupplierCode = "My first Post", SupplierName = "Hello world, this is my first post", contracNo = "Hello world, this is my first post", DueDate = "Hello world, this is my first post", AmountInCtrm_USD = "Hello world, this is my first post", AmountInJDE = "Hello world, this is my first post" },
            //    );
            //builder.Entity<CPMappings>().HasData();
            //builder.Entity<Insurance>().HasData();

            base.OnModelCreating(builder);
        }
        public virtual DbSet<CPMappings> CPMappings { get; set; }
        public virtual DbSet<ARAPJDE> ARAPJDEs { get; set; }
        public virtual DbSet<Insurance> Insurances { get; set; }

    }
}
