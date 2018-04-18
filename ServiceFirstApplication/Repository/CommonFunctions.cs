using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ErrorLibrary;
using Antlr.Runtime;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using ServiceFirstApplication.Models;

namespace ServiceFirstApplication.Repository
{
    public class CommonFunctions
    {

        public static string GetPath(string pathAfterUpload)
        {
            return Convert.ToString(HttpContext.Current.Server.MapPath(pathAfterUpload));

        }

        /// <summary>
        /// Function to get current year
        /// </summary>
        /// <returns></returns>
        public static string GetYear()
        {
            return Convert.ToString(DateTime.Now.Year);

        }

        public static Int64 getMonth(string month)
        {
            Int64 strMonth = 0;
            switch (month)
            {
                case "January":
                    strMonth = 1;
                    break;
                case "February":
                    strMonth = 2;
                    break;
                case "March":
                    strMonth = 3;
                    break;
                case "April":
                    strMonth = 4;
                    break;
                case "May":
                    strMonth = 5;
                    break;
                case "June":
                    strMonth = 6;
                    break;

                case "July":
                    strMonth = 7;
                    break;
                case "August":
                    strMonth = 8;
                    break;
                case "September":
                    strMonth = 9;
                    break;
                case "October":
                    strMonth = 10;
                    break;
                case "November":
                    strMonth = 11;
                    break;
                case "December":
                    strMonth = 12;
                    break;
                default:
                    strMonth = 0;
                    break;
            }
            return strMonth;
        }


        public static string getMonthName(Int16 month)
        {
            string strMonth = "";
            switch (month)
            {
                case 1:
                    strMonth = "January";
                    break;
                case 2:
                    strMonth = "February";
                    break;
                case 3:
                    strMonth = "March";
                    break;
                case 4:
                    strMonth = "April";
                    break;
                case 5:
                    strMonth = "May";
                    break;
                case 6:
                    strMonth = "June";
                    break;

                case 7:
                    strMonth = "July";
                    break;
                case 8:
                    strMonth = "August";
                    break;
                case 9:
                    strMonth = "September";
                    break;
                case 10:
                    strMonth = "October";
                    break;
                case 11:
                    strMonth = "November";
                    break;
                case 12:
                    strMonth = "December";
                    break;
                default:
                    strMonth = "";
                    break;
            }
            return strMonth;
        }

        public static string getTime(Int16 intHours, Int16 intMinutes)
        {

            var hours = intHours;
            var minutes = intMinutes;
            var amPmDesignator = "AM";
            if (hours == 0)
                hours = 12;
            else if (hours == 12)
                amPmDesignator = "PM";
            else if (hours > 12)
            {
                hours -= 12;
                amPmDesignator = "PM";
            }
            var formattedTime =
              String.Format("{0}:{1:00} {2}", hours, minutes, amPmDesignator);
            return formattedTime.ToString();
        }



        public static string GetDay(Int16 day)
        {
            string strday = "";
            switch (day)
            {
                case 0:
                    strday = "Mon";
                    break;
                case 1:
                    strday = "Tue";
                    break;
                case 2:
                    strday = "Wed";
                    break;
                case 3:
                    strday = "Thu";
                    break;
                case 4:
                    strday = "Fri";
                    break;
                case 5:
                    strday = "Sat";
                    break;
                case 6:
                    strday = "Sun";
                    break;
                default:
                    strday = "";
                    break;
            }
            return strday;
        }


        internal static bool Validate(string value, string strRegex, int maxLength = -1, int minLength = -1)
        {
            bool returnValue = true;

            Regex regex = new Regex(strRegex);

            if (maxLength > 0)
            {
                if (value.Length > maxLength)
                    returnValue = false;
            }

            if (minLength > 0)
            {
                if (value.Length < maxLength)
                    returnValue = false;
            }


            if (!regex.Match(value).Success)
                returnValue = false;

            return returnValue;
        }





