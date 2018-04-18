using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.ViewModels
{
    public class CompaniesViewModel
    {
        public long? ServiceFirstCompanyID { get; set; }
        public string ServiceFirstCompanyName { get; set; }
        public string ServiceFirstCompanyContactEmail { get; set; }
        public string ServiceFirstCompanyAddress { get; set; }
        public string ServiceFirstCompanyNoOfProjects { get; set; }
        public string ServiceFirstCompanyNoOfTickets { get; set; }
        public string ServiceFirstCompanyNoOfCustomers { get; set; }
        public string ServiceFirstCompanyLogoFile { get; set; }
        public bool ServiceFirstCompanyIsActive { get; set; }
        public string ServiceFirstCompanyManagerName { get; set; }
        public string ServiceFirstCompanyManagerId { get; set; }
    }
}