namespace BugTracker_1._1.Migrations{
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

            var DemoAdminPassword = WebConfigurationManager.AppSettings["DemoAdminPassword"];

            var DemoProjectManagerPassword = WebConfigurationManager.AppSettings["DemoProjectManagerPassword"];
            var DemoProjectManagerPassword1 = WebConfigurationManager.AppSettings["DemoProjectManagerPassword1"];


            var DemoDeveloperPassword = WebConfigurationManager.AppSettings["DemoDeveloperPassword"];
            var DemoDeveloperPassword1 = WebConfigurationManager.AppSettings["DemoDeveloperPassword1"];


            var DemoSubmitterPassword = WebConfigurationManager.AppSettings["DemoSubmitterPassword"];
            var DemoSubmitterPassword1 = WebConfigurationManager.AppSettings["DemoSubmitterPassword1"];
            var DemoSubmitterPassword2 = WebConfigurationManager.AppSettings["DemoSubmitterPassword2"];

            var MyPassword = WebConfigurationManager.AppSettings["MyPassword"];
            

            //if (!context.Users.Any(u => u.Email == "RichardKnott1970@gmail.com"))
            //{
            //    userManager.Create(new ApplicationUser()
            //    {
            //        Email = "RichardKnott1970@gmail.com",
            //        UserName = "RichardKnott1970@gmail.com",
            //        FirstName = "Richard",
            //        LastName = "Knott"
            //    }, MyPassword);

               
            //    var userId = userManager.FindByEmail("Richardknott1970@gmail.com").Id;               
            //    userManager.AddToRole(userId, "Admin");
            //}
            
            //if (!context.Users.Any(u => u.Email == "pmjasontwichell@CoderFoundry.com"))
            //{
            //    userManager.Create(new ApplicationUser()
            //    {
            //        Email = "pmjasontwichell@CoderFoundry.com",
            //        UserName = "pmjasontwichell@CoderFoundry.com",
            //        FirstName = "Jason",
            //        LastName = "Twichell",
            //    }, "Abc@123!");

                
            //    var userId = userManager.FindByEmail("pmjasontwichell@CoderFoundry.com").Id;          
            //    userManager.AddToRole(userId, "Project Manager");
            //}

            //if (!context.Users.Any(u => u.Email == "pmarussell@coderfoundry.com"))
            //{
            //    userManager.Create(new ApplicationUser()
            //    {
            //        Email = "pmarussell@coderfoundry.com",
            //        UserName = "pmarussell@coderfoundry.com",
            //        FirstName = "Drew",
            //        LastName = "Russell",
            //    }, "Abc@123!");

                
            //    var userId = userManager.FindByEmail("pmarussell@CoderFoundry.com").Id;               
            //    userManager.AddToRole(userId, "Project Manager");
            //}

            //if (!context.Users.Any(u => u.Email == "projectmanager@mailinator.com"))
            //{
            //    userManager.Create(new ApplicationUser()
            //    {
            //        Email = "projectmanager@mailinator.com",
            //        UserName = "projectmanager@mailinator.com",
            //        FirstName = "Project",
            //        LastName = "Manager",
            //    }, "Abc@123!");
            //    var userId = userManager.FindByEmail("projectmanager@mailinator.com").Id;
            //    userManager.AddToRole(userId, "Project Manager");
            //}

            //if (!context.Users.Any(u => u.Email == "devjohndoe@mailinator.com"))
            //{
            //    userManager.Create(new ApplicationUser()
            //    {
            //        Email = "devjohndoe@mailinator.com",
            //        UserName = "devjohndoe@mailinator.com",
            //        FirstName = "John",
            //        LastName = "Doe",
            //    }, "Abc@123!");

            //    //Get the ID that was just created by adding the above user
            //    var userId = userManager.FindByEmail("devjohndoe@mailinator.com").Id;
            //    //assign userId to a role
            //    userManager.AddToRole(userId, "Developer");
            //}

            //if (!context.Users.Any(u => u.Email == "devjanedoe@mailinator.com"))
            //{
            //    userManager.Create(new ApplicationUser()
            //    {
            //        Email = "devjanedoe@mailinator.com",
            //        UserName = "devjanedoe@mailinator.com",
            //        FirstName = "Jane",
            //        LastName = "Doe",
            //    }, "Abc@123!");

            //    //Get the ID that was just created by adding the above user
            //    var userId = userManager.FindByEmail("devjanedoe@mailinator.com").Id;
            //    //assign userId to a role
            //    userManager.AddToRole(userId, "Developer");
            //}           
            
            //if (!context.Users.Any(u => u.Email == "developer@mailinator.com"))
            //{
            //    userManager.Create(new ApplicationUser()
            //    {
            //        Email = "developer@mailinator.com",
            //        UserName = "developer@mailinator.com",
            //        FirstName = "Deve",
            //        LastName = "Loper",
            //    }, "password");
                
            //    var userId = userManager.FindByEmail("developer@mailinator.com").Id;                
            //    userManager.AddToRole(userId, "Developer");
            //}

            //if (!context.Users.Any(u => u.Email == "submitter1@mailinator.com"))
            //{
            //    userManager.Create(new ApplicationUser()
            //    {
            //        Email = "submitter1@mailinator.com",
            //        UserName = "submitter1@mailinator.com",
            //        FirstName = "Sub",
            //        LastName = "Mitter1",
            //    }, "Abc@123!");
            //    var userId = userManager.FindByEmail("submitter1@mailinator.com").Id;
            //    userManager.AddToRole(userId, "Submitter");
            //}

