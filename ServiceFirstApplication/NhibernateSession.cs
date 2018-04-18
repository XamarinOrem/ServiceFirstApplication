using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceFirstApplication
{
    public class NHibernateSession
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();
            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\hibernate.cfg.xml");
            configuration.Configure(configurationPath);
           
            var ServiceFirst_AdminLoginConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirst_AdminLogin.hbm.xml");
            configuration.AddFile(ServiceFirst_AdminLoginConfigurationFile);
            var ServiceFirst_LoginConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirst_Login.hbm.xml");
            configuration.AddFile(ServiceFirst_LoginConfigurationFile);
            var ServiceFirstCompanyConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirstCompanies.hbm.xml");
            configuration.AddFile(ServiceFirstCompanyConfigurationFile);
            var ServiceFirstCompanyManagerConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirstCompanyManager.hbm.xml");
            configuration.AddFile(ServiceFirstCompanyManagerConfigurationFile);
            var ServiceFirstCountriesConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirstCountries.hbm.xml");
            configuration.AddFile(ServiceFirstCountriesConfigurationFile);
            var ServiceFirstProjectCategoriesConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirstProjectCategories.hbm.xml");
            configuration.AddFile(ServiceFirstProjectCategoriesConfigurationFile);
            var ServiceFirstTicketTypeConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirstTicketType.hbm.xml");
            configuration.AddFile(ServiceFirstTicketTypeConfigurationFile);
            var ServiceFirstTicketPriorityConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirstTicketPriority.hbm.xml");
            configuration.AddFile(ServiceFirstTicketPriorityConfigurationFile);
            var ServiceFirst_UserLoginConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirst_UserLogin.hbm.xml");
            configuration.AddFile(ServiceFirst_UserLoginConfigurationFile);
            var ServiceFirstUserGroups = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirstUserGroup.hbm.xml");
            configuration.AddFile(ServiceFirstUserGroups);
            var ServiceFirstCustomers = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirstCustomers.hbm.xml");
            configuration.AddFile(ServiceFirstCustomers);
            var ServiceFirstProjects = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirstProjects.hbm.xml");
            configuration.AddFile(ServiceFirstProjects);
            var ServiceFirstTicketStatus = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirstTicketStatus.hbm.xml");
            configuration.AddFile(ServiceFirstTicketStatus);
            var ServiceFirstTicketResolution = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirstTicketResolution.hbm.xml");
            configuration.AddFile(ServiceFirstTicketResolution);
            var ServiceFirstTicketsConf = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirstTickets.hbm.xml");
            configuration.AddFile(ServiceFirstTicketsConf);

            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}