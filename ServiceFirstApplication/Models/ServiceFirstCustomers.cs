using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    public class ServiceFirstCustomers
    {
        public virtual long ServiceFirstCustomerID { get; set; }

        [Required(ErrorMessage = "Customer Number is required")]
        [Display(Name = "Customer Number")]
        public virtual string ServiceFirstCustomerNumber { get; set; }
        [Required(ErrorMessage = "Customer Name is required")]
        [Display(Name = "Name")]
        public virtual string ServiceFirstCustomerName { get; set; }
        public virtual DateTime ServiceFirstCustomerCreatedDate { get; set; }

        [Required(ErrorMessage = "Organization Number is required")]
        [Display(Name = "Organization Number")]
        public virtual string ServiceFirstCustomerOrganisationNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public virtual string ServiceFirstCustomerAddress { get; set; }

        [Required(ErrorMessage = "Address 1 is required")]
        [Display(Name = "Address 1")]
        public virtual string ServiceFirstCustomerAddressII { get; set; }

        [Required(ErrorMessage = "Postal Number is required")]
        [Display(Name = "Postal Number")]
        public virtual string ServiceFirstCustomerPostalNumber { get; set; }

        [Required(ErrorMessage = "Postal Place is required")]
        [Display(Name = "Postal Place")]
        public virtual string ServiceFirstCustomerPostalPlace { get; set; }

        [Required(ErrorMessage = "Contact Email is required")]
        [Display(Name = "Contact Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid Email Id")]
        public virtual string ServiceFirstCustomerContactEmail { get; set; }

        [Required(ErrorMessage = "Contact Phone is required")]
        [Display(Name = "Contact Phone")]
        [RegularExpression(@"^(\d{8})$", ErrorMessage = "Invalid Phone number")]
        public virtual string ServiceFirstCustomerContactPhone { get; set; }

        public virtual long ServiceFirstCompanyID { get; set; }

        [Display(Name = "Country")]
        public virtual long ServiceFirstCustomerCountry { get; set; }

        public virtual bool ServiceFirstCustomerIsActive { get; set; }
        [Required(ErrorMessage = "Contact Person is required")]
        [Display(Name = "Contact Person")]
        public virtual string ServiceFirstCustomerContactPerson { get; set; }
    }
}