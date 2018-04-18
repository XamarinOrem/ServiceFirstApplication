using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    public class ServiceFirstTicketPriority
    {
        /*
         [ServiceFirstTicketPriorityID]
      ,[ServiceFirstCompanyID]
      ,[ServiceFirstTicketPriorityIcon]
      ,[ServiceFirstTicketPriorityIndex]
      ,[ServiceFirstTicketPriorityName]
      ,[ServiceFirstTicketPriorityDateCreated]
      ,[ServiceFirstTicketPriorityIsActive]
      */
        public virtual long ServiceFirstTicketPriorityID { get; set; }
        public virtual long ServiceFirstCompanyID { get; set; }
        public virtual string ServiceFirstTicketPriorityIcon { get; set; }
        public virtual long ServiceFirstTicketPriorityIndex { get; set; }

        [Required(ErrorMessage ="Ticket Priority Name is required")]
        public virtual string ServiceFirstTicketPriorityName { get; set; }
        public virtual DateTime ServiceFirstTicketPriorityDateCreated { get; set; }
        public virtual bool ServiceFirstTicketPriorityIsActive { get; set; }
    }
}