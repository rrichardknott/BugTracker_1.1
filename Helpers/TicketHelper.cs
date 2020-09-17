using BugTracker_1._1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace BugTracker_1._1.Helpers
{
    public class TicketHelper
    {
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        EmailService svc = new EmailService();
        readonly string from = "Richlynn Bug Tracker<richlynnbugtracker@mailinator.com>";


        public List<Ticket> GetProjectTickets()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var ticketList = new List<Ticket>();
            ticketList = user.Projects.SelectMany(p => p.Tickets).ToList();            
            return ticketList;
        }

        public int TotalNumberOfTickets()
        {
            var currentNumberOfTickets = db.Tickets.Count();
            return currentNumberOfTickets;
        }

        public int NumberOfTicketsOnProject(int projectId)
        {
            int numberOfTickets;

            Project project = db.Projects.Find(projectId);
            numberOfTickets = project.Tickets.Count();
            return numberOfTickets;
        }
        
        public bool CanMakeComment(int ticketId )
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var ticket = db.Tickets.Find(ticketId);
            switch (myRole)
            {
                case "Admin":
                    return true;
                case "Project Manager":
                    var user = db.Users.Find(userId);
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);
                case "Developer":
                case "Submitter":
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }

        }

        public async Task ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        {
            //Scenario 1:  Reassignment - Old ticket was assigned to one developer and is now assigned to another  developer
            if ((!oldTicket.DeveloperId.IsNullOrWhiteSpace() && (!newTicket.DeveloperId.IsNullOrWhiteSpace())) && (oldTicket.DeveloperId != newTicket.DeveloperId))

                           {
                var newTicketNotificationToNewDeveloper = new TicketNotification()
                {
                    TicketId = newTicket.Id,
                    Created = DateTime.Now,
                    UserId = newTicket.DeveloperId,
                    Subject = $"You have been assigned a new Ticket: Id = {newTicket.Id}.",
                    Message = $"Hi {newTicket.Developer.FullName}, you have been assigned a new Ticket: Id = {newTicket.Id}, Project: {newTicket.Project.Name}."
                };
                db.TicketNotifications.Add(newTicketNotificationToNewDeveloper);
                db.SaveChanges();
                var userId = newTicketNotificationToNewDeveloper.UserId;
                var userEmail = db.Users.Find(userId).Email;
                
                //var userEmail = db.Users.Find(newTicketNotificationToNewDeveloper.UserId).Email;
                try
                {
                    var email = new MailMessage(from, userEmail)
                    {
                        Subject = newTicketNotificationToNewDeveloper.Subject,
                        Body = newTicketNotificationToNewDeveloper.Message,
                        IsBodyHtml = true
                    };
                    await svc.SendAsync(email);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }


                var newTicketNotificationToOldDeveloper = new TicketNotification()
                {
                    Created = DateTime.Now,
                    UserId = oldTicket.DeveloperId,
                    TicketId = newTicket.Id,
                    Subject = $"Hi {oldTicket.Developer.FullName}",
                    Message = $"you have been Unassigned from Ticket {oldTicket.Id}, Project Name: {oldTicket.Project.Name}.  The new developer on this ticket is now {newTicket.Developer.FullName}, please be sure to assist him/ her with any helpful information you may have."
                };
                db.TicketNotifications.Add(newTicketNotificationToOldDeveloper);
                db.SaveChanges();
                userId = newTicketNotificationToOldDeveloper.UserId;
                userEmail = db.Users.Find(userId).Email;
                //userEmail = db.Users.Find(newTicketNotificationToOldDeveloper.UserId).Email;
                try
                {
                    var email = new MailMessage(from, userEmail)
                    {
                        Subject = newTicketNotificationToOldDeveloper.Subject,
                        Body = newTicketNotificationToOldDeveloper.Message,
                        IsBodyHtml = true
                    };
                    await svc.SendAsync(email);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }

                //db.SaveChanges();

            }

            //Scenario 2:  Assignment to Unassignment - Old Ticket was assigned and has now been Unassigned from developer
            if ((!oldTicket.DeveloperId.IsNullOrWhiteSpace()) && (newTicket.DeveloperId.IsNullOrWhiteSpace()))
            {
                var newTicketNotification = new TicketNotification
                {
                    Created = DateTime.Now,
                    TicketId = oldTicket.Id, // changed from newTicket.Id 8/31/2020
                    UserId = oldTicket.DeveloperId,
                    Subject = $"You have been Unassigned from Ticket Id {oldTicket.Id}.",
                    Message = $"Hi {oldTicket.Developer.FullName}. You have been Unassigned from Ticket Id {oldTicket.Id}, {oldTicket.Project.Name}."
                };
                db.TicketNotifications.Add(newTicketNotification);
                db.SaveChanges();// added 8/31/2020
                var userId = newTicketNotification.UserId;
                var userEmail = db.Users.Find(userId).Email;

                //var userEmail = db.Users.Find(newTicketNotification.UserId).Email;
                try
                {

                    var email = new MailMessage(from, userEmail)
                    {
                        Subject = newTicketNotification.Subject,
                        Body = newTicketNotification.Message,
                        IsBodyHtml = true
                    };
                    await svc.SendAsync(email);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }

                //db.SaveChanges();
            }


            //Scenario 3:  Unassignment to Assignment - Old ticket was not assigned and is now assigned to developer
            if (oldTicket.DeveloperId.IsNullOrWhiteSpace() && !newTicket.DeveloperId.IsNullOrWhiteSpace())
            {

                var newNotification = new TicketNotification()
                {
                    TicketId = newTicket.Id,
                    IsRead = false,
                    UserId = newTicket.DeveloperId,
                    Created = DateTime.Now,
                    Subject = $"You have been assigned a new Ticket: Id = {newTicket.Id}",
                    Message = $"Hi {newTicket.Developer.FullName}, you have been assigned a new Ticket:  Id =  {newTicket.Id}, Project: {newTicket.Project.Name}."
                };

                db.TicketNotifications.Add(newNotification);
                db.SaveChanges();// added 8/31/2020
                var userId = newNotification.UserId;
                var userEmail = db.Users.Find(userId).Email;

                //var userEmail = db.Users.Find(newNotification.UserId).Email;
                try
                {
                    var email = new MailMessage(from, userEmail)
                    {
                        Subject = newNotification.Subject,
                        Body = newNotification.Message,
                        IsBodyHtml = true
                    };
                    await svc.SendAsync(email);
                }
                catch(Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }

                //db.SaveChanges();

            }            
        }

        public List<TicketNotification> GetUnreadNotifications()
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            var unreadTickets = db.TicketNotifications.Include("Sender").Where(t => t.UserId == currentUserId && !t.IsRead).ToList();
            return unreadTickets;
        }

        public bool IsMyTicket(int id)
        {
            var allowed = false;
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var ticket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == id);
            var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            if (userRole == "Admin")
            {
                allowed = true;
            }

            else if (userRole == "Developer")
            {
                if (ticket.DeveloperId == userId)
                {
                    allowed = true;
                }
            }
            else if (userRole == "Submitter")
            {
                if (ticket.SubmitterId == userId)
                {
                    allowed = true;
                }
            }
            else if (userRole == "Project Manager")
            {
                // If the ticket is on one of the PM's projects
                var user = db.Users.Find(userId);
                if (user.Projects.SelectMany(p => p.Tickets).Select(t => t.Id).Contains(id))
                {
                    allowed = true;
                }
            }

            else
            {
                allowed = false;
            }


            return (allowed);
        
        
        }

    }
}