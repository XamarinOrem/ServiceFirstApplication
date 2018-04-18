using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    /**
     * [ServiceFirstCompanyManagerID]
      ,[ServiceFirstCompanyManagerName]
      ,[ServiceFirstCompanyManagerCreatedDate]
      ,[ServiceFirstCompanyManagerAddress]
      ,[ServiceFirstCompanyManagerAddressII]
      ,[ServiceFirstCompanyManagerPostalNumber]
      ,[ServiceFirstCompanyManagerPostalPlace]
      ,[ServiceFirstCompanyManagerCountry]
      ,[ServiceFirstCompanyManagerContactEmail]
      ,[ServiceFirstCompanyManagerContactPhone]
      ,[ServiceFirstCompanyManagerCompanyID]
      ,[ServiceFirstCompanyManagerLogoFile]
      ,[ServiceFirstCompanyManagerIsActive]
      ,[ServiceFirstCompanyManagerUserName]
      ,[ServiceFirstCompanyManagerPassword]
      ,[ServiceFirstCompanyManagerPasswordSalt]
     */
    public class ServiceFirstCompanyManager
    {
        public virtual long ServiceFirstCompanyManagerID { get; set; }

        [CompareAttribute("ServiceFirstCompanyManagerPassword",
        ErrorMessage = "The password and confirmation password do not match.")]
        public virtual string ServiceFirstCompanyManagerConfirmPassword { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Manager Name")]
        public virtual string ServiceFirstCompanyManagerName { get; set; }

        [Required(ErrorMessage = "Address 1 is required")]
        [Display(Name = "Manager Address 1")]
        public virtual string ServiceFirstCompanyManagerAddress { get; set; }

        [Required(ErrorMessage = "Address 2 is required")]
        [Display(Name = "Manager Address 2")]
        public virtual string ServiceFirstCompanyManagerAddressII { get; set; }

        [Required(ErrorMessage = "Postal Number is required")]
        [Display(Name = "Manager Postal Number")]
        public virtual string ServiceFirstCompanyManagerPostalNumber { get; set; }

        [Required(ErrorMessage = "Postal Place is required")]
        [Display(Name = "Manager Place Number")]
        public virtual string ServiceFirstCompanyManagerPostalPlace { get; set; }

        [Required(ErrorMessage = "Contact Email is required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid Email Id")]
        [Display(Name = "Manager Contact Email")]
        public virtual string ServiceFirstCompanyManagerContactEmail { get; set; }

        [Required(ErrorMessage = "Contact Phone is required")]
        [Display(Name = "Manager Contact Phone")]
        [RegularExpression(@"^(\d{8})$", ErrorMessage = "Invalid Phone number")]
        public virtual string ServiceFirstCompanyManagerContactPhone { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        [Display(Name = "Manager Company Name")]
        public virtual Nullable<long> ServiceFirstCompanyManagerCompanyID { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        [Display(Name = "Manager User Name")]
        public virtual string ServiceFirstCompanyManagerUserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public virtual string ServiceFirstCompanyManagerPassword { get; set; }
        //public virtual string ServiceFirstCompanyManagerName { get; set; }
        public virtual DateTime ServiceFirstCompanyManagerCreatedDate { get; set; }
        //public virtual string ServiceFirstCompanyManagerAddress { get; set; }
        //public virtual string ServiceFirstCompanyManagerAddressII { get; set; }
        //public virtual string ServiceFirstCompanyManagerPostalNumber { get; set; }
       // public virtual string ServiceFirstCompanyManagerPostalPlace { get; set; }
        public virtual string ServiceFirstCompanyManagerCountry { get; set; }
        //public virtual string ServiceFirstCompanyManagerContactEmail { get; set; }
       // public virtual string ServiceFirstCompanyManagerContactPhone { get; set; }
       // public virtual long ServiceFirstCompanyManagerCompanyID { get; set; }
        public virtual string ServiceFirstCompanyManagerLogoFile { get; set; }
        public virtual bool ServiceFirstCompanyManagerIsActive { get; set; }
       // public virtual string ServiceFirstCompanyManagerUserName { get; set; }
       // public virtual string ServiceFirstCompanyManagerPassword { get; set; }
        public virtual string ServiceFirstCompanyManagerPasswordSalt { get; set; }
       // public virtual string ServiceFirstCompanyManagerConfirmPassword { get; set; }
    }


    public class ServiceFirstCompanyManagerMap : ClassMap<ServiceFirstCompanyManager>
    {
        public ServiceFirstCompanyManagerMap()
        {
            Table("ServiceFirstCompanyManager");
            Id(x => x.ServiceFirstCompanyManagerID, "ServiceFirstCompanyManagerID");
            Map(x => x.ServiceFirstCompanyManagerCompanyID, "ServiceFirstCompanyManagerCompanyID");
        }
        public class ServiceFirstCompaniesMap : ClassMap<ServiceFirstCompanies>
        {
            public ServiceFirstCompaniesMap()
            {
                Table("ServiceFirstCompanies");
                Id(x => x.ServiceFirstCompanyID, "ServiceFirstCompanyID");
                Map(x => x.ServiceFirstCompanyName, "ServiceFirstCompanyName");
                References<ServiceFirstCompanyManagerMap>(x => x.ServiceFirstCompanyID)
                    .ForeignKey("ServiceFirstCompanyManagerCompanyID");
            }
        }
    }

}