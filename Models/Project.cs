using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker_1._1.Models
{
    public class Project
    {
        public int Id { get; set; }        

        #region Actual Properties
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayFormat(DataFormatString="{0:MM/dd/yyyy}")]
        public DateTime Created { get; set; }

        [Display(Name = "Arch?")]
        public bool IsArchived { get; set; }
        #endregion

        #region Constructor
        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }
        #endregion

        #region Parents/Children
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        #endregion
    }
}