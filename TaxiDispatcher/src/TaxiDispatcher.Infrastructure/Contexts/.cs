namespace TaxiDispatcher.Infrastructure.Contexts
{
    using TaxiDispatcher.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Threading;
    using System.Threading.Tasks;

    public class  : DbContext
    {
        public (
            DbContextOptions<> options) : base(options)
        {
        }

        #region DbSet Region - Do Not Delete


        #endregion DbSet Region - Do Not Delete



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}