using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    public class ServiceFirstCompanies
    {
        /*
         [ServiceFirstCompanyID]
      ,[ServiceFirstCompanyName]
      ,[ServiceFirstCompanyCreatedDate]
      ,[ServiceFirstCompanyOrganisationNumber]
      ,[ServiceFirstCompanyAddress]
      ,[ServiceFirstCompanyAddressII]
      ,[ServiceFirstCompanyPostalNumber]
      ,[ServiceFirstCompanyPostalPlace]
      ,[ServiceFirstCompanyCountry]
      ,[ServiceFirstCompanyContactEmail]
      ,[ServiceFirstCompanyContactPhone]
      ,[ServiceFirstCompanyLogoFile]
      ,[ServiceFirstCompanyNoOfProjects]
      ,[ServiceFirstCompanyNoOfTickets]
      ,[ServiceFirstCompanyNoOfCustomers]
      ,[ServiceFirstCompanyIsActive]
         */

        public virtual long ServiceFirstCompanyID { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        [Display(Name = "Company Name")]
        public virtual string ServiceFirstCompanyName { get; set; }

        [Required(ErrorMessage = "Organization Number is required")]
        [Display(Name = "Organization Number")]
        public virtual string ServiceFirstCompanyOrganisationNumber { get; set; }

        [Required(ErrorMessage = "Address 1 is required")]
        [Display(Name = "Company Address")]
        public virtual string ServiceFirstCompanyAddress { get; set; }

        [Required(ErrorMessage = "Address 2 is required")]
        [Display(Name = "Company Address")]
        public virtual string ServiceFirstCompanyAddressII { get; set; }

        [Required(ErrorMessage = "Postal Number is required")]
        [Display(Name = "Company Postal Number")]
        public virtual string ServiceFirstCompanyPostalNumber { get; set; }

        [Required(ErrorMessage = "Postal Place is required")]
        [Display(Name = "Company Postal Place")]
        public virtual string ServiceFirstCompanyPostalPlace { get; set; }

        [Required(ErrorMessage = "Contact Email is required")]
        [Display(Name = "Company Contact Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid Email Id")]
        public virtual string ServiceFirstCompanyContactEmail { get; set; }

        [Required(ErrorMessage = "Contact Phone is required")]
        [Display(Name = "Company Contact Phone")]
        [RegularExpression(@"^(\d{8})$", ErrorMessage = "Invalid Phone number")]
        public virtual string ServiceFirstCompanyContactPhone { get; set; }

        [Required(ErrorMessage = "No. of Projects is required")]
        [Display(Name = "Company No of Projects")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public virtual Nullable<long> ServiceFirstCompanyNoOfProjects { get; set; }

        [Required(ErrorMessage = "No. of Tickets is required")]
        [Display(Name = "Company No of Tickets")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public virtual Nullable<long> ServiceFirstCompanyNoOfTickets { get; set; }

        [Required(ErrorMessage = "No. of Customers is required")]
        [Display(Name = "Company No of Customers")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public virtual Nullable<long> ServiceFirstCompanyNoOfCustomers { get; set; }
       // public virtual string ServiceFirstCompanyName { get; set; }
        public virtual DateTime ServiceFirstCompanyCreatedDate { get; set; }
       // public virtual string ServiceFirstCompanyOrganisationNumber { get; set; }
       // public virtual string ServiceFirstCompanyAddress { get; set; }
       // public virtual string ServiceFirstCompanyAddressII { get; set; }
       // public virtual string ServiceFirstCompanyPostalNumber { get; set; }
        //public virtual string ServiceFirstCompanyPostalPlace { get; set; }
        public virtual string ServiceFirstCompanyCountry { get; set; }
        //public virtual string ServiceFirstCompanyContactEmail { get; set; }
        //public virtual string ServiceFirstCompanyContactPhone { get; set; }
        public virtual string ServiceFirstCompanyLogoFile { get; set; }
        //public virtual string ServiceFirstCompanyNoOfProjects { get; set; }
       // public virtual string ServiceFirstCompanyNoOfTickets { get; set; }
       // public virtual string ServiceFirstCompanyNoOfCustomers { get; set; }
        public virtual bool ServiceFirstCompanyIsActive { get; set; }
    }
}