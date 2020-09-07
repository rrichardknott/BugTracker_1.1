namespace BugTracker_1._1.Migrations
{

    using BugTracker_1._1.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.CodeDom;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Drawing;
    using BugTracker_1._1.Helpers;
    using System.Web.Configuration;
    using Microsoft.Ajax.Utilities;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker_1._1.Models.ApplicationDbContext>
    {

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker_1._1.Models.ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userRolesHelper = new UserRolesHelper();
            var projectHelper = new ProjectHelper();
            var random = new Random();

            List<Ticket> ticketList = new List<Ticket>();
            List<ApplicationUser> projectManagers = new List<ApplicationUser>();
            List<ApplicationUser> developers = new List<ApplicationUser>();
            List<ApplicationUser> submitters = new List<ApplicationUser>();

            var adminList = userRolesHelper.UsersInRole("Admin");
            var developerList = userRolesHelper.UsersInRole("Developer");
            var submitterList = userRolesHelper.UsersInRole("Submitter");
            var projectManagerList = userRolesHelper.UsersInRole("Project Manager");

            projectManagers.AddRange(userRolesHelper.UsersInRole("Project Manager"));
            developers.AddRange(userRolesHelper.UsersInRole("Developer"));
            submitters.AddRange(userRolesHelper.UsersInRole("Submitter"));


            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            var RichardKnottEmail = WebConfigurationManager.AppSettings["RichardKnottEmail"];
            var RichardKnottPassword = WebConfigurationManager.AppSettings["RichardKnottPassword"];
            var AmyAdamsEmail = WebConfigurationManager.AppSettings["AmyAdamsEmail"];
            var AmyAdamsPassword = WebConfigurationManager.AppSettings["AmyAdamsPassword"];
            var BenjaminBrattEmail = WebConfigurationManager.AppSettings["BenjaminBrattEmail"];
            var BenjaminBrattPassword = WebConfigurationManager.AppSettings["BenjaminBrattPassword"];
            var CourtneyCoxEmail = WebConfigurationManager.AppSettings["CourtneyCoxEmail"];
            var CourtneyCoxPassword = WebConfigurationManager.AppSettings["CourtneyCoxPassword"];
            var DavidDuchovnyEmail = WebConfigurationManager.AppSettings["DavidDuchovnyEmail"];
            var DavidDuchovnyPassword = WebConfigurationManager.AppSettings["DavidDuchovnyPassword"];
            var EmilioEstevezEmail = WebConfigurationManager.AppSettings["EmilioEstevezEmail"];
            var EmilioEstevezPassword = WebConfigurationManager.AppSettings["EmilioEstevezPassword"];
            var FreddieFenderEmail = WebConfigurationManager.AppSettings["FreddieFenderEmail"];
            var FreddieFenderPassword = WebConfigurationManager.AppSettings["FreddieFenderPassword"];
            var GaryGulmanEmail = WebConfigurationManager.AppSettings["GaryGulmanEmail"];
            var GaryGulmanPassword = WebConfigurationManager.AppSettings["GaryGulmanPassword"];
            var HarryHamlinEmail = WebConfigurationManager.AppSettings["HarryHamlinEmail"];
            var HarryHamlinPassword = WebConfigurationManager.AppSettings["HarryHamlinPassword"];
            var AlanAldaEmail = WebConfigurationManager.AppSettings["AlanAldaEmail"];
            var AlanAldaPassword = WebConfigurationManager.AppSettings["AlanAldaPassword"];
            var BenjaminButtonEmail = WebConfigurationManager.AppSettings["BenjaminButtonEmail"];
            var BenjaminButtonPassword = WebConfigurationManager.AppSettings["BenjaminButtonPassword"];
            var ChevyChaseEmail = WebConfigurationManager.AppSettings["ChevyChaseEmail"];
            var ChevyChasePassword = WebConfigurationManager.AppSettings["ChevyChasePassword"];
            var DannyDeVitoEmail = WebConfigurationManager.AppSettings["DannyDeVitoEmail"];
            var DannyDeVitoPassword = WebConfigurationManager.AppSettings["DannyDeVitoPassword"];




            if (!context.Users.Any(u => u.Email == RichardKnottEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = RichardKnottEmail,
                    UserName = RichardKnottEmail,
                    FirstName = "Richard",
                    LastName = "Knott"
                }, RichardKnottPassword);


                var userId = userManager.FindByEmail(RichardKnottEmail).Id;
                userManager.AddToRole(userId, "Admin");
            }

            if (!context.Users.Any(u => u.Email == AmyAdamsEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = AmyAdamsEmail,
                    UserName = AmyAdamsEmail,
                    FirstName = "Amy",
                    LastName = "Adams"
                }, AmyAdamsPassword);


                var userId = userManager.FindByEmail(AmyAdamsEmail).Id;
                userManager.AddToRole(userId, "Admin");
            }

            if (!context.Users.Any(u => u.Email == (BenjaminBrattEmail)))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = BenjaminBrattEmail,
                    UserName = BenjaminBrattEmail,
                    FirstName = "Benjamin",
                    LastName = "Bratt"
                }, BenjaminBrattPassword);


                var userId = userManager.FindByEmail(BenjaminBrattEmail).Id;
                userManager.AddToRole(userId, "Project Manager");
            }

            if (!context.Users.Any(u => u.Email == CourtneyCoxEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = CourtneyCoxEmail,
                    UserName = CourtneyCoxEmail,
                    FirstName = "Courtney",
                    LastName = "Cox"
                }, CourtneyCoxPassword);


                var userId = userManager.FindByEmail(CourtneyCoxEmail).Id;
                userManager.AddToRole(userId, "Project Manager");
            }

            if (!context.Users.Any(u => u.Email == DavidDuchovnyEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = DavidDuchovnyEmail,
                    UserName = DavidDuchovnyEmail,
                    FirstName = "David",
                    LastName = "Duchovny"
                }, DavidDuchovnyPassword);


                var userId = userManager.FindByEmail(DavidDuchovnyEmail).Id;
                userManager.AddToRole(userId, "Developer");
            }

            if (!context.Users.Any(u => u.Email == EmilioEstevezEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = EmilioEstevezEmail,
                    UserName = EmilioEstevezEmail,
                    FirstName = "Emilio",
                    LastName = "Estevez"
                }, EmilioEstevezPassword);


                var userId = userManager.FindByEmail(EmilioEstevezEmail).Id;
                userManager.AddToRole(userId, "Developer");
            }

            if (!context.Users.Any(u => u.Email == FreddieFenderEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = FreddieFenderEmail,
                    UserName = FreddieFenderEmail,
                    FirstName = "Freddie",
                    LastName = "Fender"
                }, FreddieFenderPassword);

                var userId = userManager.FindByEmail(FreddieFenderEmail).Id;
                userManager.AddToRole(userId, "Submitter");
            }

            if (!context.Users.Any(u => u.Email == GaryGulmanEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = GaryGulmanEmail,
                    UserName = GaryGulmanEmail,
                    FirstName = "Gary",
                    LastName = "Gulman"
                }, GaryGulmanPassword);


                var userId = userManager.FindByEmail(GaryGulmanEmail).Id;
                userManager.AddToRole(userId, "Submitter");
            }

            if (!context.Users.Any(u => u.Email == HarryHamlinEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = HarryHamlinEmail,
                    UserName = HarryHamlinEmail,
                    FirstName = "Harry",
                    LastName = "Hamlin"
                }, HarryHamlinPassword);


                var userId = userManager.FindByEmail(HarryHamlinEmail).Id;
                userManager.AddToRole(userId, "Submitter");
            }


            //For Demo Version
            if (!context.Users.Any(u => u.Email == AlanAldaEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = AlanAldaEmail,
                    UserName = AlanAldaEmail,
                    FirstName = "Alan",
                    LastName = "Alda"
                }, AlanAldaPassword);
                var userId = userManager.FindByEmail(AlanAldaEmail).Id;
                userManager.AddToRole(userId, "Admin");
            }

            if (!context.Users.Any(u => u.Email == BenjaminButtonEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = BenjaminButtonEmail,
                    UserName = BenjaminButtonEmail,
                    FirstName = "Benjamin",
                    LastName = "Button"
                }, BenjaminButtonPassword);
                var userId = userManager.FindByEmail(BenjaminButtonEmail).Id;
                userManager.AddToRole(userId, "Project Manager");
            }

            if (!context.Users.Any(u => u.Email == ChevyChaseEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = ChevyChaseEmail,
                    UserName = ChevyChaseEmail,
                    FirstName = "Chevy",
                    LastName = "Chase"
                }, ChevyChasePassword);
                var userId = userManager.FindByEmail(ChevyChaseEmail).Id;
                userManager.AddToRole(userId, "Developer");
            }

            if (!context.Users.Any(u => u.Email == DannyDeVitoEmail))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = DannyDeVitoEmail,
                    UserName = DannyDeVitoEmail,
                    FirstName = "Danny",
                    LastName = "DeVito"
                }, DannyDeVitoPassword);
                var userId = userManager.FindByEmail(DannyDeVitoEmail).Id;
                userManager.AddToRole(userId, "Submitter");
            }


            context.SaveChanges();

            //Ticket Type Seed
            context.TicketTypes.AddOrUpdate(
                tt => tt.Name,
                new TicketType() { Name = "Software" },
                new TicketType() { Name = "Hardware" },
                new TicketType() { Name = "UI" },
                new TicketType() { Name = "Defect" },
                new TicketType() { Name = "Other" }
                );

            //Ticket Priority Seed
            context.TicketPriorities.AddOrUpdate(
                tp => tp.Name,
                new TicketPriority() { Name = "Low" },
                new TicketPriority() { Name = "Medium" },
                new TicketPriority() { Name = "High" },
                new TicketPriority() { Name = "On Hold" }
                );

            //Ticket Status Seed
            context.TicketStatuses.AddOrUpdate(
                ts => ts.Name,
                new TicketStatus() { Name = "Open" },
                new TicketStatus() { Name = "Assigned" },
                new TicketStatus() { Name = "Resolved" },
                new TicketStatus() { Name = "Reopened" },
                new TicketStatus() { Name = "Archived" }
                );

            //Project Seed
            context.Projects.AddOrUpdate(
                p => p.Name,
                new Project() { Name = "Veteran's Park Community Event Registration Software", Created = DateTime.Now.AddDays(-60), IsArchived = true, Description = "Orange County needs a professional and robust new events scheduling and registration software program." },
                new Project() { Name = "Circus Vargas Employee Leave Tracker", Created = DateTime.Now.AddDays(-45), Description = "Circus Vargas contacted us to create an employee leave tracking program for FMLA, PDL, CFRA leave." },
                new Project() { Name = "City of Lights New Hire On-Boarding Software", Created = DateTime.Now.AddDays(-30), Description = "City of Lights Human Resources Department needs a new HR program for their newly hired employees." },
                new Project() { Name = "Schools First Credit Union Financial Software", Created = DateTime.Now.AddDays(-15), Description = "Schools First is looking to purchase our Financial Portal software, but is asking us to customize." },
                new Project() { Name = "UrTurn Bidding Software", Created = DateTime.Now.AddDays(-7), Description = "UrTurn start-up wants us to make a bidding software program." },
                new Project() { Name = "Doublz Deli Register Software", Created = DateTime.Now.AddDays(-45), Description = "Doublz Deli purchased our Restaurant Cashier software." },
                 new Project() { Name = "True Movies Streaming Service", Created = DateTime.Now.AddDays(-5), Description = "True Movies contracted with us to create an updatd online streaming portal." },
                  new Project() { Name = "Pets Bin Dog Grooming Client Software", Created = DateTime.Now.AddDays(-15), Description = "Pets Bin purchased our new floor-to-ceiling client software." },
                   new Project() { Name = "Syndicate Attorneys, LLP", Created = DateTime.Now, Description = "Syndicate Attorneys contracted with us to create a Client-Conflict program to avoid representing opposing parties." },
                    new Project() { Name = "Tricycle Club Casino", Created = DateTime.Now.AddDays(-2), Description = "The Tricycle Club purchased our Human Resources On-Boarding software." }

                );
            context.SaveChanges();

            // Assign users to projects by Role
            //foreach (var project in context.Projects.ToList())
            //{
            //    foreach (var user in userRolesHelper.UsersInRole("Admin"))
            //    {
            //        projectHelper.AddUserToProject(user.Id, project.Id);
            //    }
            //    projectHelper.AddUserToProject(projectManagers[random.Next(projectManagers.Count -1)].Id, project.Id);

            //    // Assign Developers
            //    var firstDeveloper = developers[random.Next(developers.Count)].Id;
            //    var secondDeveloper = developers[random.Next(developers.Count)].Id;
            //    while (firstDeveloper == secondDeveloper)
            //    {
            //        secondDeveloper = developers[random.Next(developers.Count)].Id;
            //    }
            //    projectHelper.AddUserToProject(firstDeveloper, project.Id);
            //    projectHelper.AddUserToProject(secondDeveloper, project.Id);

            //    //Assign Submitters
            //    var firstSubmitter = submitters[random.Next(submitters.Count)].Id;
            //    var secondSubmitter = submitters[random.Next(submitters.Count)].Id;
            //    while (firstSubmitter == secondSubmitter)
            //    {
            //        secondSubmitter = submitters[random.Next(submitters.Count)].Id;
            //    }
            //    projectHelper.AddUserToProject(firstSubmitter, project.Id);
            //    projectHelper.AddUserToProject(secondSubmitter, project.Id);
            //}

            // Creation of assignments to projects and tickets

            //foreach (var project in context.Projects.ToList())
            //{
            //    projectManagers = projectHelper.ListUsersOnProjectInRole(project.Id, "ProjectManager");
            //    developers = projectHelper.ListUsersOnProjectInRole(project.Id, "Developer");
            //    submitters = projectHelper.ListUsersOnProjectInRole(project.Id, "Submitter");

            //    foreach (var type in context.TicketTypes.ToList())
            //    {
            //        foreach (var status in context.TicketStatuses.ToList())
            //        {
            //            foreach (var priority in context.TicketPriorities.ToList())
            //            {

            //                var developerId = developers[random.Next(developers.Count)].Id;
            //                if (status.Name == "Open")
            //                {
            //                    developerId = null;
            //                }
            //                var resolved = false;
            //                var archived = false;
            //                if (status.Name == "Resolved")
            //                {
            //                    resolved = true;
            //                }
            //                if (status.Name == "Archived" || project.IsArchived == true)
            //                {
            //                    archived = true;
            //                }
            //                var newTicket = new Ticket()
            //                {
            //                    ProjectId = project.Id,
            //                    TicketPriorityId = priority.Id,
            //                    TicketTypeId = type.Id,
            //                    TicketStatusId = status.Id,
            //                    SubmitterId = submitters[random.Next(submitters.Count)].Id,
            //                    DeveloperId = developerId,
            //                    Created = DateTime.Now,
            //                    Issue = $"Ticket type:{type.Name}. Status:{status.Name}. Priority:{priority.Name}",
            //                    IssueDescription = $"This is a {type.Name} type of ticket for Project:{project.Name}.",
            //                    IsResolved = resolved,
            //                    IsArchived = archived
            //                };
            //                ticketList.Add(newTicket);
            //            }
            //        }
            //    }
            //}

            //context.Tickets.AddRange(ticketList);
            //context.SaveChanges();
            // end of creation of assignment of projects to projects and tickets

            var userList = context.Users.ToList();
            var projectList = context.Projects.ToList();
            foreach (var project in projectList)
            {
                foreach (var user in userList)
                {
                    projectHelper.AddUserToProject(user.Id, project.Id);
                }
            }


        }
    }
}

