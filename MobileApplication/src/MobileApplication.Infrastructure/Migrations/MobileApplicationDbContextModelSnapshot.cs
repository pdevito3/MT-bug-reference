// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MobileApplication.Infrastructure.Contexts;

namespace MobileApplication.Infrastructure.Migrations
{
    [DbContext(typeof(MobileApplicationDbContext))]
    partial class MobileApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("MobileApplication.Core.Entities.RideRequest", b =>
                {
                    b.Property<Guid>("RideRequestId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsEco")
                        .HasColumnType("bit");

                    b.Property<string>("RideType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RideRequestId");

                    b.ToTable("RideRequest");
                });
#pragma warning restore 612, 618
        }
    }
}
