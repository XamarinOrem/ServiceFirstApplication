using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    public class ServiceFirstProjects
    {
        public virtual long ServiceFirstProjectID { get; set; }

        [Display(Name = "Due Date")]
        [Required(ErrorMessage = "Due Date is required")]
        public virtual DateTime ServiceFirstProjectDueDate { get; set; }

        public virtual DateTime ServiceFirstProjectCreatedDate { get; set; }

        [Display(Name = "Customer")]
        [Required(ErrorMessage = "Customer is required")]
        public virtual long ServiceFirstProjectCustomer { get; set; }

        [Display(Name = "Owner")]
        [Required(ErrorMessage = "Owner is required")]
        public virtual long ServiceFirstProjectOwner { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        public virtual string ServiceFirstProjectName { get; set; }

        public virtual long ServiceFirstCompanyID { get; set; }

        public virtual bool ServiceFirstProjectIsActive { get; set; }
    }

    public class ServiceFirstProjectsVM
    {
        public virtual long ServiceFirstProjectID { get; set; }
        public virtual DateTime ServiceFirstProjectDueDate { get; set; }
        public virtual string ServiceFirstProjectCustomer { get; set; }
        public virtual string ServiceFirstProjectOwner { get; set; }
        public virtual string ServiceFirstProjectName { get; set; }
        public virtual long ServiceFirstCompanyID { get; set; }
        public virtual bool ServiceFirstProjectIsActive { get; set; }
    }
}