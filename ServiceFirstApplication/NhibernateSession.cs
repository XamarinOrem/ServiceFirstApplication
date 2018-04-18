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
            var ServiceFirstCountriesManagerConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Mappings\ServiceFirstCountries.hbm.xml");
            configuration.AddFile(ServiceFirstCountriesManagerConfigurationFile);

            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}