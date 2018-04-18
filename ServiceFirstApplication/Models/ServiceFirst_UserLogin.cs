using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    public class ServiceFirst_UserLogin
    {
        public virtual long ServiceFirstUserID { get; set; }
        public virtual long ServiceFirstCompanyID { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [Display(Name = "UserName")]
        public virtual string ServiceFirstUserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public virtual string ServiceFirstUserPassword { get; set; }

        [CompareAttribute("ServiceFirstUserPassword",
        ErrorMessage = "The passwords do not match.")]
        public virtual string ServiceFirstUserConfirmPassword { get; set; }
        public virtual string ServiceFirstUserResetCode { get; set; }
        public virtual string ServiceFirstUserResectCoderExpiration { get; set; }
        public virtual string ServiceFirstUserPassworSalt { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        [Display(Name = "FirstName")]
        public virtual string ServiceFirstUserFirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [Display(Name = "LastName")]
        public virtual string ServiceFirstUserLastName { get; set; }
        public virtual string ServiceFirstName { get { return ServiceFirstUserFirstName + " " + ServiceFirstUserLastName; } }
        public virtual string ServiceFirstUserPicture { get; set; }

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
        public virtual bool ServiceFirstUserIsActive { get; set; }
        public virtual DateTime ServiceFirstUserCreatedDate { get; set; }
        public virtual long ServiceFirstUserGroupID { get; set; }
    }

    public class ServiceFirst_UserLoginVM
    {
        public virtual long ServiceFirstUserID { get; set; }
        public virtual string ServiceFirstUserName { get; set; }
        public virtual string ServiceFirstUserFirstName { get; set; }
        public virtual string ServiceFirstUserLastName { get; set; }
        public virtual string ServiceFirstUserPicture { get; set; }
        public virtual string ServiceFirstUserGroupID { get; set; }

        public virtual string ServiceFirstUserContactNumber { get; set; }
        public virtual string ServiceFirstUserContactEmail { get; set; }
        public virtual bool ServiceFirstUserIsActive { get; set; }
    }

    public class ExportData
    {
        public long ID { get; set; }
        // public string UserName { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }

        public string ContactEmail { get; set; }

        //public  string ContactTitle { get; set; }
    }
}