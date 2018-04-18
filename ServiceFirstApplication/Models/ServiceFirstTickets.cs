using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    public class ServiceFirstTickets
    {
        /*
           [ServiceFirstTicketID]
      ,[ServiceFirstCompanyID]
      ,[ServiceFirstCompanyCustomerID]
      ,[ServiceFirstCompanyCustomersProjectID]
      ,[SeviceFirstTicketName]
      ,[ServiceFirstTicetCreatedDate]
      ,[ServiceFirstTicketLastChanged]
      ,[ServiceFirstTicketReporter]
      ,[ServiceFirstTicketType]
      ,[ServiceFirstTicketPriority]
      ,[ServiceFirstTicketStatus]
      ,[ServiceFirstTicketResolution]
      ,[ServiceFirstTicketDescription]
      ,[ServiceFirstTicketAssigneeID]
         */
        public virtual long ServiceFirstTicketID { get; set; }
        public virtual long ServiceFirstTicketID { get; set; }
        public virtual long ServiceFirstCompanyID { get; set; }
        [Display(Name = "Customer")]
        [Required(ErrorMessage = "Customer is required.")]
        public virtual long ServiceFirstCompanyCustomerID { get; set; }
        public virtual Nullable<long> ServiceFirstCompanyCustomersProjectID { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public virtual string SeviceFirstTicketName { get; set; }
        public virtual DateTime ServiceFirstTicetCreatedDate { get; set; }
        public virtual DateTime ServiceFirstTicketLastChanged { get; set; }
        [Display(Name = "Reporter")]
        [Required(ErrorMessage = "Reporter is required.")]
        public virtual long ServiceFirstTicketReporter { get; set; }
        [Display(Name = "Ticket Type")]
        [Required(ErrorMessage = "Ticket Type is required.")]
        public virtual long ServiceFirstTicketType { get; set; }
        [Display(Name = "Ticket Priority")]
        [Required(ErrorMessage = "Ticket Priority is required.")]
        public virtual long ServiceFirstTicketPriority { get; set; }
        [Display(Name = "Ticket Status")]
        [Required(ErrorMessage = "Ticket Status is required.")]
        public virtual long ServiceFirstTicketStatus { get; set; }
        [Display(Name = "Resolution")]
        [Required(ErrorMessage = "Resolution is required.")]
        public virtual long ServiceFirstTicketResolution { get; set; }
        
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required.")]
        public virtual string ServiceFirstTicketDescription { get; set; }
        [Display(Name = "Assignee")]
        [Required(ErrorMessage = "Assignee is required.")]
        public virtual long ServiceFirstTicketAssigneeID { get; set; }
    }
}