using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections;
using System.Threading;
using System.Data.SqlClient;
using ServiceFirstApplication.Models;

namespace ServiceFirstApplication.Repository
{
    public class EmailThreading
    {
       // private readonly ServiceFirstEntities _dbContext;
        public EmailThreading()
        {
          //  _dbContext = new ServiceFirstEntities();
        }

        public delegate void delProfileApplicationCompliance();
        public static delProfileApplicationCompliance invProfileApplicationCompliance;
        public static string RequestFilePath { get; set; }
        public static bool RequestLogMode { get; set; }

        public void declaration()
        {
            //invProfileApplicationCompliance = new delProfileApplicationCompliance(SendScheduledMail);
            //invProfileApplicationCompliance.BeginInvoke(new AsyncCallback(CallbackDelegatedFunctions), null);
        }

        public static void CallbackDelegatedFunctions(IAsyncResult t)
        {
            List<string> objList = new List<string>();
            try
            {
                invProfileApplicationCompliance.EndInvoke(t);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// if doctor send prescription to any patient if the prescription about to finish before one day he/she will ge the email as reminder
        /// </summary>
        /// /*
        //public static void SendScheduledMail()
        //{
        //    ServiceFirstEntities _dbContext = new ServiceFirstEntities();
        //    string FileName = DateTime.Now.ToString().Replace("/", "_").Replace("\\", "_").Replace(" ", "_").Replace(":", "_") + ".txt";
        //    string filepath = ConfigClass.NotificationLogPath + FileName;
        //    string fileaccesspath = ConfigClass.SiteUrl + "Uploads/Log/" + FileName;
            
        //    List<PrescriptionReminderParams> lstCustomer = new List<PrescriptionReminderParams>();
        //    lstCustomer = _dbContext.Database.SqlQuery<PrescriptionReminderParams>
        //    ("exec MD_GetSchedulerPatientEmails").ToList<PrescriptionReminderParams>();
            
        //    var lstEmail = lstCustomer.Select(u => new
        //    {
        //        NAME = u.userName,
        //        EMAIL = u.UserEmail,
        //        MedicineName = u.MedicineName,
        //        DistinctData = u.DistinctData
        //    });

        //    List<string> objMsgs = new List<string>();
        //    objMsgs.Add("Campaign mails start at " + DateTime.Now.ToString());
        //    objMsgs.Add("Number of Email(s): " + lstEmail.Count().ToString());
        //    string strMsg = "Number of Email(s): " + lstEmail.Count();

        //    #region Create Log

        //    //MD_Mail_Log_Status objNew = new MD_Mail_Log_Status();
        //    //objNew.MailCount = lstEmail.Count();
        //    //objNew.MailDate = DateTime.Now;
        //    //objNew.MailTo = "";
        //    //objNew.ProcessStatus = "Processing..";
        //    //objNew.LogFilePath = fileaccesspath;
        //    //objNew.FileName = FileName;
        //    //objNew.Status = StatusType.Active;
        //    //objNew.CreatedDate = DateTime.Now;
        //    //_dbContext.MD_Mail_Log_Status.Add(objNew);
        //    //_dbContext.SaveChanges();

        //    //Int64? mailschedularid = objNew.mailschid;

        //    #endregion
        //    string staticPrescriptionReminderModuleLink = "";
        //    foreach (var item in lstEmail)
        //    {
        //        if (IsValidEmailID(item.EMAIL))
        //        {
        //            //staticPrescriptionReminderModuleLink = ConfigClass.SiteUrl + "Patient/Openprescription?ModuleName=" + NotificationModule.DoctorPrescriptionForm + "&ModuleID=" +  CommonFunctions.EncryptUrlParam(item.DistinctData);
        //            var htValues = new Hashtable();
        //            //htValues.Add("CONTENT", item.TEMPLATE.Replace("/Content/Assets", ConfigClass.SiteUrl + "Content/Assets"));
        //            htValues.Add("MedicineName", staticPrescriptionReminderModuleLink);//Medicinename will treat a prescription module link
        //            htValues.Add("FullName", item.NAME);
        //            htValues.Add("SiteName", ConfigClass.SiteName);
        //            //Thread.Sleep(30000); //30sec
        //            Thread.Sleep(2000); //30sec
        //            string mailresult = "";
        //            if (CommonFunctions.SendBulkMail(item.EMAIL, "Manddo- Medicine Reminder", ConfigClass.EmailTemplatePath, "PrescriptionReminder.html", htValues, ref mailresult))
        //            {
        //                objMsgs.Add("Email successfully sent: " + item.EMAIL + ". Time: " + DateTime.Now);
        //            }
        //            else
        //            {
        //                objMsgs.Add("Error during sending mail: " + item.EMAIL + ". Reason: " + mailresult + ". Time: " + DateTime.Now);
        //            }
        //        }
        //        else
        //        {
        //            objMsgs.Add("Email not valid: " + item.EMAIL);
        //        }
        //    }

        //    #region Update Log Status
        //  //  objNew = _dbContext.MD_Mail_Log_Status.Where(x => x.mailschid == mailschedularid).FirstOrDefault();
        //    //objNew.ProcessStatus = "Complete";
        //    _dbContext.SaveChanges();
        //    #endregion

        //    objMsgs.Add("Campaign process end.");
        //    MaintainMailLog(objMsgs, filepath);
        //}

        public static bool IsValidEmailID(string emailAddress)
        {
            return Regex.IsMatch(emailAddress, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public static string MaintainMailLog(List<string> msgs, string filePath = "")
        {
            string strResult = "";
            try
            {
                //if (ConfigClass.ERROR_LOG_MODE == "1")
                //{
                //    RequestLogMode = true;
                //}
                //else
                //{
                //    RequestLogMode = false;
                //}
                RequestLogMode = true;
                if (RequestLogMode)
                {
                    StreamWriter w;
                    w = File.Exists(filePath) ? File.AppendText(filePath) : File.CreateText(filePath);
                    w.WriteLine("---------------------------------------------------------------------");
                    w.WriteLine(DateTime.Now);
                    w.WriteLine("---------------------------------------------------------------------");
                    foreach (var objMsg in msgs)
                    {
                        w.WriteLine(objMsg);
                    }
                    w.WriteLine("---------------------------------------------------------------------");
                    w.Flush();
                    w.Close();
                    strResult = "Campaign file created successfully";
                }
                else
                {
                    strResult = "Campaign file disabled";
                }
            }
            catch (Exception ex)
            {
                strResult = "Exception: " + ex.InnerException;
                strResult += "Message: " + ex.Message;
            }
            return strResult;
        }



    }

    public class PrescriptionReminderParams
    {
        public long UserID { get; set; }
        public string UserEmail { get; set; }
        public string userName { get; set; }
        public Int32 MedicineDays { get; set; }
        public DateTime CreatedDate { get; set; }
        public string MedicineName { get; set; }
        public long DistinctData { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }

}