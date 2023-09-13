using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace aspnetapp
{
    public partial class CounterContext : DbContext
    {
        public CounterContext()
        {
        }
        public DbSet<Counter> Counters { get; set; } = null!;
        public DbSet<Data_Day> Data_Day { get; set; } = null!;
        public DbSet<Data_Month> Data_Month { get; set; } = null!;
        public DbSet<Data_OutHourly> Data_OutHourly { get; set; } = null!;
        public DbSet<Data_7Day> Data_7Day { get; set; } = null!;
        public DbSet<Data_Inv> Data_Inv { get; set; } = null!;
        public DbSet<Data_InOutHourly> Data_InOutHourly { get; set; } = null!;
        public DbSet<Data_InOut> Data_InOut { get; set; } = null!;

        public DbSet<Menus> Menus { get; set; } = null!;
        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<UserMenus> UserMenus { get; set; } = null!;

        public CounterContext(DbContextOptions<CounterContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var username = Environment.GetEnvironmentVariable("MYSQL_USERNAME");
                var password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
                var addressParts = Environment.GetEnvironmentVariable("MYSQL_ADDRESS")?.Split(':');
                var host = addressParts?[0];
                var port = addressParts?[1];
                var connstr = $"server={host};port={port};user={username};password={password};database=aspnet_demo";
                optionsBuilder.UseMySql(connstr, Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.18-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            modelBuilder.Entity<Counter>().ToTable("Counters");
            modelBuilder.Entity<Data_Day>().ToTable("Data_Day");
            modelBuilder.Entity<Data_Month>().ToTable("Data_Month");
            modelBuilder.Entity<Data_OutHourly>().ToTable("Data_OutHourly");
            modelBuilder.Entity<Data_7Day>().ToTable("Data_7Day");
            modelBuilder.Entity<Data_Inv>().ToTable("Data_Inv");
            modelBuilder.Entity<Data_InOutHourly>().ToTable("Data_InOutHourly");
            modelBuilder.Entity<Data_InOut>().ToTable("Data_InOut");

            modelBuilder.Entity<Menus>().ToTable("Menus");
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<UserMenus>().ToTable("UserMenus");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
