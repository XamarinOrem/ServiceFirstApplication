using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    public class ServiceFirstUserGroup
    {
        public virtual long ServiceFirstGroupID { get; set; }
        public virtual string ServiceFirstCompanyID { get; set; }

        [Required(ErrorMessage = "Group Name is required")]
        [Display(Name = "Group Name")]
        public virtual string ServiceFirstGroupName { get; set; }

        public virtual DateTime ServiceFirstGroupCreatedDate { get; set; }

        public virtual bool ServiceFirstGroupIsActive { get; set; }
    }
}