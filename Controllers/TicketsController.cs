
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker_1._1.Models;
using Microsoft.AspNet.Identity;
using BugTracker_1._1.Helpers;
using System.Threading.Tasks;

namespace BugTracker_1._1.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectHelper projectHelper = new ProjectHelper();
        private UserRolesHelper userRolesHelper = new UserRolesHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private HistoryHelper historyHelper = new HistoryHelper();


        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var myRole = userRolesHelper.ListUserRoles(userId).FirstOrDefault();
            List<Ticket> model = new List<Ticket>();
            
            switch (myRole)
            {
                case "Admin":
                    model = db.Tickets.ToList();
                    break;
                case "Project Manager":
                case "Developer":
                    model = projectHelper.ListUserProjects(userId).SelectMany(p => p.Tickets).ToList();
                    break;
                case "Submitter":
                    model = db.Tickets.Where(t => t.SubmitterId == userId).ToList();
                    break;

                default:
                    return RedirectToAction("Index", "Home");
            }

            return View(model);
            
            //return View(db.Tickets.ToList());
        }
       
        public ActionResult GetProjectTickets()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var ticketList = new List<Ticket>();
            ticketList = user.Projects.SelectMany(p => p.Tickets).ToList();
            return View("Index", ticketList);
        }

        public ActionResult GetMyTickets()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var ticketList = new List<Ticket>();
            if (User.IsInRole("Developer"))
            {
                ticketList = db.Tickets.Where(t => t.DeveloperId == userId).ToList();
                return View("Index", ticketList);
            }
            if (User.IsInRole("Submitter"))
            {
                ticketList = db.Tickets.Where(t => t.SubmitterId == userId).ToList();
                return View("Index", ticketList);
            }
            else
            {
                return RedirectToAction("GetProjectTickets");
            }

        }

        // GET: Tickets/Dashboard/5
        public ActionResult Dashboard(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Ticket ticket = db.Tickets.Find(id);
        if (ticket == null)
        {
            return HttpNotFound();
        }
        return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter, Admin")]
        public ActionResult Create()
        {
            Ticket userModel = new Ticket();
            var userId = User.Identity.GetUserId();        
            if (userId == null)
            {
                return RedirectToAction("Index");
            }
            
            ViewBag.ProjectId = new SelectList(projectHelper.ListUserProjects(userId), "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            return View(userModel);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see //go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Submitter, Admin")]        
        public ActionResult Create([Bind(Include = "Id,ProjectId,TicketPriorityId,TicketTypeId,Issue,IssueDescription")] Ticket ticket)
        {
            var userId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                //Add back in: Created, SubmitterId
                //Set DeveloperId to null, IsArchived and IsResolved will be false
                ticket.TicketStatusId = db.TicketStatuses.Where(ts => ts.Name == "Open").FirstOrDefault().Id;
                ticket.Created = DateTime.Now;
                ticket.SubmitterId = userId;
                db.Tickets.Add(ticket);
                db.SaveChanges();                
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(projectHelper.ListUserProjects(userId), "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin, Developer, Project Manager, Submitter")]
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ticket ticket = db.Tickets.Find(id);

            if (ticketHelper.IsMyTicket((int)id) == false)
            {
                TempData["ErrorMessage"] = $"You are not authorized to edit Ticket Id: {id}. The Developer on this Ticket is {ticket.Developer.FullName} and the Submitter on this Ticket is {ticket.Submitter.FullName}.";
                return RedirectToAction("Unauthorized", "Tickets");
            }

            // Below is good code to determine if the developer or submitter have the same id as the ticket//

            

            //if (ticket == null)
            //{
            //    return HttpNotFound();
            //}            
            
            //if (User.IsInRole("Developer"))
            //{
            //    if (ticket.DeveloperId != User.Identity.GetUserId())
            //    {
            //        return HttpNotFound();
            //    }
            //}

            //if (User.IsInRole("Submitter"))
            //{
            //    if (ticket.SubmitterId != User.Identity.GetUserId())
            //    {
            //        return HttpNotFound();
            //    }
            //}
            //============================================================================//
           
            ViewBag.DeveloperId = new SelectList(projectHelper.ListUsersOnProjectInRole(ticket.ProjectId, "Developer"), "Id", "FullName", ticket.DeveloperId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see //go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProjectId,TicketPriorityId,TicketStatusId,TicketTypeId,SubmitterId,DeveloperId,Issue,IssueDescription,Created,IsResolved,IsArchived")] Ticket ticket)
        {            
            
            if (ModelState.IsValid)
            {
                //Go to db and get an unedited copy of the ticket.  AsNoTracking does not save it in dbContext
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t=> t.Id==ticket.Id);
                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                //Compare oldTicket with ticket after changes
                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                await ticketHelper.ManageTicketNotifications(oldTicket, newTicket);
                historyHelper.ManageHistories(oldTicket, newTicket);
                 
                return RedirectToAction("Index");
            }

            //If modelstate is not valid then send them back to their original edit view with the following info
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FullName", ticket.SubmitterId);
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FullName", ticket.DeveloperId);
            return View(ticket);
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
