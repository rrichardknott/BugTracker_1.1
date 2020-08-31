using BugTracker_1._1.Models;
using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace BugTracker_1._1.Helpers
{
    public class ProjectHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public int NumberOfProjects()
        {
            var currentNumberOfProjects = db.Projects.Count();
            return currentNumberOfProjects;
            
        }

        public List<string> NameOfProject()            
        {
            List<string> listOfProjectNames = new List<string>();
            foreach (var item in db.Projects)
            {
                if (item.Name != "")
                {
                    listOfProjectNames.Add(item.Name);
                }
            }


            return listOfProjectNames;
        }

        

        public bool IsUserOnProject(string userId, int projectId)
        {
            Project project = db.Projects.Find(projectId);
            return project.Users.Any(u => u.Id == userId);
        }

        // Add user to a project
        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                var project = db.Projects.Find(projectId);
                var user = db.Users.Find(userId);
                project.Users.Add(user);
                db.SaveChanges();
            }


        }

        // Remove user from a project
        public bool RemoveUserFromProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var user = db.Users.Find(userId);
            var result = project.Users.Remove(user);
            db.SaveChanges();
            return result;
        }

        // Be able to List users in a project
        public List<ApplicationUser> ListUsersOnProject(int projectId)
        {
            var project = db.Projects.Find(projectId);
            var resultList = new List<ApplicationUser>();
            resultList.AddRange(project.Users);
            return resultList;
        }

        // Be able to list users not in a project
        public List<ApplicationUser> ListUsersNotOnProject(int projectId)
        {
            var project = db.Projects.Find(projectId);
            var resultList = new List<ApplicationUser>();
            foreach (var user in db.Users.ToList())
            {
                if (!IsUserOnProject(user.Id, projectId))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        //List users on a project that occupy a specific role
        public List<ApplicationUser> ListUsersOnProjectInRole(int projectId, string roleName)
        {
            var userList = ListUsersOnProject(projectId);
            var resultList = new List<ApplicationUser>();
            var roleHelper = new UserRolesHelper();
            foreach (var user in userList)
            {
                if (roleHelper.IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        // List projects for a specific user
        public List<Project> ListUserProjects(string userId)
        {
            var user = db.Users.Find(userId);
            var resultList = new List<Project>();
            resultList.AddRange(user.Projects);
            return resultList;
        }

    }
}