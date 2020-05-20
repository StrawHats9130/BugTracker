using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
             

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string AvatarPath { get; set; }


        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }
        }

        public virtual ICollection<Project> Projects { get; set; }
              
        public virtual ICollection<TicketComment> Comments { get; set; }
        public virtual ICollection<TicketAttachment> Attchments { get; set; }
      
        public virtual ICollection<TicketHistory> Histories { get; set; }

        public ApplicationUser()
        {
            Projects = new HashSet<Project>();
            Comments = new HashSet<TicketComment>();
            Attchments = new HashSet<TicketAttachment>();
            Histories = new HashSet<TicketHistory>();
        }

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

        public System.Data.Entity.DbSet<BugTracker.Models.Ticket> Tickets { get; set; }

       

        public System.Data.Entity.DbSet<BugTracker.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<BugTracker.Models.TicketAttachment> TicketAttachments { get; set; }

        public System.Data.Entity.DbSet<BugTracker.Models.TicketComment> TicketComments { get; set; }

        public System.Data.Entity.DbSet<BugTracker.Models.TicketHistory> TicketHistories { get; set; }

        public System.Data.Entity.DbSet<BugTracker.Models.TicketNotification> TicketNotifications { get; set; }

        public System.Data.Entity.DbSet<BugTracker.Models.TicketPriority> TicketPriorities { get; set; }

        public System.Data.Entity.DbSet<BugTracker.Models.TicketStatus> TicketStatus { get; set; }

        public System.Data.Entity.DbSet<BugTracker.Models.TicketType> TicketTypes { get; set; }

        public System.Data.Entity.DbSet<BugTracker.ViewModel.UserProfileViewModel> UserProfileViewModels { get; set; }
    }
}