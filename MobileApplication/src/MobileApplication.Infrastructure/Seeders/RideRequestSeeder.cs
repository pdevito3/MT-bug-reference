namespace MobileApplication.Infrastructure.Seeders
{

    using AutoBogus;
    using MobileApplication.Core.Entities;
    using MobileApplication.Infrastructure.Contexts;
    using System.Linq;

    public static class RideRequestSeeder
    {
        public static void SeedSampleRideRequestData(MobileApplicationDbContext context)
        {
            if (!context.RideRequests.Any())
            {
                context.RideRequests.Add(new AutoFaker<RideRequest>());
                context.RideRequests.Add(new AutoFaker<RideRequest>());
                context.RideRequests.Add(new AutoFaker<RideRequest>());

                context.SaveChanges();
            }
        }
    }
}