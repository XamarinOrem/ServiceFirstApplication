using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    public class ServiceFirst_Login
    {
        /*
         [ServiceFirstID]
      ,[ServiceFirstCompanyID]
      ,[ServiceFirstUserName]
      ,[ServiceFirstPassword]
      ,[ServiceFirstPasswordHash]
      ,[ServiceFirstResetCode]
      ,[ServiceFirstResectCoderExpiration]
      ,[ServiceFirstPassworSalt]
      ,[ServiceFirstUserFirstName]
      ,[ServiceFirstUserLastName]
      ,[ServiceFirstUserPicture]
      ,[ServiceFirstCompanyCustomersID]
      ,[ServiceFirstUserContactNumber]
      ,[ServiceFirstUserContactEmail]
      ,[ServiceFirstUserContactTitle]
         */
        public virtual long ServiceFirstID { get; set; }
        public virtual string ServiceFirstCompanyID { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        [Display(Name = "UserName")]
        public virtual string ServiceFirstUserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public virtual string ServiceFirstPassword { get; set; }

        [CompareAttribute("ServiceFirstPassword",ErrorMessage = "The passwords do not match.")]
        public virtual string ServiceFirstUserConfirmPassword { get; set; }
        public virtual string ServiceFirstPasswordHash { get; set; }
        public virtual string ServiceFirstResetCode { get; set; }
        public virtual string ServiceFirstResectCoderExpiration { get; set; }
        public virtual string ServiceFirstPassworSalt { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [Display(Name = "FirstName")]
        public virtual string ServiceFirstUserFirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [Display(Name = "LastName")]
        public virtual string ServiceFirstUserLastName { get; set; }
        public virtual string ServiceFirstName { get { return ServiceFirstUserFirstName+" "+ServiceFirstUserLastName; } }
        public virtual string ServiceFirstUserPicture { get; set; }
        public virtual long ServiceFirstCompanyCustomersID { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [Display(Name = "Contact Number")]
        [RegularExpression(@"^(\d{8})$", ErrorMessage = "Invalid Phone number")]
        public virtual string ServiceFirstUserContactNumber { get; set; }

        [Required(ErrorMessage = "Contact Email is required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid Email Id")]
        [Display(Name = "Contact Email")]
        public virtual string ServiceFirstUserContactEmail { get; set; }

        [Required(ErrorMessage = "Contact Title is required")]
        [Display(Name = "Contact Title")]
        public virtual string ServiceFirstUserContactTitle { get; set; }

        public virtual DateTime ContactPersonsListCreatedDate { get; set; }

        public virtual bool ContactPersonsListIsActive { get; set; }
    }


    public class ServiceFirst_LoginVM
    {
        public virtual long ServiceFirstID { get; set; }
        public virtual string ServiceFirstUserName { get; set; }
        public virtual string ServiceFirstUserFirstName { get; set; }
        public virtual string ServiceFirstUserLastName { get; set; }
        public virtual string ServiceFirstUserPicture { get; set; }
        public virtual string ServiceFirstCompanyCustomersID { get; set; }
        public virtual bool ContactPersonsListIsActive { get; set; }
    }
}