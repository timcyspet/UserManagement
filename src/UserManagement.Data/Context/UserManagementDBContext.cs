using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Model;

namespace UserManagement.Data.Context
{
    public class UserManagementDBContext : DbContext
    {
        public UserManagementDBContext(DbContextOptions optionsBuilder)
         : base(optionsBuilder)
        {
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
            //this.Database.CommandTimeout = 180;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get;set; }
        public DbSet<UserGroup> GroupUsers { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<PolicyRules> PolicyRules { get; set; }
        public DbSet<PolicyRoleMapping> PolicyRoleMappings { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            // Configure the database provider and connection string
             //optionsBuilder.UseSqlServer(System.Configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
