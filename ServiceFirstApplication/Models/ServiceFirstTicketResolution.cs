using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    public class ServiceFirstTicketResolution
    {
        public virtual long ServiceFirstTicketResolutionID { get; set; }
        public virtual string ServiceFirstCompanyID { get; set; }

        [Display(Name = "Icon")]
        public virtual string ServiceFirstTicketResolutionIcon { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public virtual string ServiceFirstTicketResolutionName { get; set; }

        public virtual DateTime ServiceFirstTicketResolutionDateCreated { get; set; }

        public virtual bool ServiceFirstTicketResolutionIsActive { get; set; }
        public virtual long ServicFirstTicketResolutionIndex { get; set; }
    }
}