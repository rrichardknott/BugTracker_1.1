using BugTracker_1._1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace BugTracker_1._1.Helpers
{
    public class TicketHelper
    {
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        EmailService svc = new EmailService();
        readonly string from = "Richlynn Bug Tracker<richlynnbugtracker@mailinator.com>";
        
        public int TotalNumberOfTickets()
        {
            var currentNumberOfTickets = db.Tickets.Count();
            return currentNumberOfTickets;
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
                case "ProjectManager":
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
            //Scenario 1:  Assignment - Old ticket was not assigned and is now assigned to developer
            if (oldTicket.DeveloperId == "" && newTicket.DeveloperId != "")
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

                var userEmail = db.Users.Find(newNotification.UserId).Email;
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

                db.SaveChanges();

            }

            //Scenario 2:  Unassignment - Ticket has been unassigned from developer
            if (oldTicket.DeveloperId != "" && newTicket.DeveloperId == "")
            {
                var newTicketNotification = new TicketNotification
                {
                    Created = DateTime.Now,
                    TicketId = newTicket.Id,
                    UserId = oldTicket.DeveloperId,
                    Subject = $"You have been Unassigned from Ticket Id {oldTicket.Id}.",
                    Message = $"Hi {oldTicket.Developer.FullName}. You have been Unassigned from Ticket Id {oldTicket.Id}, {oldTicket.Project.Name}."
                };
                db.TicketNotifications.Add(newTicketNotification);

                var userEmail = db.Users.Find(newTicketNotification.UserId).Email;
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

                db.SaveChanges();
            }

            //Scenario 3:  Reassignment - Old ticket was assigned to a developer and is now assigned to a new developer
            if (oldTicket.DeveloperId != "" && newTicket.DeveloperId != "" && oldTicket.DeveloperId != newTicket.DeveloperId)
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

                var userEmail = db.Users.Find(newTicketNotificationToNewDeveloper.UserId).Email;
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

                userEmail = db.Users.Find(newTicketNotificationToOldDeveloper.UserId).Email;
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

                db.SaveChanges();

            }
        }



        public List<TicketNotification> GetUnreadNotifications()
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifications.Include("Sender").Where(t => t.UserId == currentUserId && !t.IsRead).ToList();
        }

    }
}