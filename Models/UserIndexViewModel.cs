using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker_1._1.Models
{
    public class UserIndexViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string FullName
        {
            get
            {
                return $"{FirstName}, {LastName}";
            }
        }

        //Allows for a dropdown list per user
        public IEnumerable<SelectListItem> RoleId { get; set; }
    }
}