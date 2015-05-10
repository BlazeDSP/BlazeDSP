﻿namespace Frontend.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Blaze.DSP.Library.Constants;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Azure;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            return userIdentity;
        }

        [Required]
        public bool AuthenticatorEnabled { get; set; }
        [MaxLength(64)]
        public string AuthenticatorSecrete { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private static readonly string DefaultConnection = CloudConfigurationManager.GetSetting(DatabaseStrings.DatabaseConnectionStringName);

        public ApplicationDbContext()
        {
            // NOTE: For migrations
        }

        public ApplicationDbContext(string defaultConnection) : base(defaultConnection, false)
        {
            // LL kills Azure SQL performance
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext(DefaultConnection);
        }
    }
}