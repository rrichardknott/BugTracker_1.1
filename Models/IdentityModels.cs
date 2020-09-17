﻿using BugTracker_1._1.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Security.Policy;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker_1._1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        #region Parents/Children
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<TicketAttachment> Attachments { get; set; }
        public virtual ICollection<TicketComment> Comments { get; set; }
        public virtual ICollection<TicketHistory> Histories { get; set; }
        public virtual ICollection<TicketNotification> Notifications { get; set; }
        #endregion

        #region Actual Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string DisplayName { get; internal set; }
        public string AvatarPath { get; set; }
        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }
        }

        #endregion

        #region Constructior
        public ApplicationUser()
        {
            Projects = new HashSet<Project>();
            Attachments = new HashSet<TicketAttachment>();
            Histories = new HashSet<TicketHistory>();
            Notifications = new HashSet<TicketNotification>();
            Comments = new HashSet<TicketComment>();
        }
        #endregion


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }

        public System.Data.Entity.DbSet<BugTracker_1._1.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<BugTracker_1._1.Models.Ticket> Tickets { get; set; }

        public System.Data.Entity.DbSet<BugTracker_1._1.Models.TicketAttachment> TicketAttachments { get; set; }

        public System.Data.Entity.DbSet<BugTracker_1._1.Models.TicketComment> TicketComments { get; set; }

        public System.Data.Entity.DbSet<BugTracker_1._1.Models.TicketHistory> TicketHistories { get; set; }

        public System.Data.Entity.DbSet<BugTracker_1._1.Models.TicketNotification> TicketNotifications { get; set; }

        public System.Data.Entity.DbSet<BugTracker_1._1.Models.UserIndexViewModel> UserIndexViewModels { get; set; }
        public object Invitation { get; internal set; }
    }
}