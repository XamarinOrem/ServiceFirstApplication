using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    public class ServiceFirstTicketStatus
    {
        public virtual long ServiceFirstTicketStatusID { get; set; }
        public virtual string ServiceFirstCompanyID { get; set; }

        [Display(Name ="Icon")]
        public virtual string ServiceFirstTicketStatusIcon { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name ="Name")]
        public virtual string ServiceFirstTicketStatusName { get; set; }

        public virtual DateTime ServiceFirstTicketStatusCreatedDate { get; set; }

        public virtual bool ServiceFirstTicketStatusIsActive { get; set; }
    }
}