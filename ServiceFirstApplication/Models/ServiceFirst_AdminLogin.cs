using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    public class ServiceFirst_AdminLogin
    {
        /*
         * [ServiceFirstID]
      ,[ServiceFirstUserName]
      ,[ServiceFirstPassword]
      ,[ServiceFirstPassworSalt]
         * */
        public virtual long ServiceFirstID { get; set; }
        public virtual string ServiceFirstUserName { get; set; }
        public virtual string ServiceFirstPassword { get; set; }
        public virtual string ServiceFirstPassworSalt { get; set; }
    }
}