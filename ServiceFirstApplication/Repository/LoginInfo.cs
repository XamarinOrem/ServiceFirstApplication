using System;
using System.Web;
using System.Web.Security;

namespace ServiceFirstApplication.Repository
{
    public static class LoginInfo
    {
       
        public static long AdminUserId
        {
            get { return Convert.ToInt64(HttpContext.Current.Session["AdminUserId"]); }
        }

        public static long LoginId
        {
            get { return Convert.ToInt64(HttpContext.Current.Session["LoginId"]); }
        }

        public static long DoctorId
        {
            get { return Convert.ToInt64(HttpContext.Current.Session["DoctorId"]); }
        }

        public static string RestrictionValidate { get { return Convert.ToString(HttpContext.Current.Session["ValidateResctriction"]); } }

        public static long SuperAdminId { get { return Convert.ToInt64(HttpContext.Current.Session["SuperAdminId"]); } set { HttpContext.Current.Session["SuperAdminId"] = value; } }
        public static string AdminUserName { get { return Convert.ToString(HttpContext.Current.Session["AdminUserName"]); } }
        public static string AdminFirstName { get { return Convert.ToString(HttpContext.Current.Session["AdminFirstName"]); } set { HttpContext.Current.Session["AdminFirstName"] = value; } }
        public static string AdminLastName { get { return Convert.ToString(HttpContext.Current.Session["AdminLastName"]); } set { HttpContext.Current.Session["AdminLastName"] = value; } }       
        public static string Type { get { return Convert.ToString(HttpContext.Current.Session["Type"]); } }

        public static DateTime? LastLogin { get { return (DateTime?)HttpContext.Current.Session["LastLogin"]; } }
        public static string LastLoginIP { get { return Convert.ToString(HttpContext.Current.Session["LastLoginIP"]).Trim() == "" ? "-" : Convert.ToString(HttpContext.Current.Session["LastLoginIP"]).Trim(); } }
        public static string LoggedUserIPAddress { get { return Convert.ToString(HttpContext.Current.Session["LoggedUserIPAddress"]).Trim() == "" ? "-" : Convert.ToString(HttpContext.Current.Session["LoggedUserIPAddress"]).Trim(); } }

        public static string SytemIPAddress { get { return Convert.ToString(HttpContext.Current.Session["SytemIPAddress"]).Trim() == "" ? "-" : Convert.ToString(HttpContext.Current.Session["SytemIPAddress"]).Trim(); } }
        public static bool IsSuper { get { return Type.Contains(UserTypeChar.SuperAdminChar); } }
        public static string LoginUserType { get { return Type; } }
        public static string UserImage { get { return Convert.ToString(HttpContext.Current.Session["UserImage"]); } }
        public static Int64 LoggedUserID { get { return Convert.ToInt64(HttpContext.Current.Session["LogedUserID"]); } }
        public static string LogedUserFullname { get { return Convert.ToString(HttpContext.Current.Session["LogedUserFullname"]); } }
        public static string UserType { get { return Convert.ToString(HttpContext.Current.Session["UserType"]); } }
        public static string UserEmail { get { return Convert.ToString(HttpContext.Current.Session["UserEmail"]); } }
        public static string FrontUserApproved { get { return Convert.ToString(HttpContext.Current.Session["FrontUserApproved"]); } }
        internal static void CreateAdditionalSession(bool IsHired, string HiredDate)
        {
            HttpContext.Current.Session["IsHired"] = IsHired;
            HttpContext.Current.Session["HiredDate"] = HiredDate;
        }

        internal static void CreateRestrictionSession(string IsValidateResctriction)
        {
            HttpContext.Current.Session["ValidateResctriction"] = IsValidateResctriction;
        }


