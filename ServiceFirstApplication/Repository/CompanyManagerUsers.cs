using ClosedXML.Excel;
using NHibernate;
using NHibernate.Linq;
using ServiceFirstApplication.Models;
using ServiceFirstApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using static ServiceFirstApplication.Models.ServiceFirstCompanyManagerMap;

namespace ServiceFirstApplication.Repository
{
    public class CompanyManagerUsers
    {
        #region Properties/data members

        public ServiceFirstCompanyManager UserObj { get; private set; }


        // public RM_Registration NormalUserObj { get; private set; }



        public string Message { get; private set; }

        string _salt, _password = string.Empty;
        public string currentUserType { get; set; }

        #endregion

        #region Constructors

        public CompanyManagerUsers()
        {

        }

        #endregion
        public bool Login(LoginViewModel obj, out long CompanyId)
        {
            CompanyId = 0;
            try
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    UserObj = session.Query<ServiceFirstCompanyManager>()
                        .Where(x => x.ServiceFirstCompanyManagerUserName == obj.UserName).FirstOrDefault();

                    if (UserObj != null)
                    {
                        if (!UserObj.ServiceFirstCompanyManagerIsActive)
                        {
                            Message = "Your account is not activated.";
                            return false;
                        }
                        else
                        {
                            session.Clear();
                            //CompanyId = (from q in session.Query<ServiceFirstCompanies>()
                            //             let p = session.Query<ServiceFirstCompanyManager>().Where(x => x.ServiceFirstCompanyManagerID ==
                            //             UserObj.ServiceFirstCompanyManagerCompanyID).FirstOrDefault()
                            //             select q.ServiceFirstCompanyID).FirstOrDefault();

                            CompanyId = UserObj.ServiceFirstCompanyManagerCompanyID??0;

                            if (CommonFunctions.GetPassword(obj.Password, UserObj.ServiceFirstCompanyManagerPasswordSalt)
                                == UserObj.ServiceFirstCompanyManagerPassword)
                            {
                                obj.Salt = UserObj.ServiceFirstCompanyManagerPasswordSalt;
                                return true;
                            }
                            else
                            {
                                Message = "Please enter valid user name/ password.";
                                return false;
                            }
                        }
                    }

                    else
                    {
                        Message = "Please enter valid user name/ password.";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLibrary.ErrorClass.WriteLog(ex.GetBaseException(), System.Web.HttpContext.Current.Request);
                return false;
            }
        }
        public bool UpdateLogo(String fileName)
        {
            bool status = false;
            try
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    var com_id = Convert.ToInt64(HttpContext.Current.Session["CManagerCompanyId"]);
                    var compnayData = session.Query<ServiceFirstCompanies>().Where(x => x.ServiceFirstCompanyID == com_id).FirstOrDefault();
                    compnayData.ServiceFirstCompanyLogoFile = fileName;
                    session.Clear();

                    using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                    {
                        session.SaveOrUpdate(compnayData); //  Save the book in session
                        transaction.Commit();   //  Commit the changes to the database
                    }
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public bool ChangePassword(ResetPasswordViewModel obj)
        {
            bool status = false;
            try
            {
                var id = (long)HttpContext.Current.Session["CManagerUserId"];
                //  var data = db.ServiceFirst_AdminLogin.Where(x=>x.ServiceFirstID == id).FirstOrDefault();
                using (ISession session = NHibernateSession.OpenSession())
                {
                    UserObj = session.Query<ServiceFirstCompanyManager>().Where(x => x.ServiceFirstCompanyManagerID == id).FirstOrDefault();
                }
                if (CommonFunctions.GetPassword(obj.OldPassword, UserObj.ServiceFirstCompanyManagerPasswordSalt) ==
                    UserObj.ServiceFirstCompanyManagerPassword)
                {
                    CommonFunctions.GeneratePassword(obj.Password, "new", ref _salt, ref _password);
                    UserObj.ServiceFirstCompanyManagerPassword = _password;
                    UserObj.ServiceFirstCompanyManagerPasswordSalt = _salt;
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
                status = false;
            }

            return status;
        }

        #region Project Categories
        public ServiceFirstProjectCategories GetProjectCategoryById(long? id)
        {
            ServiceFirstProjectCategories Company;
            using (ISession session = NHibernateSession.OpenSession())
            {
                Company = session.Query<ServiceFirstProjectCategories>().Where(b => b.ServiceFirstProjectCategoryId
                == id).FirstOrDefault();
            }
            return Company;
        }

        public bool AddEditProjectCat(ServiceFirstProjectCategories _objCatData)
        {
            bool status = false;
            try
            {
                _objCatData.ServiceFirstProjectCategoryName = _objCatData.ServiceFirstProjectCategoryName;
                _objCatData.ServiceFirstCompanyID = Convert.ToInt64((HttpContext.Current.Session["CManagerCompanyId"]
                    .ToString()));
                _objCatData.ServiceFirstProjectCategoryCreatedDate = DateTime.Now;
                _objCatData.ServiceFirstProjectCategoryIsActive = true;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                    {
                        if (_objCatData.ServiceFirstProjectCategoryId == 0)
                        {
                            session.Save(_objCatData); //  Save the book in session
                        }
                        else
                        {
                            session.SaveOrUpdate(_objCatData); //  Save the book in session
                        }
                        transaction.Commit();   //  Commit the changes to the database
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public List<ServiceFirstProjectCategories> GetProjectCategories()
        {
            List<ServiceFirstProjectCategories> mainList = new List<Models.ServiceFirstProjectCategories>();
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                try
                {
                    var companyId = Convert.ToInt64((HttpContext.Current.Session["CManagerCompanyId"]
                    .ToString()));
                    mainList = session.Query<ServiceFirstProjectCategories>().Where(x => x.ServiceFirstCompanyID == companyId).ToList();
                }
                catch (Exception ex)
                {

                }
            }
            return mainList;
        }
        public bool ActiveInActiveProjectCatById(int? Id)
        {
            bool status = false;
            try
            {
                ServiceFirstProjectCategories data;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    data = session.Query<ServiceFirstProjectCategories>().Where(b => b.ServiceFirstProjectCategoryId
                    == Id).FirstOrDefault();
                    if (data.ServiceFirstProjectCategoryIsActive)
                    {
                        data.ServiceFirstProjectCategoryIsActive = false;
                    }
                    else
                    {
                        data.ServiceFirstProjectCategoryIsActive = true;
                    }
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(data);
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }

        #endregion

        #region Ticket Types
        public bool AddEditTicketType(ServiceFirstTicketType _objCatData)
        {
            bool status = false;
            try
            {
                _objCatData.ServiceFirstTicketTypeName = _objCatData.ServiceFirstTicketTypeName;
                _objCatData.ServiceFirstCompanyID = Convert.ToInt64((HttpContext.Current.Session["CManagerCompanyId"]
                    .ToString()));
                _objCatData.ServiceFirstTicketTypeIcon = _objCatData.ServiceFirstTicketTypeIcon;
                _objCatData.ServiceFirstTicketTypeDateCreated = DateTime.Now;
                _objCatData.ServiceFirstTicketTypeIsActive = true;
                using (ISession session = NHibernateSession.OpenSession())
                {

                    using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                    {
                        if (_objCatData.ServiceFirstTickeTypeID == 0)
                        {
                            session.Save(_objCatData); //  Save the book in session
                        }
                        else
                        {
                            var data = session.Query<ServiceFirstTicketType>().Where(x => x.ServiceFirstTickeTypeID ==
                       _objCatData.ServiceFirstTickeTypeID).FirstOrDefault();
                            if (string.IsNullOrEmpty(_objCatData.ServiceFirstTicketTypeIcon))
                            {
                                _objCatData.ServiceFirstTicketTypeIcon = data.ServiceFirstTicketTypeIcon;
                            }
                            session.Clear();
                            session.SaveOrUpdate(_objCatData); //  Save the book in session
                        }
                        transaction.Commit();   //  Commit the changes to the database
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public List<ServiceFirstTicketType> GetTicketTypes()
        {
            List<ServiceFirstTicketType> mainList = new List<Models.ServiceFirstTicketType>();
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                try
                {
                    var companyId = Convert.ToInt64((HttpContext.Current.Session["CManagerCompanyId"]
                    .ToString()));
                    mainList = session.Query<ServiceFirstTicketType>().Where(x => x.ServiceFirstCompanyID == companyId).ToList();
                }
                catch (Exception)
                {

                }
            }
            return mainList;
        }
        public bool ActiveInActiveTicketType(int? Id)
        {
            bool status = false;
            try
            {
                ServiceFirstTicketType data;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    data = session.Query<ServiceFirstTicketType>().Where(b => b.ServiceFirstTickeTypeID
                    == Id).FirstOrDefault();
                    if (data.ServiceFirstTicketTypeIsActive)
                    {
                        data.ServiceFirstTicketTypeIsActive = false;
                    }
                    else
                    {
                        data.ServiceFirstTicketTypeIsActive = true;
                    }
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(data);
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public ServiceFirstTicketType GetTicketTypeById(long? id)
        {
            ServiceFirstTicketType typeData;
            using (ISession session = NHibernateSession.OpenSession())
            {
                typeData = session.Query<ServiceFirstTicketType>().Where(b => b.ServiceFirstTickeTypeID
                == id).FirstOrDefault();
            }
            return typeData;
        }

        #endregion

        #region Ticket Priority
        public ServiceFirstTicketPriority GetTicketPriorityById(long? id)
        {
            ServiceFirstTicketPriority typeData;
            using (ISession session = NHibernateSession.OpenSession())
            {
                typeData = session.Query<ServiceFirstTicketPriority>().Where(b => b.ServiceFirstTicketPriorityID
                == id).FirstOrDefault();
            }
            return typeData;
        }
        public bool ActiveInActiveTicketPriorityById(int? Id)
        {
            bool status = false;
            try
            {
                ServiceFirstTicketPriority data;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    data = session.Query<ServiceFirstTicketPriority>().Where(b => b.ServiceFirstTicketPriorityID
                    == Id).FirstOrDefault();
                    if (data.ServiceFirstTicketPriorityIsActive)
                    {
                        data.ServiceFirstTicketPriorityIsActive = false;
                    }
                    else
                    {
                        data.ServiceFirstTicketPriorityIsActive = true;
                    }
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(data);
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public bool AddEditTicketPriority(ServiceFirstTicketPriority _objCatData)
        {
            bool status = false;
            try
            {
                _objCatData.ServiceFirstTicketPriorityName = _objCatData.ServiceFirstTicketPriorityName;
                _objCatData.ServiceFirstCompanyID = Convert.ToInt64((HttpContext.Current.Session["CManagerCompanyId"]
                    .ToString()));
                _objCatData.ServiceFirstTicketPriorityIcon = _objCatData.ServiceFirstTicketPriorityIcon;
                _objCatData.ServiceFirstTicketPriorityDateCreated = DateTime.Now;
                _objCatData.ServiceFirstTicketPriorityIsActive = true;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    var indexData = session.CreateQuery("select max(ServiceFirstTicketPriorityIndex) from ServiceFirstTicketPriority").List();
                    var maxIndex = !string.IsNullOrEmpty(indexData[0].ToString()) ? Convert.ToInt64(indexData[0].ToString()) + 1 : 1;
                    _objCatData.ServiceFirstTicketPriorityIndex = maxIndex;
                    using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                    {
                        if (_objCatData.ServiceFirstTicketPriorityID == 0)
                        {
                            session.Save(_objCatData); //  Save the book in session
                        }
                        else
                        {
                            var data = session.Query<ServiceFirstTicketType>().Where(x => x.ServiceFirstTickeTypeID ==
                       _objCatData.ServiceFirstTicketPriorityID).FirstOrDefault();
                            if (string.IsNullOrEmpty(_objCatData.ServiceFirstTicketPriorityIcon))
                            {
                                _objCatData.ServiceFirstTicketPriorityIcon = data.ServiceFirstTicketTypeIcon;
                            }
                            session.Clear();
                            session.SaveOrUpdate(_objCatData); //  Save the book in session
                        }
                        transaction.Commit();   //  Commit the changes to the database
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public List<ServiceFirstTicketPriority> GetTicketPriorities()
        {
            List<ServiceFirstTicketPriority> mainList = new List<Models.ServiceFirstTicketPriority>();
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                try
                {
                    var companyId = Convert.ToInt64((HttpContext.Current.Session["CManagerCompanyId"]
                    .ToString()));
                    mainList = session.Query<ServiceFirstTicketPriority>().Where(x => x.ServiceFirstCompanyID == companyId).ToList();
                }
                catch (Exception)
                {

                }
            }
            return mainList;
        }

        #endregion

        #region Ticket Status
        public ServiceFirstTicketStatus GetEditTicketStatus(long? id)
        {
            ServiceFirstTicketStatus Tickets;
            using (ISession session = NHibernateSession.OpenSession())
            {
                Tickets = session.Query<ServiceFirstTicketStatus>().Where(b => b.ServiceFirstTicketStatusID
                == id).FirstOrDefault();
            }
            return Tickets;
        }
        public bool AddEditTicketStatus(ServiceFirstTicketStatus _objTicketStatus)
        {
            bool status = false;
            try
            {
                _objTicketStatus.ServiceFirstCompanyID = (HttpContext.Current.Session["CManagerCompanyId"].ToString());
                _objTicketStatus.ServiceFirstTicketStatusCreatedDate = DateTime.Now;
                _objTicketStatus.ServiceFirstTicketStatusIsActive = true;
                using (ISession session = NHibernateSession.OpenSession())
                {

                    using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                    {
                        if (_objTicketStatus.ServiceFirstTicketStatusID == 0)
                        {
                            session.Save(_objTicketStatus); //  Save the book in session
                        }
                        else
                        {
                            var _ticketStatus = session.Query<ServiceFirstTicketStatus>().Where(b => b.ServiceFirstTicketStatusID
                         == _objTicketStatus.ServiceFirstTicketStatusID).FirstOrDefault();
                            if (string.IsNullOrEmpty(_objTicketStatus.ServiceFirstTicketStatusIcon))
                            {
                                _objTicketStatus.ServiceFirstTicketStatusIcon = _ticketStatus.ServiceFirstTicketStatusIcon;
                            }
                            session.Clear();
                            session.SaveOrUpdate(_objTicketStatus); //  Save the book in session
                        }
                        transaction.Commit();   //  Commit the changes to the database
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public List<ServiceFirstTicketStatus> GetTicketStatus()
        {
            List<ServiceFirstTicketStatus> TicketStatusList = new List<ServiceFirstTicketStatus>();
            try
            {
                using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
                {
                    try
                    {
                        TicketStatusList = session.Query<ServiceFirstTicketStatus>().ToList();
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
            return TicketStatusList;
        }
        public bool ActiveInActiveTicketStatus(int? id)
        {
            bool status = false;
            try
            {
                ServiceFirstTicketStatus data;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    data = session.Query<ServiceFirstTicketStatus>().Where(b => b.ServiceFirstTicketStatusID
                    == id).FirstOrDefault();
                    if (data.ServiceFirstTicketStatusIsActive)
                    {
                        data.ServiceFirstTicketStatusIsActive = false;
                    }
                    else
                    {
                        data.ServiceFirstTicketStatusIsActive = true;
                    }
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(data);
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }

        #endregion

        #region User Groups
        public ServiceFirstUserGroup GetEditUserGroups(long? id)
        {
            ServiceFirstUserGroup userGroups;
            using (ISession session = NHibernateSession.OpenSession())
            {
                userGroups = session.Query<ServiceFirstUserGroup>().Where(b => b.ServiceFirstGroupID
                == id).FirstOrDefault();
            }
            return userGroups;
        }
        public bool AddEditUserGroups(ServiceFirstUserGroup _objUserGroup)
        {
            bool status = false;
            try
            {
                _objUserGroup.ServiceFirstCompanyID = (HttpContext.Current.Session["CManagerCompanyId"].ToString());
                _objUserGroup.ServiceFirstGroupCreatedDate = DateTime.Now;
                _objUserGroup.ServiceFirstGroupIsActive = true;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        if (_objUserGroup.ServiceFirstGroupID == 0)
                        {
                            session.Save(_objUserGroup);
                        }
                        else
                        {
                            session.SaveOrUpdate(_objUserGroup);
                        }
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public List<ServiceFirstUserGroup> GetUserGroups()
        {
            List<ServiceFirstUserGroup> UserGroupList = new List<ServiceFirstUserGroup>();
            try
            {
                using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
                {
                    try
                    {
                        UserGroupList = session.Query<ServiceFirstUserGroup>().ToList();
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
            return UserGroupList;
        }
        public bool ActiveInActiveUserGroup(int? Id)
        {
            bool status = false;
            try
            {
                ServiceFirstUserGroup data;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    data = session.Query<ServiceFirstUserGroup>().Where(b => b.ServiceFirstGroupID
                    == Id).FirstOrDefault();
                    if (data.ServiceFirstGroupIsActive)
                    {
                        data.ServiceFirstGroupIsActive = false;
                    }
                    else
                    {
                        data.ServiceFirstGroupIsActive = true;
                    }
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(data);
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }

        #endregion

        #region Ticket Resolution
        public ServiceFirstTicketResolution GetEditTicketResolution(long? id)
        {
            ServiceFirstTicketResolution TicketResolution;
            using (ISession session = NHibernateSession.OpenSession())
            {
                TicketResolution = session.Query<ServiceFirstTicketResolution>().Where(b => b.ServiceFirstTicketResolutionID
                == id).FirstOrDefault();
            }
            return TicketResolution;
        }

        public bool AddEditTicketResolution(ServiceFirstTicketResolution _objTicketResolution)
        {
            bool status = false;
            try
            {
                _objTicketResolution.ServiceFirstCompanyID = (HttpContext.Current.Session["CManagerCompanyId"]
                    .ToString());
                _objTicketResolution.ServiceFirstTicketResolutionDateCreated = DateTime.Now;
                _objTicketResolution.ServiceFirstTicketResolutionIsActive = true;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    var indexData = session.CreateQuery("select max(ServicFirstTicketResolutionIndex) from ServiceFirstTicketResolution").List();
                    var maxIndex = !string.IsNullOrEmpty(indexData[0].ToString()) ? Convert.ToInt64(indexData[0].ToString()) + 1 : 1;
                    _objTicketResolution.ServicFirstTicketResolutionIndex = maxIndex;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        if (_objTicketResolution.ServiceFirstTicketResolutionID == 0)
                        {
                            session.Save(_objTicketResolution);
                        }
                        else
                        {
                            var data = session.Query<ServiceFirstTicketResolution>().Where(x => x.ServiceFirstTicketResolutionID ==
                     _objTicketResolution.ServiceFirstTicketResolutionID).FirstOrDefault();
                            if (string.IsNullOrEmpty(_objTicketResolution.ServiceFirstTicketResolutionIcon))
                            {
                                _objTicketResolution.ServiceFirstTicketResolutionIcon = data.ServiceFirstTicketResolutionIcon;
                            }
                            session.Clear();
                            session.SaveOrUpdate(_objTicketResolution);
                        }
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }

        public List<ServiceFirstTicketResolution> GetTicketResolution()
        {
            List<ServiceFirstTicketResolution> TicketResolutionList = new List<ServiceFirstTicketResolution>();
            try
            {
                using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
                {
                    try
                    {
                        TicketResolutionList = session.Query<ServiceFirstTicketResolution>().ToList();
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
            return TicketResolutionList;
        }
        public bool InActiveTicketResolution(int? Id)
        {
            bool status = false;
            try
            {
                ServiceFirstTicketResolution data;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    data = session.Query<ServiceFirstTicketResolution>().Where(b => b.ServiceFirstTicketResolutionID
                    == Id).FirstOrDefault();
                    if (data.ServiceFirstTicketResolutionIsActive)
                    {
                        data.ServiceFirstTicketResolutionIsActive = false;
                    }
                    else
                    {
                        data.ServiceFirstTicketResolutionIsActive = true;
                    }
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(data);
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }

        #endregion

        #region Customers
        public ServiceFirstCustomers GetEditCustomersList(long? id)
        {
            ServiceFirstCustomers customersList;
            using (ISession session = NHibernateSession.OpenSession())
            {
                customersList = session.Query<ServiceFirstCustomers>().Where(b => b.ServiceFirstCustomerID
                == id).FirstOrDefault();
            }
            return customersList;
        }
        public bool AddEditCustomers(ServiceFirstCustomers _objCustomers)
        {
            bool status = false;
            try
            {
                _objCustomers.ServiceFirstCompanyID = Convert.ToInt64((HttpContext.Current.Session["CManagerCompanyId"]));
                _objCustomers.ServiceFirstCustomerCreatedDate = DateTime.Now;
                _objCustomers.ServiceFirstCustomerIsActive = true;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        if (_objCustomers.ServiceFirstCustomerID == 0)
                        {
                            session.Save(_objCustomers);
                        }
                        else
                        {
                            session.SaveOrUpdate(_objCustomers);
                        }
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public List<ServiceFirstCustomers> GetCustomersList()
        {
            List<ServiceFirstCustomers> CustomersList = new List<ServiceFirstCustomers>();
            try
            {
                using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
                {
                    try
                    {
                        CustomersList = session.Query<ServiceFirstCustomers>().ToList();
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
            return CustomersList;
        }
        public bool ActiveInActiveCustomers(int? Id)
        {
            bool status = false;
            try
            {
                ServiceFirstCustomers data;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    data = session.Query<ServiceFirstCustomers>().Where(b => b.ServiceFirstCustomerID
                    == Id).FirstOrDefault();
                    if (data.ServiceFirstCustomerIsActive)
                    {
                        data.ServiceFirstCustomerIsActive = false;
                    }
                    else
                    {
                        data.ServiceFirstCustomerIsActive = true;
                    }
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(data);
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public bool UploadCustomers(List<ServiceFirstCustomers> lstData)
        {
            bool status = false;
            try
            {
                foreach (var item in lstData)
                {
                    item.ServiceFirstCompanyID = Convert.ToInt64((HttpContext.Current.Session["CManagerCompanyId"]
                    .ToString()));
                    item.ServiceFirstCustomerCreatedDate = DateTime.Now;
                    item.ServiceFirstCustomerIsActive = true;
                    using (ISession session = NHibernateSession.OpenSession())
                    {
                        var validCustomeNoData = session.Query<ServiceFirstCustomers>().Where(x => x.ServiceFirstCustomerNumber
                        == item.ServiceFirstCustomerNumber && x.ServiceFirstCompanyID == item.ServiceFirstCompanyID).FirstOrDefault();
                        if (validCustomeNoData == null)
                        {
                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Save(item);
                                transaction.Commit();
                            }
                        }
                        else
                        {
                            Message = validCustomeNoData.ServiceFirstCustomerNumber + " Customer Number is already exist in database.";
                            status = false;
                            return status;
                        }
                    }
                }

                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }

        #endregion

        #region Projects

        public ServiceFirstProjects GetEditProjectsList(long? id)
        {
            ServiceFirstProjects projectsList;
            using (ISession session = NHibernateSession.OpenSession())
            {
                projectsList = session.Query<ServiceFirstProjects>().Where(b => b.ServiceFirstProjectID
                == id).FirstOrDefault();
            }
            return projectsList;
        }
        public bool AddEditProjects(ServiceFirstProjects _objProjects)
        {
            bool status = false;
            try
            {
                _objProjects.ServiceFirstCompanyID = Convert.ToInt64((HttpContext.Current.Session["CManagerCompanyId"]));
                _objProjects.ServiceFirstProjectCreatedDate = DateTime.Now;
                _objProjects.ServiceFirstProjectIsActive = true;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        if (_objProjects.ServiceFirstProjectID == 0)
                        {
                            session.Save(_objProjects);
                        }
                        else
                        {
                            session.SaveOrUpdate(_objProjects);
                        }
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public List<ServiceFirstProjectsVM> GetProjectsList()
        {
            List<ServiceFirstProjects> ProjectsList = new List<ServiceFirstProjects>();
            List<ServiceFirstProjectsVM> _NewProjectsList = new List<ServiceFirstProjectsVM>();
            try
            {
                using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
                {
                    try
                    {
                        ProjectsList = session.Query<ServiceFirstProjects>().ToList();
                        if (ProjectsList != null)
                        {
                            foreach (var item in ProjectsList)
                            {
                                var getCustomerName = session.Query<ServiceFirstCustomers>().Where(a => a.ServiceFirstCustomerID == item.ServiceFirstProjectCustomer).FirstOrDefault();
                                var getOwnerName = session.Query<ServiceFirst_UserLogin>().Where(a => a.ServiceFirstUserID == item.ServiceFirstProjectOwner).FirstOrDefault();
                                var CustomerName = getCustomerName != null ? getCustomerName.ServiceFirstCustomerName : String.Empty;
                                var OwnerName = getOwnerName != null ? getOwnerName.ServiceFirstUserName : String.Empty;
                                _NewProjectsList.Add(new ServiceFirstProjectsVM { ServiceFirstProjectName = item.ServiceFirstProjectName, ServiceFirstCompanyID = item.ServiceFirstCompanyID, ServiceFirstProjectCustomer = CustomerName, ServiceFirstProjectDueDate = item.ServiceFirstProjectDueDate, ServiceFirstProjectID = item.ServiceFirstProjectID, ServiceFirstProjectIsActive = item.ServiceFirstProjectIsActive, ServiceFirstProjectOwner = OwnerName });

                            }

                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
            return _NewProjectsList;
        }
        public bool ActiveInActiveProjects(int? Id)
        {
            bool status = false;
            try
            {
                ServiceFirstProjects data;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    data = session.Query<ServiceFirstProjects>().Where(b => b.ServiceFirstProjectID
                    == Id).FirstOrDefault();
                    if (data.ServiceFirstProjectIsActive)
                    {
                        data.ServiceFirstProjectIsActive = false;
                    }
                    else
                    {
                        data.ServiceFirstProjectIsActive = true;
                    }
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(data);
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }

        #endregion

        #region Users
        public bool AddEditUser(ServiceFirst_UserLogin _objData)
        {
            bool status = false;
            try
            {
                CommonFunctions.GeneratePassword(_objData.ServiceFirstUserPassword,
             "new", ref _salt, ref _password);

                _objData.ServiceFirstCompanyID = Convert.ToInt64((HttpContext.Current.Session["CManagerCompanyId"]
                    .ToString()));
                _objData.ServiceFirstUserCreatedDate = DateTime.Now;
                _objData.ServiceFirstUserIsActive = true;
                _objData.ServiceFirstUserPassword = _password;
                _objData.ServiceFirstUserPassworSalt = _salt;
                using (ISession session = NHibernateSession.OpenSession())
                {

                    using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                    {
                        if (_objData.ServiceFirstUserID == 0)
                        {
                            session.Save(_objData); //  Save the book in session
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(_objData.ServiceFirstUserPicture))
                            {
                                var data = session.Query<ServiceFirst_UserLogin>().Where(x => x.ServiceFirstUserID ==
                          _objData.ServiceFirstUserID).FirstOrDefault();
                                _objData.ServiceFirstUserPicture = data.ServiceFirstUserPicture;
                            }
                            session.Clear();
                            session.SaveOrUpdate(_objData); //  Save the book in session
                        }
                        transaction.Commit();   //  Commit the changes to the database
                    }
                }
                CommonFunctions.SendMail(_objData.ServiceFirstUserContactEmail,
                   _objData.ServiceFirstUserName, _objData.ServiceFirstUserConfirmPassword,
                   "Company Users");
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public List<ServiceFirst_UserLoginVM> GetUsersList()
        {
            List<ServiceFirst_UserLogin> usersList = new List<ServiceFirst_UserLogin>();
            List<ServiceFirst_UserLoginVM> usersListDisplay = new List<ServiceFirst_UserLoginVM>();
            try
            {
                using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
                {
                    try
                    {
                        usersList = session.Query<ServiceFirst_UserLogin>().ToList();
                        if (usersList != null)
                        {
                            foreach (var item in usersList)
                            {
                                var getGroupName = session.Query<ServiceFirstUserGroup>().Where(a => a.ServiceFirstGroupID == item.ServiceFirstUserGroupID).FirstOrDefault();
                                var GroupName = getGroupName != null ? getGroupName.ServiceFirstGroupName : String.Empty;
                                usersListDisplay.Add(new ServiceFirst_UserLoginVM {ServiceFirstUserContactEmail=item.ServiceFirstUserContactEmail,ServiceFirstUserContactNumber=item.ServiceFirstUserContactNumber, ServiceFirstUserFirstName = item.ServiceFirstUserFirstName, ServiceFirstUserID = item.ServiceFirstUserID, ServiceFirstUserLastName = item.ServiceFirstUserLastName, ServiceFirstUserName = item.ServiceFirstUserName, ServiceFirstUserPicture = item.ServiceFirstUserPicture, ServiceFirstUserGroupID = GroupName ,ServiceFirstUserIsActive=item.ServiceFirstUserIsActive});

                            }

                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
            return usersListDisplay;
        }
        public bool ActiveInActiveUser(int? Id)
        {
            bool status = false;
            try
            {
                ServiceFirst_UserLogin data;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    data = session.Query<ServiceFirst_UserLogin>().Where(b => b.ServiceFirstUserID
                    == Id).FirstOrDefault();
                    if (data.ServiceFirstUserIsActive)
                    {
                        data.ServiceFirstUserIsActive = false;
                    }
                    else
                    {
                        data.ServiceFirstUserIsActive = true;
                    }
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(data);
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public ServiceFirst_UserLogin GetUserById(long? id)
        {
            ServiceFirst_UserLogin userData;
            using (ISession session = NHibernateSession.OpenSession())
            {
                userData = session.Query<ServiceFirst_UserLogin>().Where(b => b.ServiceFirstUserID
                == id).FirstOrDefault();
            }
            return userData;
        }

        #endregion
        public SelectList GetCustomersSelectList()
        {
            List<ServiceFirstCustomers> customers = new List<ServiceFirstCustomers>();
            SelectList selectList = null;
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                customers = session.Query<ServiceFirstCustomers>().ToList();
            }

            if (customers != null)
            {
                selectList = new SelectList(customers.ToArray(),
                                "ServiceFirstCustomerID",
                                "ServiceFirstCustomerName");
            }
            return selectList;
        }

        public SelectList GetUsersSelectList()
        {
            List<ServiceFirst_UserLogin> users = new List<ServiceFirst_UserLogin>();
            SelectList selectList = null;
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                users = session.Query<ServiceFirst_UserLogin>().ToList();
            }

            if (users != null)
            {
                selectList = new SelectList(users.ToArray(),
                                "ServiceFirstUserID",
                                "ServiceFirstUserName");
            }
            return selectList;
        }
        public SelectList GetContactPersonsSelectList()
        {
            List<ServiceFirst_Login> users = new List<ServiceFirst_Login>();
            SelectList selectList = null;
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                users = session.Query<ServiceFirst_Login>().ToList();
            }

            if (users != null)
            {
                selectList = new SelectList(users.ToArray(),
                                "ServiceFirstID",
                                "ServiceFirstName");
            }
            return selectList;
        }
        public SelectList GetProjectsSelectList()
        {
            List<ServiceFirstProjects> projects = new List<ServiceFirstProjects>();
            SelectList selectList = null;
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                projects = session.Query<ServiceFirstProjects>().ToList();
            }

            if (projects != null)
            {
                selectList = new SelectList(projects.ToArray(),
                                "ServiceFirstProjectID",
                                "ServiceFirstProjectName");
            }
            return selectList;
        }
        public SelectList GetTicketTypesSelectList()
        {
            List<ServiceFirstTicketType> data = new List<ServiceFirstTicketType>();
            SelectList selectList = null;
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                data = session.Query<ServiceFirstTicketType>().ToList();
            }

            if (data != null)
            {
                selectList = new SelectList(data.ToArray(),
                                "ServiceFirstTickeTypeID",
                                "ServiceFirstTicketTypeName");
            }
            return selectList;
        }
        public SelectList GetTicketPrioritySelectList()
        {
            List<ServiceFirstTicketPriority> data = new List<ServiceFirstTicketPriority>();
            SelectList selectList = null;
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                data = session.Query<ServiceFirstTicketPriority>().ToList();
            }

            if (data != null)
            {
                selectList = new SelectList(data.ToArray(),
                                "ServiceFirstTicketPriorityID",
                                "ServiceFirstTicketPriorityName");
            }
            return selectList;
        }
        public SelectList GetTicketStatusSelectList()
        {
            List<ServiceFirstTicketStatus> data = new List<ServiceFirstTicketStatus>();
            SelectList selectList = null;
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                data = session.Query<ServiceFirstTicketStatus>().ToList();
            }

            if (data != null)
            {
                selectList = new SelectList(data.ToArray(),
                                "ServiceFirstTicketStatusID",
                                "ServiceFirstTicketStatusName");
            }
            return selectList;
        }
        public SelectList GetTicketResolutionSelectList()
        {
            List<ServiceFirstTicketResolution> data = new List<ServiceFirstTicketResolution>();
            SelectList selectList = null;
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                data = session.Query<ServiceFirstTicketResolution>().ToList();
            }

            if (data != null)
            {
                selectList = new SelectList(data.ToArray(),
                                "ServiceFirstTicketResolutionID",
                                "ServiceFirstTicketResolutionName");
            }
            return selectList;
        }
       
        public SelectList GetUserGroupsSelectList()
        {
            List<ServiceFirstUserGroup> userGroups = new List<ServiceFirstUserGroup>();
            SelectList selectList = null;
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                userGroups = session.Query<ServiceFirstUserGroup>().ToList();
            }

            if (userGroups != null)
            {
                selectList = new SelectList(userGroups.ToArray(),
                                "ServiceFirstGroupID",
                                "ServiceFirstGroupName");
            }
            return selectList;
        }

        public static void ExportToExcel(List<ExportData> data, string sheetName)
        {
            XLWorkbook wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(sheetName);
            ws.Cell(2, 1).InsertTable(data);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xlsx", sheetName.Replace(" ", "_")));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                memoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                memoryStream.Close();
            }

            HttpContext.Current.Response.End();
        }

        #region Contact Persons

        public ServiceFirst_Login GetEditContactPersons(long? id)
        {
            ServiceFirst_Login ContactPersons;
            using (ISession session = NHibernateSession.OpenSession())
            {
                ContactPersons = session.Query<ServiceFirst_Login>().Where(b => b.ServiceFirstID
                == id).FirstOrDefault();
            }
            return ContactPersons;
        }

        public bool AddEditContactPersons(ServiceFirst_Login _objContactPersons)
        {
            bool status = false;
            try
            {
                CommonFunctions.GeneratePassword(_objContactPersons.ServiceFirstPassword,
            "new", ref _salt, ref _password);

                _objContactPersons.ServiceFirstCompanyID = HttpContext.Current.Session["CManagerCompanyId"].ToString();
                _objContactPersons.ContactPersonsListCreatedDate = DateTime.Now;
                _objContactPersons.ContactPersonsListIsActive = true;
                _objContactPersons.ServiceFirstPassword = _password;
                _objContactPersons.ServiceFirstPassworSalt = _salt;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        if (_objContactPersons.ServiceFirstID == 0)
                        {
                            session.Save(_objContactPersons);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(_objContactPersons.ServiceFirstUserPicture))
                            {
                                var data = session.Query<ServiceFirst_Login>().Where(x => x.ServiceFirstID ==
                          _objContactPersons.ServiceFirstID).FirstOrDefault();
                                _objContactPersons.ServiceFirstUserPicture = data.ServiceFirstUserPicture;
                            }
                            session.Clear();

                            session.SaveOrUpdate(_objContactPersons);
                        }
                        transaction.Commit();
                    }
                }
                CommonFunctions.SendMail(_objContactPersons.ServiceFirstUserContactEmail,
   _objContactPersons.ServiceFirstUserName, _objContactPersons.ServiceFirstUserConfirmPassword,
   "Company Contact Persons");
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }

        public List<ServiceFirst_LoginVM> GetContactPersons()
        {
            List<ServiceFirst_Login> ServiceFirst_UserLoginList = new List<ServiceFirst_Login>();
            List<ServiceFirst_LoginVM> ServicesList_Display = new List<ServiceFirst_LoginVM>();
            try
            {
                using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
                {
                    try
                    {
                        ServiceFirst_UserLoginList = session.Query<ServiceFirst_Login>().ToList();
                        if (ServiceFirst_UserLoginList != null)
                        {
                            foreach (var item in ServiceFirst_UserLoginList)
                            {
                                var getCustomerName = session.Query<ServiceFirstCustomers>().Where(a => a.ServiceFirstCustomerID == item.ServiceFirstCompanyCustomersID).FirstOrDefault();                                
                                var CustomerName = getCustomerName != null ? getCustomerName.ServiceFirstCustomerName : String.Empty;
                                ServicesList_Display.Add(new ServiceFirst_LoginVM { ServiceFirstUserFirstName = item.ServiceFirstUserFirstName, ServiceFirstID = item.ServiceFirstID, ServiceFirstUserLastName = item.ServiceFirstUserLastName, ServiceFirstUserName = item.ServiceFirstUserName, ServiceFirstUserPicture = item.ServiceFirstUserPicture, ContactPersonsListIsActive = item.ContactPersonsListIsActive, ServiceFirstCompanyCustomersID = CustomerName });

                            }

                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
            return ServicesList_Display;
        }
        public bool InActiveContactPersons(int? Id)
        {
            bool status = false;
            try
            {
                ServiceFirst_Login data;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    data = session.Query<ServiceFirst_Login>().Where(b => b.ServiceFirstID
                    == Id).FirstOrDefault();
                    if (data.ContactPersonsListIsActive)
                    {
                        data.ContactPersonsListIsActive = false;
                    }
                    else
                    {
                        data.ContactPersonsListIsActive = true;
                    }
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(data);
                        transaction.Commit();
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }

        #endregion


        public bool AddEditTicket(ServiceFirstTickets _objData)
        {
            /*INSERT INTO ServiceFirstTickets (ServiceFirstCompanyID, ServiceFirstCompanyCustomerID, 
             * ServiceFirstCompanyCustomersProjectID, SeviceFirstTicketName, ServiceFirstTicetCreatedDate,
             *  ServiceFirstTicketLastChanged, ServiceFirstTicketReporter, ServiceFirstTicketType, ServiceFirstTicketPriority,
             *   ServiceFirstTicketStatus, ServiceFirstTicketResolution, ServiceFirstTicketDescription, 
             *   ServiceFirstTicketAssigneeID) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?); select SCOPE_IDENTITY()*/
            bool status = false;
            try
            {
                _objData.ServiceFirstCompanyID = Convert.ToInt64((HttpContext.Current.Session["CManagerCompanyId"]
                    .ToString()));
                using (ISession session = NHibernateSession.OpenSession())
                {

                    using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                    {
                        if (_objData.ServiceFirstTicketID == 0)
                        {
                            _objData.ServiceFirstTicetCreatedDate = DateTime.Now;
                            _objData.ServiceFirstTicketLastChanged = DateTime.Now;
                            session.Save(_objData); //  Save the book in session
                        }
                        else
                        {
                            var _oldData = session.Query<ServiceFirstTickets>().Where(x => x.ServiceFirstTicketID
                            == _objData.ServiceFirstTicketID).FirstOrDefault();
                            _objData.ServiceFirstTicetCreatedDate = _oldData.ServiceFirstTicetCreatedDate;
                            session.Clear();
                            _objData.ServiceFirstTicketLastChanged = DateTime.Now;
                            session.SaveOrUpdate(_objData); //  Save the book in session
                        }
                        transaction.Commit();   //  Commit the changes to the database
                    }
                }
               
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }
        public ServiceFirstTickets GetTicketById(long? id)
        {
            ServiceFirstTickets ticketData;
            using (ISession session = NHibernateSession.OpenSession())
            {
                ticketData = session.Query<ServiceFirstTickets>().Where(b => b.ServiceFirstTicketID
                == id).FirstOrDefault();
            }
            return ticketData;
        }

    
        public List<TicketViewModel  > GetTicketList()
        {
            List<TicketViewModel> usersList = new List<TicketViewModel>();
            try
            {
                using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
                {
                    try
                    {
                        var com_id = Convert.ToInt64(HttpContext.Current.Session["CManagerCompanyId"]);
//                        string queryy= "select  ServiceFirstTicketID as TicketID,SeviceFirstTicketName as Name," +
//"ServiceFirstTicketDescription as TicketDescription, ServiceFirstCustomers.ServiceFirstCustomerName as Customer," +
// "ServiceFirstProjects.ServiceFirstProjectName as Project " +
//"from ServiceFirstTickets inner join ServiceFirstCustomers on ServiceFirstTickets.ServiceFirstCompanyCustomerID" +
//"= ServiceFirstCustomers.ServiceFirstCustomerID left " +
//"join ServiceFirstProjects on ServiceFirstTickets.ServiceFirstCompanyCustomersProjectID" +
//"= ServiceFirstProjects.ServiceFirstProjectID  where ServiceFirstTickets.ServiceFirstCompanyID = " + com_id;
//                        var data = session.CreateQuery(queryy).List();
                       
                        var data = session.Query<ServiceFirstTickets>().Where(x=>x.ServiceFirstCompanyID== com_id).ToList();
                        foreach (var item in data)
                        {
                            var project = session.Query<ServiceFirstProjects>().Where(x => x.ServiceFirstProjectID ==
                               item.ServiceFirstCompanyCustomersProjectID).FirstOrDefault();
                            usersList.Add(new TicketViewModel
                            {
                                ServiceFirstTicketID=item.ServiceFirstTicketID,
                                SeviceFirstTicketName = item.SeviceFirstTicketName,
                                ServiceFirstTicketDescription = item.ServiceFirstTicketDescription ,
                                Customer = session.Query<ServiceFirstCustomers >().Where(x => x.ServiceFirstCustomerID == 
                                item.ServiceFirstCompanyCustomerID).FirstOrDefault().ServiceFirstCustomerName,
                                Project =project !=null && !string.IsNullOrEmpty(project.ServiceFirstProjectName) 
                                ? project.ServiceFirstProjectName :"",
                            });
                        }
                    }
                    catch(Exception ex  )
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
            return usersList;
        }
    }
}