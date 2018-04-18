using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication.Models
{
    public class ServiceFirstProjectCategories
    {
        /*        
CREATE TABLE [dbo].[ServiceFirstProjectCategories](
	[ServiceFirstProjectCategoryId] [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceFirstCompanyID] [bigint] NULL,
	[ServiceFirstProjectCategoryName] [nvarchar](max) NULL,
	[ServiceFirstProjectCategoryCreatedDate] [datetime] NULL,
	[ServiceFirstProjectCategoryIsActive] [bit] NULL,
    */
        public virtual long ServiceFirstProjectCategoryId { get; set; }
        public virtual long ServiceFirstCompanyID { get; set; }

        [Required(ErrorMessage = "Project Category Name is required")]
        [Display(Name = "Name")]
        public virtual string ServiceFirstProjectCategoryName { get; set; }

        public virtual DateTime ServiceFirstProjectCategoryCreatedDate { get; set; }
        public virtual bool ServiceFirstProjectCategoryIsActive { get; set; }
    }
}