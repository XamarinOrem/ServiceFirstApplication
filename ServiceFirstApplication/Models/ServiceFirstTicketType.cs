using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    public class ServiceFirstTicketType
    {
        /*[ServiceFirstTickeTypeID]
      ,[ServiceFirstCompanyID]
      ,[ServiceFirstTicketTypeIcon]
      ,[ServiceFirstTicketTypeName]
      ,[ServiceFirstTicketTypeDateCreated]
      ,[ServiceFirstTicketTypeIsActive]*/

        public virtual long ServiceFirstTickeTypeID { get; set; }
        public virtual long ServiceFirstCompanyID { get; set; }
        public virtual string ServiceFirstTicketTypeIcon { get; set; }
        [Required(ErrorMessage = "Ticket Type Name is required")]
        [Display(Name = "Name")]
        public virtual string ServiceFirstTicketTypeName { get; set; }
        public virtual DateTime ServiceFirstTicketTypeDateCreated { get; set; }
        public virtual bool ServiceFirstTicketTypeIsActive { get; set; }
    }
}