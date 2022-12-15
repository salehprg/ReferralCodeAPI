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
            optionsBuilder.UseNpgsql(ConfigDatabase.conStr);
        }

        public DbSet<ReferralCode> referralCodes {get;set;}
        public DbSet<ScoreBoard> scoreBoards {get;set;}
    }
}