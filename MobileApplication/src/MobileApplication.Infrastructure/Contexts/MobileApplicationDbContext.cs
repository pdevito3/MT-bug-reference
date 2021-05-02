namespace MobileApplication.Infrastructure.Contexts
{
    using MobileApplication.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Threading;
    using System.Threading.Tasks;

    public class MobileApplicationDbContext : DbContext
    {
        public MobileApplicationDbContext(
            DbContextOptions<MobileApplicationDbContext> options) : base(options)
        {
        }

        #region DbSet Region - Do Not Delete

        public DbSet<RideRequest> RideRequests { get; set; }
        #endregion DbSet Region - Do Not Delete



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RideRequest>().Property(p => p.RideRequestId).ValueGeneratedNever();
        }
    }
}