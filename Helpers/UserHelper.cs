using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker_1._1.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker_1._1.Helpers
{
    public class UserHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public string GetFullName(string userId)
        {
           
            var user = db.Users.Find(userId);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            return firstName + " " + lastName;
        }

        public string LastNameFirst(string userId)
        {
            var user = db.Users.Find(userId);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            return lastName + ", " + firstName;
        }       

        public string GetUserRole()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);            
            var roleId = user.Roles.Where(u => u.UserId == userId);         
            return null;
        }

        public string GetUserRole(string userId)
        {
            return null;
        }
    }
}