        #region Encryption/ Decryption
        /// <summary>
        /// Encrypting params here
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string EncryptString(string Password, string salt)
        {

            if (Password == null)
                return null;
            if (Password == "")
                return "";

            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(salt));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Password);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }

        /// <summary>
        /// Decrypting string here
        /// </summary>
        /// <param name="EncryptedString"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string DecryptString(string EncryptedString, string salt)
        {
            if (EncryptedString == null)
                return null;
            if (EncryptedString == "")
                return "";

            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(salt));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(EncryptedString);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

        const string passphrase = "password";
        //Valid base64 characters are below.
        //ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=
        public static string EncryptUrlParam(long id)
        {
            //return CommonLibClass.encryptStr(Convert.ToString(id)); 
            string Message = Convert.ToString(id);
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(Message);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            var baseStr = Convert.ToBase64String(Results);
            return baseStr.Replace("+", ",").Replace("/", "_").Replace("=", "-");
        }

        public static long DecryptUrlParam(string eid)
        {
            try
            {
                //return (eid != null) ? Convert.ToInt32(CommonLibClass.decryptStr(eid)) : 0;
                if (eid != null)
                {
                    string Message = eid.Replace(",", "+").Replace("_", "/").Replace("-", "=");
                    byte[] Results;
                    System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
                    MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                    byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
                    TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
                    TDESAlgorithm.Key = TDESKey;
                    TDESAlgorithm.Mode = CipherMode.ECB;
                    TDESAlgorithm.Padding = PaddingMode.PKCS7;
                    byte[] DataToDecrypt = Convert.FromBase64String(Message);
                    try
                    {
                        ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                        Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                    }
                    finally
                    {
                        TDESAlgorithm.Clear();
                        HashProvider.Clear();
                    }
                    return Convert.ToInt64(UTF8.GetString(Results));
                }
            }
            catch
            {
                throw;
            }
            return 0;
        }
        #endregion

        #region Password
        /// <summary>
        /// Get password to match entered pwd
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetPassword(string p, string s)
        {
            return EncryptString(p, s);
        }

        /// <summary>
        /// password generation - forgot password
        /// </summary>
        /// <param name="p"></param>
        /// <param name="_salt"></param>
        /// <param name="_password"></param>
        public static void GeneratePassword(string p, string userType, ref string _salt, ref string _password)
        {
            if (userType == "new")
            {
                _salt = CommonLibClass.FetchRandStr(3);
            }
            _password = EncryptString(p, _salt);
        }

        /// <summary>
        /// Password decryption- log in
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DecryptPassword(string p, string s)
        {
            return DecryptString(p, s);
        }
        #endregion

        #region mails

        internal static bool SendMail(string email, string username, string Password, string UserType)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.Subject = "Login Details";
                mail.From = new System.Net.Mail.MailAddress(ConfigClass.SMTP_Username);
                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(mail.From.Address, ConfigClass.SMTP_Password);
                smtp.Host = "smtp.gmail.com";

                //recipient
                mail.To.Add(new MailAddress(email));

                mail.IsBodyHtml = true;
                string msg = "<span>" +
                       " Welcome to ServiceFirst!!<br><br>Congratulations for becoming a member of ServiceFirst system.<br><br> " +
                        "  <br ><br>" +
                                  "  Login details are as follows: <br><br>" +
                                   " User Type: " + UserType + "  <br> " +
                                    "Username: " + username + "<br>" +
                                  "  Password:" + Password + "<br>" +
                               " </span> ";

                mail.Body = msg;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        internal static bool SendUserNewRegistrationMail(ServiceFirstCompanyManager objUser)
        {
            var htValues = new Hashtable();
            htValues.Add("LogoPath", ConfigClass.SiteUrl);
            htValues.Add("SiteName", ConfigClass.SiteName);
            htValues.Add("FullName", objUser.ServiceFirstCompanyManagerName);
            //htValues.Add("usertype", objUser.UserType);
            htValues.Add("phonenumber", objUser.ServiceFirstCompanyManagerContactPhone);
            htValues.Add("Username", objUser.ServiceFirstCompanyManagerUserName);
            htValues.Add("password", objUser.ServiceFirstCompanyManagerPassword);
           
                htValues.Add("message", "Welcome to ServiceFirst!!<br><br>Congratulations for becoming a member of ServiceFirst system.<br><br> ");
           
            return SendMail(objUser.ServiceFirstCompanyManagerContactEmail, ConfigClass.SiteName + " - Registration information", ConfigClass.EmailTemplatePath, "registration.htm", htValues);
        }
        /// <summary>
        /// Created By: [Developer Name]
        /// Created Date: [date]
        /// Purpose: Bulding up the mail content for forgot email here
        /// </summary>
        /// <param name="objUser">details of the sender user</param> [All parameters with explanation]
        /// <param name="newpassword">New Password</param>
        /// <returns>In case of successfull mail send - function will return TRUE ; otherwise FALSE</returns> [Detail of Return parameter]
        /*
        internal static bool SendAdminMailForgot(MD_AdminUser objUser, string newpassword)
        {
            var htValues = new Hashtable();
            htValues.Add("LogoPath", ConfigClass.SiteUrl);
            htValues.Add("SiteName", ConfigClass.SiteName);
            htValues.Add("Fname", objUser.FirstName);
            htValues.Add("Lname", objUser.LastName);
            htValues.Add("UserName", objUser.UserName);
            htValues.Add("Password", newpassword);
            return SendMail(objUser.Email, "Manddo - Forgot Password", ConfigClass.EmailTemplatePath, "forgotpassword.htm", htValues);
        }


*/
        /*
        internal static bool SendRescheduleMail(string UserEmail, string BookFromFullName, string BookWithFullName, string AppointmentDate, string AppointmentTime, string Comments, string Message)
        {
            ManddoEntities db = new ManddoEntities();
            var htValues = new Hashtable();
            htValues.Add("LogoPath", ConfigClass.SiteUrl);
            htValues.Add("SiteName", ConfigClass.SiteName);
            htValues.Add("FullName", BookFromFullName);
            htValues.Add("BookWithFullName", BookWithFullName);
            htValues.Add("AppointmentDate", AppointmentDate);
            htValues.Add("Time", AppointmentTime);
            htValues.Add("Comments", Comments);
            htValues.Add("msgContent", Message);
            return SendMail(UserEmail, ConfigClass.SiteName + " -Book Appointment", ConfigClass.EmailTemplatePath, "bookappointment.html", htValues);
        }
        */














        /// <summary>
        /// Bulding up the mail content for forgot email here
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        //internal static bool SendMailForgot(RM_Registration objUser, string newpassword)
        //{
        //    var htValues = new Hashtable();
        //    htValues.Add("LogoPath", ConfigClass.SiteUrl);
        //    htValues.Add("SiteName", ConfigClass.SiteName);
        //    htValues.Add("Fname", objUser.FirstName);
        //    htValues.Add("Lname", objUser.LastName);
        //    htValues.Add("UserName", objUser.Email);
        //    htValues.Add("Password", newpassword);
        //    return SendMail(objUser.Email, "Recruit Me - Forgot Password", ConfigClass.EmailTemplatePath, "forgotpassword.htm", htValues);
        //}

        //internal static bool ChangePassword(RM_Registration objUser, string link)
        //{
        //    var htValues = new Hashtable();
        //    htValues.Add("LogoPath", ConfigClass.SiteUrl);
        //    htValues.Add("SiteName", ConfigClass.SiteName);
        //    htValues.Add("Fname", objUser.FirstName);
        //    htValues.Add("Lname", objUser.LastName);
        //    htValues.Add("link", link);

        //    return SendMail(objUser.Email, "Recruit Me - Change Password", ConfigClass.EmailTemplatePath, "changepassword.htm", htValues);
        //}


        /// <summary>
        /// Bulding up the mail content for forgot email here
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        internal static bool AddNewSkills(string skills, long id)
        {
            var htValues = new Hashtable();
            htValues.Add("LogoPath", ConfigClass.SiteUrl);
            htValues.Add("SiteName", ConfigClass.SiteName);
            htValues.Add("skills", skills);
            htValues.Add("url", ConfigClass.SiteUrl + "admin/login?skill=" + skills);
            htValues.Add("urlEdit", ConfigClass.SiteUrl + "admin/login?skilledit=" + CommonFunctions.EncryptUrlParam(id));
            return SendMail(ConfigClass.AdminEmail, "Recruit Me - New Skill Submitted", ConfigClass.EmailTemplatePath, "NewSkills.htm", htValues);
        }

        internal static bool SendCustomerLead(string toMail, string subject, string template, Hashtable htValues)
        {
            var htVal = htValues;

            return SendMail(toMail, subject, ConfigClass.EmailTemplatePath, template, htVal);
        }

        /// <summary>
        /// Mail sending functionality implemented here
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="templatePath"></param>
        /// <param name="templateName"></param>
        /// <param name="hashVars"></param>
        /// <returns></returns>
        public static bool SendMail(string email, string subject, string templatePath, string templateName, Hashtable hashVars)
        {
            try
            {

                #region Old Code

                //var footer = "Copyright &copy; " + GetYear();
                //hashVars.Add("MailFooter", "Copyright @ " + GetYear());
                //string apppath = System.Configuration.ConfigurationManager.AppSettings["WebApp_Path"];
                //var mailParser = new Parser(apppath + templatePath + "/" + templateName, hashVars);
                //var fromAddress = new MailAddress(ConfigClass.FromMail);
                //if (email == ConfigClass.AdminOrderEmail || email == ConfigClass.AdminEmail || email == ConfigClass.AdminRegistrationEmail)
                //{
                //    string[] userEmails = null;
                //    string sepratedEmails = "";
                //    if (email == ConfigClass.AdminRegistrationEmail)
                //    {
                //        sepratedEmails = email;
                //    }
                //    else if (email == ConfigClass.AdminOrderEmail)
                //    {
                //        sepratedEmails = email;
                //    }
                //    else if (email == ConfigClass.AdminEmail)
                //    {
                //        sepratedEmails = email;
                //    }
                //    if (!string.IsNullOrEmpty(sepratedEmails))
                //    {
                //        sepratedEmails = sepratedEmails + "," + ConfigClass.AdminEmailCC;
                //    }
                //    else
                //    {
                //        sepratedEmails = ConfigClass.AdminEmailCC;
                //    }
                //    userEmails = sepratedEmails.Split(',');
                //    if (userEmails != null)
                //    {
                //        if (userEmails.Count() > 0)
                //        {
                //            foreach (var data in userEmails)
                //            {
                //                try
                //                {
                //                    var message = new MailMessage();
                //                    message.From = fromAddress;
                //                    message.Subject = subject;
                //                    message.IsBodyHtml = true;
                //                    message.Body = mailParser.Parse();
                //                    message.BodyEncoding = Encoding.UTF8;
                //                    message.To.Add(data);
                //                    var client = new SmtpClient(ConfigClass.SMTP_Host);
                //                    client.Port = 25;
                //                    var credential = new NetworkCredential();
                //                    credential.UserName = ConfigClass.SMTP_Username;
                //                    credential.Password = ConfigClass.SMTP_Password;
                //                    client.Credentials = credential;
                //                    client.Send(message);
                //                    client.Dispose();
                //                }
                //                catch (Exception ex)
                //                {
                //                    List<string> successMessage = new List<string>();
                //                    successMessage.Add(data + " " + ex.InnerException.Message);
                //                    string FileName = DateTime.Now.ToString().Replace("/", "_").Replace("\\", "_").Replace(" ", "_").Replace(":", "_") + ".txt";
                //                    string filepath = ConfigClass.NotificationLogPath + FileName;
                //                    EmailThreading.MaintainMailLog(successMessage, filepath);
                //                }
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    try
                //    {
                //        var message = new MailMessage();
                //        message.From = fromAddress;
                //        message.Subject = subject;
                //        message.IsBodyHtml = true;
                //        message.Body = mailParser.Parse();
                //        message.BodyEncoding = Encoding.UTF8;
                //        message.To.Add(email);
                //        var client = new SmtpClient(ConfigClass.SMTP_Host);
                //        client.Port = 25;
                //        var credential = new NetworkCredential();
                //        credential.UserName = ConfigClass.SMTP_Username;
                //        credential.Password = ConfigClass.SMTP_Password;
                //        client.Credentials = credential;
                //        client.Send(message);
                //        client.Dispose();

                //        //List<string> test = new List<string>();
                //        //test.Add("Success harcoded after dispose");
                //        //string FileNametest = DateTime.Now.ToString().Replace("/", "_").Replace("\\", "_").Replace(" ", "_").Replace(":", "_") + ".txt";
                //        //string filepathtest = ConfigClass.NotificationLogPath + FileNametest;
                //        //EmailThreading.MaintainMailLog(test, filepathtest);
                //    }
                //    catch(Exception ex)
                //    {
                //        List<string> successMessage = new List<string>();
                //        successMessage.Add(email + " " +  ex.InnerException.Message);
                //        string FileName = DateTime.Now.ToString().Replace("/", "_").Replace("\\", "_").Replace(" ", "_").Replace(":", "_") + ".txt";
                //        string filepath = ConfigClass.NotificationLogPath + FileName;
                //        EmailThreading.MaintainMailLog(successMessage, filepath);
                //    }

                //}

                #endregion

                invUpdateCommonMailPlay = new delCommonMail(CallCommonMailDelegatedFunctions);
                invUpdateCommonMailPlay.BeginInvoke(email, subject, templatePath, templateName, hashVars, new AsyncCallback(CallbackCommonMailDelegatedFunctions), null);
                return true;
            }
            catch (Exception ex)
            {
                List<string> message = new List<string>();
                message.Add(ex.InnerException.Message);
                string FileName = DateTime.Now.ToString().Replace("/", "_").Replace("\\", "_").Replace(" ", "_").Replace(":", "_") + ".txt";
                string filepath = ConfigClass.NotificationLogPath + FileName;
                EmailThreading.MaintainMailLog(message, filepath);
                ErrorLibrary.ErrorClass.WriteLog(ex.InnerException, HttpContext.Current.Request);
                return false;
            }
        }


        #region Asynchronous Code Email
        public static delCommonMail invUpdateCommonMailPlay;
        public static void CallbackCommonMailDelegatedFunctions(IAsyncResult t)
        {
            try
            {
                invUpdateCommonMailPlay.EndInvoke(t);
            }
            catch (Exception ex)
            {
                List<string> msg = new List<string>();
                msg.Add("Exception in callback update locaton: " + Convert.ToString(ex.Message));
            }
        }
        public delegate void delCommonMail(string email, string subject, string templatePath, string templateName, Hashtable hashVars);
        public static void CallCommonMailDelegatedFunctions(string email, string subject, string templatePath, string templateName, Hashtable hashVars)
        {
            var footer = "Copyright &copy; " + GetYear();
            hashVars.Add("MailFooter", "Copyright @ " + GetYear());
            string apppath = System.Configuration.ConfigurationManager.AppSettings["WebApp_Path"];
            var mailParser = new Parser(apppath + templatePath + "/emailtemplates/" + templateName, hashVars);
            var fromAddress = new MailAddress(ConfigClass.FromMail);

            if (email == ConfigClass.AdminOrderEmail || email == ConfigClass.AdminEmail || email == ConfigClass.AdminRegistrationEmail)
            {
                string[] userEmails = null;
                string sepratedEmails = "";
                if (email == ConfigClass.AdminRegistrationEmail)
                {
                    sepratedEmails = email;
                }
                else if (email == ConfigClass.AdminOrderEmail)
                {
                    sepratedEmails = email;
                }
                else if (email == ConfigClass.AdminEmail)
                {
                    sepratedEmails = email;
                }

                if (!string.IsNullOrEmpty(sepratedEmails))
                {
                    sepratedEmails = sepratedEmails + "," + ConfigClass.AdminEmailCC;
                }
                else
                {
                    sepratedEmails = ConfigClass.AdminEmailCC;
                }
                userEmails = sepratedEmails.Split(',');
                if (userEmails != null)
                {
                    if (userEmails.Count() > 0)
                    {
                        foreach (var data in userEmails)
                        {
                            var message = new MailMessage();
                            message.From = fromAddress;
                            message.Subject = subject;
                            message.IsBodyHtml = true;
                            message.Body = mailParser.Parse();
                            message.BodyEncoding = Encoding.UTF8;
                            message.To.Add(data);

                            var client = new SmtpClient(ConfigClass.SMTP_Host);
                            client.Port = 25;
                            var credential = new NetworkCredential();
                            credential.UserName = ConfigClass.SMTP_Username;
                            credential.Password = ConfigClass.SMTP_Password;
                            client.Credentials = credential;
                            client.Send(message);
                            client.Dispose();
                        }
                    }
                }
            }
            else
            {
                var message = new MailMessage();
                message.From = fromAddress;
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = mailParser.Parse();
                message.BodyEncoding = Encoding.UTF8;
                message.To.Add(email);

                var client = new SmtpClient(ConfigClass.SMTP_Host);
                client.Port = 25;
                var credential = new NetworkCredential();
                credential.UserName = ConfigClass.SMTP_Username;
                credential.Password = ConfigClass.SMTP_Password;
                client.Credentials = credential;
                client.Send(message);
                client.Dispose();
            }

        }
        #endregion

        #endregion

      
                public static bool SendBulkMail(string email, string subject, string templatePath, string templateName, Hashtable hashVars, ref string mailresult)
                {
                    try
                    {
                        var footer = "Copyright &copy; " + GetYear();
                        hashVars.Add("MailFooter", "Copyright @ " + GetYear());
                        hashVars.Add("LogoPath", ConfigClass.SiteUrl);
                        string apppath = System.Configuration.ConfigurationManager.AppSettings["WebApp_Path"];
                        var mailParser = new Parser(apppath + templatePath + "/" + templateName, hashVars);
                        //var mailParser = new Parser(Path.Combine(HttpRuntime.AppDomainAppPath, templatePath + "\\" + templateName), hashVars);
                        var message = new MailMessage();
                        var fromAddress = new MailAddress(ConfigClass.FromMail);
                        message.To.Add(email);
                        message.From = fromAddress;
                        message.Subject = subject;
                        message.IsBodyHtml = true;
                        message.Body = mailParser.Parse();
                        message.BodyEncoding = Encoding.UTF8;

                        var client = new SmtpClient(ConfigClass.SMTP_Host);
                        client.Port = 25;
                        var credential = new NetworkCredential();
                        credential.UserName = ConfigClass.SMTP_Username;
                        credential.Password = ConfigClass.SMTP_Password;
                        client.Credentials = credential;
                        client.Send(message);
                        message.Dispose();
                        client.Dispose();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //ErrorLibrary.ErrorClass.WriteLog(ex.InnerException, HttpContext.Current.Request);
                        mailresult = ex.Message.ToString();
                        return false;
                    }
                }


     

        public static string Message(string action, string module)
        {
            switch (action.ToLower())
            {
                case "add":
                    return "New " + module + " has been added.";
                case "update":
                    return module + " has been updated.";
                case "send":
                    return module + " send successfully.";
                case "unaccess":
                    return "You are not authorised to perform this action!";
                case "duplicate":
                    return module + " is already exist.";
                case "delete":
                    return module + " deleted successfully.";
                default:
                    return action;
            }
        }

        public static void WriteLog(string message)
        {
            using (var sw = File.AppendText(HttpContext.Current.Server.MapPath(ConfigClass.ErrorFilePath)))
            {
                sw.Write(DateTime.Now + ": " + "Message:  " + message + "....." + HttpContext.Current.Request.ToString() + System.Reflection.MethodInfo.GetCurrentMethod());
                sw.Write(Environment.NewLine);
                sw.Close();
            }


        }
        public static Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
        {
            System.Drawing.Bitmap bmpOut = null;
            try
            {
                Bitmap loBMP = new Bitmap(lcFilename);
                ImageFormat loFormat = loBMP.RawFormat;

                decimal lnRatio;
                int lnNewWidth = 0;
                int lnNewHeight = 0;

                //*** If the image is smaller than a thumbnail just return it
                if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
                    return loBMP;

                if (loBMP.Width > loBMP.Height)
                {
                    lnRatio = (decimal)lnWidth / loBMP.Width;
                    lnNewWidth = lnWidth;
                    decimal lnTemp = loBMP.Height * lnRatio;
                    lnNewHeight = (int)lnTemp;
                }
                else
                {
                    lnRatio = (decimal)lnHeight / loBMP.Height;
                    lnNewHeight = lnHeight;
                    decimal lnTemp = loBMP.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }
                bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);

                loBMP.Dispose();
            }
            catch
            {
                return null;
            }

            return bmpOut;
        }
       



    }
}

   
