using System;
using BugTracker_1._1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugTracker_1._1.Helpers;
using System.EnterpriseServices;


namespace BugTracker_1._1.Controllers
{

    public class AssignmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        // GET: Assignments
        [Authorize(Roles = "Admin")]
        public ActionResult ManageRoles()
        {
            // Use my ViewBag to  hold a multi-select list of users in the system
            // new MultiSelectList(the data itself, "Id", "Email")
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");

            // Use my ViewBag to hold a select list of Roles
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name");

            return View(db.Users.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            // Step 1: If anyone was selected, remove them from all of their roles
            if (userIds == null)
            {
                return RedirectToAction("NoRoleSelected");
            }
            else
            {
                foreach (var userId in userIds)
                {
                    // Grab all user roles and assign to userRoles
                    var userRoles = roleHelper.ListUserRoles(userId).ToList();
                    // Determine if user currently occupies a role
                    foreach (var role in userRoles)
                    {
                        roleHelper.RemoveUserFromRole(userId, role);
                    }
                    // If a role was chosen, add each person to that role
                    if (!string.IsNullOrEmpty(roleName))
                    {
                        roleHelper.AddUserToRole(userId, roleName);
                    }

                }
            }

            // Step 2: If a role was chosen, add each person to that role


            return RedirectToAction("ManageRoles");
        }
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult ManageUserRoles()
        {
            return View();
        }

        #region Projects Assignments
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult ManageProjectUsers()
        {
            // If i want two listboxes in my view (Projects and Users) then I want two multiselect lists
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");//this is what is passed into POST

            ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name");

            return View(db.Users.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult ManageProjectUsers(List<string> userIds, List<int> projectIds)
        {

            // If no users and/or no projects selected send them back to this page
            if (userIds == null || projectIds == null)
            {
                return RedirectToAction("ManageProjectUsers");
            }
            foreach (var userId in userIds)
            {
                foreach (var projectId in projectIds)
                {
                    projectHelper.AddUserToProject(userId, projectId);
                }
            }

            return RedirectToAction("ManageProjectUsers");
        }


        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult RemoveUserFromProject()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");

            ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name");

            return View(db.Users.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult RemoveUserFromProject(List<string> userIds, List<int> projectIds)
        {
            if (userIds == null || projectIds == null)
            {
                return RedirectToAction("RemoveUserFromProject");
            }
            foreach (var userId in userIds)
            {
                foreach (var projectId in projectIds)
                {
                    projectHelper.RemoveUserFromProject(userId, projectId);
                }
            }
            return RedirectToAction("RemoveUserFromProject");
        }


        public ActionResult ManageUserProjects(string id)
        {
            var myProjectIds = projectHelper.ListUserProjects(id).Select(p => p.Id).ToList();
            ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name", myProjectIds);
            ViewBag.UserId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserProjects(string userId, List<int> projectIds)
        {
            //Remove user from all projects           
            foreach (var project in projectHelper.ListUserProjects(userId))
            {
                projectHelper.RemoveUserFromProject(userId, project.Id);
            }

            //Add user back to selected project
            if (projectIds != null)
            {
                foreach (var projectId in projectIds)
                {
                    projectHelper.AddUserToProject(userId, projectId);
                }
            }
            return RedirectToAction("ManageUserProjects");
        }

        #endregion
    }
}