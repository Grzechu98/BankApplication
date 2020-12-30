using BankApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.Data
{
    public class MainContext : DbContext
    {
        private readonly IConfiguration _config;

        public DbSet<UserModel> Users { get; set; }
        public DbSet<OperationModel> Operations { get; set; }
        public DbSet<BankAccountModel> BankAccounts { get; set; }
        public DbSet<AccountSettings> AccountSettings { get; set; }

        public MainContext(IConfiguration config)
        {
            _config = config; 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("Db"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasMany(um => um.Accounts).WithOne(ba=>ba.User).HasForeignKey(ba=>ba.UserId);
            modelBuilder.Entity<AccountSettings>().HasOne(a => a.BankAccount).WithOne(ba => ba.Settings).HasForeignKey<AccountSettings>(a => a.BankAccountId);
            modelBuilder.Entity<OperationModel>().HasOne(om => om.Recipient).WithMany(ba => ba.Incomings).HasForeignKey(om => om.RecipientId);
            modelBuilder.Entity<OperationModel>().HasOne(om => om.Sender).WithMany(ba => ba.Outgoings).HasForeignKey(om => om.SenderId);
        }
    }
}
