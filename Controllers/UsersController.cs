using BugTracker_1._1.Helpers;
using BugTracker_1._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace BugTracker_1._1.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        // GET: Users
        public ActionResult Index()
        {
            //Create a ViewModel that has a few properties

            //var users = db.Users.Select(u => new UserIndexViewModel()
            //{
            //    Id = u.Id,
            //    FirstName = u.FirstName,
            //    LastName = u.LastName,
            //    Email = u.Email,
            //}).ToList();

            //foreach (var user in users)
            //{
            //    var roleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault();
            //    user.RoleId = new SelectList(db.Roles, "Name", "Name", roleName);
            //}


            return View(db.Users.ToList());
        }

        public ActionResult ManageUserRole(string id)
        {
            var userRole = roleHelper.ListUserRoles(id).FirstOrDefault(); //current role
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name", userRole);
            return View(db.Users.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserRole(string id, string roleName)
        {
            foreach (var role in roleHelper.ListUserRoles(id))
            {
                roleHelper.RemoveUserFromRole(id, role);
            }

            // If a role was selected then add that user to that role
            if (!string.IsNullOrEmpty(roleName))
            {
                roleHelper.AddUserToRole(id, roleName);
            }

            // Redirect the user back to the page they came from
            return RedirectToAction("ManageUserRole", new { id });
        }
    }
}