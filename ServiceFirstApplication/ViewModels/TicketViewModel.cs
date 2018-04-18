using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.ViewModels
{
    public class TicketViewModel
    {
        public long? ServiceFirstTicketID { get; set; }
        public string SeviceFirstTicketName { get; set; }
        public string ServiceFirstTicketDescription { get; set; }
        public string Customer { get; set; }
        public string Project { get; set; }
    }
}