using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Api.Models
{
    public class UserRegistration
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class UserLogin 
    { 
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }


    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        private readonly IConfiguration _configuration;

        public AppIdentityDbContext(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Identity"));
            //base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>().Property(x => x.FirstName).HasMaxLength(100);
            builder.Entity<AppUser>().Property(x => x.LastName).HasMaxLength(100);
            builder.Entity<AppUser>().Property(x => x.AvatarUrl);
        }
    }
}
