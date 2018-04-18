using NHibernate;
using NHibernate.Linq;
using ServiceFirstApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ServiceFirstApplication.Repository
{
    public class Users
    {
       // protected ServiceFirstEntities db = new ServiceFirstEntities();
        #region Properties/data members

        public ServiceFirst_AdminLogin UserObj { get; private set; }
     

        // public RM_Registration NormalUserObj { get; private set; }

        

        public string Message { get; private set; }

        string _salt, _password = string.Empty;
        public string currentUserType { get; set; }

        #endregion

        #region Constructors

        public Users()
        {
            //db = new ServiceFirstEntities();
        }
        //public Users(long? id)
        //{
        //    Db = new ManddoEntities();
        //    UserObj = Db.MD_AdminUser.FirstOrDefault(u => u.Id == id);
        //    UserLoginObj = Db.MD_UserLogin.FirstOrDefault(u => u.Id == id);
        //}

        #endregion
        public bool Login(LoginViewModel obj)
        {
            //string password = "123456";
            // CommonFunctions.GeneratePassword(password, "new", ref _salt, ref _password);
            try
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    UserObj = session.Query<ServiceFirst_AdminLogin>().Where(x => x.ServiceFirstUserName == obj.UserName).FirstOrDefault();
                }
               // UserObj = db.ServiceFirst_AdminLogin.FirstOrDefault(x => x.ServiceFirstUserName == obj.UserName);// && (x.UserType == UserType.Admin || x.UserType == UserType.SuperAdmin));
                if (UserObj != null)
                {
                    //if (!UserObj.Active)
                    //{
                    //    Message = "Your account is not activated.";
                    //    return false;
                    //}
                    //else
                    if (CommonFunctions.GetPassword(obj.Password, UserObj.ServiceFirstPassworSalt) 
                        == UserObj.ServiceFirstPassword)
                    {
                        obj.Salt = UserObj.ServiceFirstPassworSalt;
                        return true;
                    }
                    else
                        Message = "Please enter valid user name/ password.";
                }
                else
                    Message = "Please enter valid user name/ password.";
            }
            catch (Exception ex)
            {
                ErrorLibrary.ErrorClass.WriteLog(ex.GetBaseException(), System.Web.HttpContext.Current.Request);
                return false;
            }
            return false;
        }


        public bool AddCompanyManager(ServiceFirstCompanyManager _objCompanyManager)
        {
            bool status = false;
             CommonFunctions.GeneratePassword(_objCompanyManager.ServiceFirstCompanyManagerPassword,
                 "new", ref _salt, ref _password);
            try
            {

                _objCompanyManager.ServiceFirstCompanyManagerPassword = _password;
                _objCompanyManager.ServiceFirstCompanyManagerPasswordSalt = _salt;
                _objCompanyManager.ServiceFirstCompanyManagerCreatedDate = DateTime.Now;
                _objCompanyManager.ServiceFirstCompanyManagerIsActive = true;
                /*
                db.ServiceFirstCompanyManagers.Add(_objCompanyManager);
                db.SaveChanges();
                */
                using (ISession session = NHibernateSession.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                    {
                        session.Save(_objCompanyManager); //  Save the book in session
                        transaction.Commit();   //  Commit the changes to the database
                    }
                }
                CommonFunctions.SendMail(_objCompanyManager);
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }

            return status;
        }


        public bool ChangePasswordAdmin(ResetPasswordViewModel obj)
        {
            bool status = false;
            try
            {
                var id =(long) HttpContext.Current.Session["AdminUserId"];
              //  var data = db.ServiceFirst_AdminLogin.Where(x=>x.ServiceFirstID == id).FirstOrDefault();
                using (ISession session = NHibernateSession.OpenSession())
                {
                    UserObj = session.Query<ServiceFirst_AdminLogin>().Where(x => x.ServiceFirstID == id).FirstOrDefault();
                }
                if (CommonFunctions.GetPassword(obj.OldPassword, UserObj.ServiceFirstPassworSalt) ==
                    UserObj.ServiceFirstPassword)
                {
                    CommonFunctions.GeneratePassword(obj.Password, "new", ref _salt, ref _password);
                    UserObj.ServiceFirstPassword = _password;
                    UserObj.ServiceFirstPassworSalt = _salt;
                    /*
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    */
                    using (ISession session = NHibernateSession.OpenSession())
                    {
                        using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                        {
                            session.SaveOrUpdate(UserObj); //  Save the book in session
                            transaction.Commit();   //  Commit the changes to the database
                        }
                    }

                    status = true;
                }
                else
                {
                    Message = "Old password is not correct.";
                    status = false;
                }
                    
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status= false;
            }

            return status;
        }
        public SelectList GetCountrySelectList()
        {
            List<ServiceFirstCountries> countries = new List<Models.ServiceFirstCountries>();
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                countries = session.Query<ServiceFirstCountries>().ToList(); //  Querying to get all the books
            }
            return new SelectList(countries.ToArray(),
                                "Id",
                                "Name");

        }

        public SelectList GetCompanyList()
        {
          
            List<ServiceFirstCompanyManager> companiesManager = new List<ServiceFirstCompanyManager>();
            List<ServiceFirstCompanies> companies = new List<Models.ServiceFirstCompanies>();
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                try
                {
                    //try
                 //{
                 //    companiesManager = session.Query<ServiceFirstCompanyManager>()
                 //  .Where(a => a.ServiceFirstCompanyManagerCompanyID != null).ToList();
                 //}
                 //catch
                 //{
                 //}

                    // foreach (var item in companiesManager)
                    //{
                    companies = session.Query<ServiceFirstCompanies>().Where(a => a.ServiceFirstCompanyIsActive == true
                   //&& a.ServiceFirstCompanyID != item.ServiceFirstCompanyManagerCompanyID
                   ).ToList();
                    //}
                }
                catch
                {

                }
               

            }
            return new SelectList(companies.ToArray(),
                                "ServiceFirstCompanyID",
                                "ServiceFirstCompanyName");
        }
    }
}