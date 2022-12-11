using System;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReferralCodeAPI.Config;

namespace ReferralCodeAPI
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySQL(ConfigDatabase.conStr);
        }

        public DbSet<ReferralCode> referralCodes {get;set;}
    }
}