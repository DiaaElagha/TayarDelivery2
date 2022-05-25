using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TayarDelivery.Entity.Domins;
using TayarDelivery.Entity.Domins.Base;
using TayarDelivery.Entity.Domins.Home;
using TayarDelivery.Entity.Domins.LookUp;
using TayarDelivery.Entity.Domins.Notification;
using TayarDelivery.Entity.Domins.User;

namespace TayarDelivery.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasOne(d => d.Area)
               .WithMany(p => p.ApplicationUser)
               .HasForeignKey(d => d.AreaId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ApplicationUser>().HasOne(d => d.UserType)
               .WithMany(p => p.ApplicationUser)
               .HasForeignKey(d => d.UserTypeID)
               .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<ApplicationUser>().HasOne(d => d.PriceType)
               .WithMany(p => p.ApplicationUser)
               .HasForeignKey(d => d.PriceTypeId)
               .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<AreasPrice>().HasKey(x => new { x.DealerAreaId, x.ReceverAreaId });

            modelBuilder.Entity<RoleLinks>().HasKey(x => new { x.RoleId, x.LinkId });

            modelBuilder.Entity<RolesUser>().HasKey(x => new { x.RoleId, x.UserId });

            base.OnModelCreating(modelBuilder);
        }

        #region LookUp
        public DbSet<Area> Area { set; get; }
        public DbSet<AreasPrice> AreasPrice { set; get; }
        public DbSet<CompanyInformation> CompanyInformation { set; get; }
        #endregion

        #region Notification
        public DbSet<MessageSMS> MessageSMS { set; get; }
        public DbSet<Notification> Notification { set; get; }
        #endregion

        #region Order
        public DbSet<Order> Order { set; get; }
        public DbSet<OrderStatus> OrderStatus { set; get; }
        public DbSet<OrderType> OrderType { set; get; }
        public DbSet<PriceType> PriceType { set; get; }
        public DbSet<BillTahsil> BillTahsil { set; get; }
        public DbSet<OrderHistory> OrderHistory { set; get; }
        public DbSet<OrderContent> OrderContent { set; get; }
        #endregion

        #region User
        public DbSet<UserProfile> UserProfile { set; get; }
        public DbSet<UserType> UserType { set; get; }

        public DbSet<Link> Link { set; get; }
        public DbSet<Role> Role { set; get; }
        public DbSet<RoleLinks> RoleLinks { set; get; }
        public DbSet<RolesUser> RolesUser { set; get; }
        #endregion

        #region Home
        public DbSet<RegisterTrader> RegisterTrader { set; get; }
        public DbSet<ContactUs> ContactUs { set; get; }
        public DbSet<HomeInfo> HomeInfo { set; get; }
        public DbSet<Services> Services { set; get; }
        #endregion

    }
}
