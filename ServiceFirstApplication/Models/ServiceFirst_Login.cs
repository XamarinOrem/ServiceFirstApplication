using System;
using System.Collections.Generic;
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
        public virtual string ServiceFirstUserName { get; set; }
        public virtual string ServiceFirstPassword { get; set; }
        public virtual string ServiceFirstPasswordHash  { get; set; }
        public virtual string ServiceFirstResetCode { get; set; }
        public virtual string ServiceFirstResectCoderExpiration { get; set; }
        public virtual string ServiceFirstPassworSalt { get; set; }
        public virtual string ServiceFirstUserFirstName { get; set; }
        public virtual string ServiceFirstUserLastName { get; set; }
        public virtual string ServiceFirstUserPicture { get; set; }
        public virtual string ServiceFirstCompanyCustomersID { get; set; }
        public virtual string ServiceFirstUserContactNumber { get; set; }
        public virtual string ServiceFirstUserContactEmail { get; set; }
        public virtual string ServiceFirstUserContactTitle { get; set; }

    }
}