        internal static void CreateFrontUserLoginSession(long LogedUserID, string LogedUserFullname, string UserType, bool KeepMeLogin,string userName,string password,bool approved)
        {
            HttpContext.Current.Session["LogedUserID"] = LogedUserID;
            HttpContext.Current.Session["LogedUserFullname"] = LogedUserFullname;
            HttpContext.Current.Session["UserType"] = UserType;
            HttpContext.Current.Session["UserEmail"] = userName;
            
             
            if (approved)
            {
                HttpContext.Current.Session["FrontUserApproved"] = "Yes";
            }
            else
            {
                HttpContext.Current.Session["FrontUserApproved"] = "No";
            }
            if (KeepMeLogin)
            {
                HttpContext.Current.Response.Cookies["loginUser"].Values.Add("uname", userName);
                HttpContext.Current.Response.Cookies["loginUser"].Values.Add("pwd", password);
            }
            FormsAuthentication.SetAuthCookie(LogedUserFullname, false);
            HttpContext.Current.Response.Cookies["loginAdmin"].Expires = DateTime.Now.AddDays(30);
        }


        internal static void CreateAdminLoginSession(long userId,
                                                string email,
                                                string userName, 
                                                string password,
                                                string firstName,
                                                string lastName,
                                                string type,
                                                bool KeepMeLogin,
                                                string salt)
        {
            HttpContext.Current.Session["AdminUserId"] = userId;
            HttpContext.Current.Session["OrganizationEmail"] = email;
            HttpContext.Current.Session["AdminUserName"] = userName;
            HttpContext.Current.Session["AdminFirstName"] = firstName;
            HttpContext.Current.Session["AdminLastName"] = lastName;
            HttpContext.Current.Session["Type"] = type;

            if (KeepMeLogin)
            {
                HttpContext.Current.Response.Cookies["loginAdmin"].Values.Add("uname", userName);
                HttpContext.Current.Response.Cookies["loginAdmin"].Values.Add("pwd", password);
                HttpContext.Current.Response.Cookies["loginAdmin"].Values.Add("salt", salt);
            }

            HttpContext.Current.Response.Cookies["loginAdmin"].Expires = DateTime.Now.AddDays(30);
        }

        internal static void CreateCManagerLoginSession(long userId,
                                               string email,
                                               string userName,
                                               string password,
                                               string Name,
                                               string CompanyId,
                                               string type,
                                               bool KeepMeLogin,
                                               string salt)
        {
            HttpContext.Current.Session["CManagerUserId"] = userId;
            HttpContext.Current.Session["CManagerEmail"] = email;
            HttpContext.Current.Session["CManagerUserName"] = userName;
            HttpContext.Current.Session["CManagerName"] = Name;
            HttpContext.Current.Session["CManagerCompanyId"] = CompanyId;
            HttpContext.Current.Session["Type"] = type;
            if (KeepMeLogin)
            {
                HttpContext.Current.Response.Cookies["loginCManager"].Values.Add("uname", userName);
                HttpContext.Current.Response.Cookies["loginCManager"].Values.Add("pwd", password);
                HttpContext.Current.Response.Cookies["loginCManager"].Values.Add("salt", salt);
            }

            HttpContext.Current.Response.Cookies["loginCManager"].Expires = DateTime.Now.AddDays(30);
        }


        public static bool IsAdminLoginUser { get { return AdminUserId > 0; } }       
        public static string AdminName { get { return AdminFirstName + " " + AdminLastName; } }

        public static bool IsFrontUserLogin { get { return LoggedUserID > 0; } }
        public static string SessionStatus
        {
            get
            {
                return LoggedUserID > 0 ? ServiceFirstApplication.Repository.SessionStatus.Active : ServiceFirstApplication.Repository.SessionStatus.InActive;
            }
            set
            {
                //SessionStatus = value;
            }
        }

        public static void LoginOffSession()
        {
            
            AdminUsers objUsers = new AdminUsers();
            //objUsers.UpdateUserIPAddress();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            LoginInfo.SessionStatus = ServiceFirstApplication.Repository.SessionStatus.Logout;
        }
    }
}