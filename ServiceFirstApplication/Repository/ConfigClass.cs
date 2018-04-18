using System;
using System.Configuration;


namespace ServiceFirstApplication.Repository
{
    /// <summary>
    /// Created By: Arum
    /// Created By: 05 Nov. 2014 
    /// Purpose:Site prefix data
    /// </summary>
    public class ConfigClass
    {
        public static string SiteName = ConfigurationManager.AppSettings["SITE_NAME"];
        public static string SiteUrl = ConfigurationManager.AppSettings["site_url"];
        public static string SitePrefix = ConfigurationManager.AppSettings["SITE_PREFIX"];    
        public static int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PAGE_SIZE"]);
        public static int FrontSearchPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["FRONT_SEARCH_PAGE_SIZE"]);
        public static string WebApp_Path = ConfigurationManager.AppSettings["WebApp_Path"];
        public static int MessageListingPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["MESSAGE_LISTING_PAGE_SIZE"]);

        public static string SuperAdminID = ConfigurationManager.AppSettings["SuperAdminID"];
        public static string EmailTemplatePath = ConfigurationManager.AppSettings["EMAIL_TEMPLATE_PATH"];
        public static string FromMail = ConfigurationManager.AppSettings["FROM_EMAILID"];
        public static string SMTP_Host = ConfigurationManager.AppSettings["SMTP_Host"];
        public static string SMTP_Username = ConfigurationManager.AppSettings["SMTP_Username"];
        public static string SMTP_Password = ConfigurationManager.AppSettings["SMTP_Password"];
        public static string ErrorFilePath = ConfigurationManager.AppSettings["Error_FilePath"];
        public static string ErrorMessagesFilePath = ConfigurationManager.AppSettings["ErrorMessage_FilePath"];

        public static string AdminEmail = ConfigurationManager.AppSettings["AdminEmail"];
        public static string AdminEmailCC = ConfigurationManager.AppSettings["AdminCC"];
        //public static string AdminEmailBCC = ConfigurationManager.AppSettings["AdminBCC"];
        
        public static string AdminOrderEmail = ConfigurationManager.AppSettings["AdminOrderEmail"];
        public static string AdminOrderPhoneNumber = ConfigurationManager.AppSettings["AdminOrderPhoneNumber"];
        public static string AdminRegistrationEmail = ConfigurationManager.AppSettings["AdminRegistrationEmail"];

        public static string NotificationLogPath = ConfigurationManager.AppSettings["NotificationLogPath"];

        public static string NotificationSMSLogPath = ConfigurationManager.AppSettings["NotificationSMSLogPath"];

        public static string SMSUsername = ConfigurationManager.AppSettings["SMSUsername"];
        public static string SMSPassword = ConfigurationManager.AppSettings["SMSPassword"];
        public static string SMSSenderId = ConfigurationManager.AppSettings["SMSSenderId"];
        public static string SMSUrl = ConfigurationManager.AppSettings["SMS_URL"];
        
        public static string SHIPPINGCHARGE = ConfigurationManager.AppSettings["SHIPPINGCHARGE"];

        public static string IPHONE_CERT = ConfigurationManager.AppSettings["IPHONE_CERT"];

        public static string CERT_PASSWORD = ConfigurationManager.AppSettings["CERT_PASSWORD"];
        
        public static string GOOGLE_API_KEY = ConfigurationManager.AppSettings["GOOGLE_API_KEY"];
        public static string GOOGLE_PROJECT_ID = ConfigurationManager.AppSettings["GOOGLE_PROJECT_ID"];

        public static string FACEBOOK_API_KEY = ConfigurationManager.AppSettings["FACEBOOK_API_KEY"];
        public static string FACEBOOK_SECRET_KEY = ConfigurationManager.AppSettings["FACEBOOK_SECRET_KEY"];


        public static string ClientPhoneNumber1=ConfigurationManager.AppSettings["ClientPhone1"];
        public static string ClientPhoneNumber2 = ConfigurationManager.AppSettings["ClientPhone2"];

        public static string AppReminderMedicinePushNotificationPath = ConfigurationManager.AppSettings["AppReminderMedicinePushNotificationPath"];

    }
}