            //if (!context.Users.Any(u => u.Email == "submitter2@mailinator.com"))
            //{
            //    userManager.Create(new ApplicationUser()
            //    {
            //        Email = "submitter2@mailinator.com",
            //        UserName = "submitter2@mailinator.com",
            //        FirstName = "Sub",
            //        LastName = "Mitter2",
            //    }, "Abc@123!");
            //    var userId = userManager.FindByEmail("submitter2@mailinator.com").Id;
            //    userManager.AddToRole(userId, "Submitter");
            //}

            //if (!context.Users.Any(u => u.Email == "submitter3@mailinator.com"))
            //{
            //    userManager.Create(new ApplicationUser()
            //    {
            //        Email = "submitter3@mailinator.com",
            //        UserName = "submitter3@mailinator.com",
            //        FirstName = "Sub",
            //        LastName = "Mitter3",
            //    }, "Abc@123!");
            //    var userId = userManager.FindByEmail("submitter3@mailinator.com").Id;
            //    userManager.AddToRole(userId, "Submitter");
            //}

            if (!context.Users.Any(u => u.Email == "DemoAdminEmail@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoAdminEmail@mailinator.com",
                    UserName = "DemoAdminEmail@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Admin"
                }, DemoAdminPassword);

                
                var userId = userManager.FindByEmail("DemoAdminEmail@mailinator.com").Id;             
                userManager.AddToRole(userId, "Admin");
            }

            if (!context.Users.Any(u => u.Email == "DemoProjectManagerEmail@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoProjectManagerEmail@mailinator.com",
                    UserName = "DemoProjectManagerEmail@mailinator.com",
                    FirstName = "Demo",
                    LastName = "ProjectManager"
                }, DemoProjectManagerPassword);

                
                var userId = userManager.FindByEmail("DemoProjectManagerEmail@mailinator.com").Id;    
                userManager.AddToRole(userId, "Project Manager");
            }

            if (!context.Users.Any(u => u.Email == "DemoProjectManagerEmail1@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoProjectManagerEmail1@mailinator.com",
                    UserName = "DemoProjectManagerEmail1@mailinator.com",
                    FirstName = "Demo",
                    LastName = "ProjectManager1"
                }, DemoProjectManagerPassword1);

                
                var userId = userManager.FindByEmail("DemoProjectManagerEmail1@mailinator.com").Id;
                userManager.AddToRole(userId, "Project Manager");
            }

            if (!context.Users.Any(u => u.Email == "DemoDeveloperEmail@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoDeveloperEmail@mailinator.com",
                    UserName = "DemoDeveloperEmail@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Developer"
                }, DemoDeveloperPassword);

                
                var userId = userManager.FindByEmail("DemoDeveloperEmail@mailinator.com").Id;
                userManager.AddToRole(userId, "Developer");
            }

            if (!context.Users.Any(u => u.Email == "DemoDeveloperEmail1@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoDeveloperEmail1@mailinator.com",
                    UserName = "DemoDeveloperEmail1@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Developer1"
                }, DemoDeveloperPassword1);

                
                var userId = userManager.FindByEmail("DemoDeveloperEmail1@mailinator.com").Id;
                userManager.AddToRole(userId, "Developer");
            }

            if (!context.Users.Any(u => u.Email == "DemoSubmitterEmail@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoSubmitterEmail@mailinator.com",
                    UserName = "DemoSubmitterEmail@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Submitter"
                }, DemoSubmitterPassword);
                
                var userId = userManager.FindByEmail("DemoSubmitterEmail@mailinator.com").Id;
                userManager.AddToRole(userId, "Submitter");
            }

            if (!context.Users.Any(u => u.Email == "DemoSubmitterEmail1@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoSubmitterEmail1@mailinator.com",
                    UserName = "DemoSubmitterEmail1@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Submitter1"
                }, DemoSubmitterPassword1);

                
                var userId = userManager.FindByEmail("DemoSubmitterEmail1@mailinator.com").Id;
                userManager.AddToRole(userId, "Submitter");
            }

            if (!context.Users.Any(u => u.Email == "DemoSubmitterEmail2@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemoSubmitterEmail2@mailinator.com",
                    UserName = "DemoSubmitterEmail2@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Submitter2"
                }, DemoSubmitterPassword2);


                var userId = userManager.FindByEmail("DemoSubmitterEmail2@mailinator.com").Id;
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
                new Project() { Name = "Doublz Deli Register Software", Created = DateTime.Now.AddDays(-45), Description = "Doublz Deli purchased our Restaurant Cashier software."}
                );
            context.SaveChanges();

            //// Assign users to projects by Role
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
            //                    Issue = $"Ticket type: {type.Name}. Status: {status.Name}. Priority: {priority.Name}",
            //                    IssueDescription = $"This is a seeded ticket of type: {type.Name} for Project: {project.Name}.",
            //                    IsResloved = resolved,
            //                    IsArchived = archived
            //                };
            //                ticketList.Add(newTicket);
            //            }
            //        }
            //    }
            //}

            //context.Tickets.AddRange(ticketList);
            //context.SaveChanges();

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
