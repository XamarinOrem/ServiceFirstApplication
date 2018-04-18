using NHibernate;
using NHibernate.Linq;
using ServiceFirstApplication.Models;
using ServiceFirstApplication.ViewModels;
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
    public class AdminUsers
    {
        #region Properties/data members

        public ServiceFirst_AdminLogin UserObj { get; private set; }


        // public RM_Registration NormalUserObj { get; private set; }



        public string Message { get; private set; }

        string _salt, _password = string.Empty;
        public string currentUserType { get; set; }

        #endregion

        #region Constructors

        public AdminUsers()
        {

        }

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

        public bool AddCompany(ServiceFirstCompanies _objCompany)
        {
            bool status = false;
            try
            {
                _objCompany.ServiceFirstCompanyCreatedDate = DateTime.Now;
                _objCompany.ServiceFirstCompanyIsActive = true;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(_objCompany);
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
                CommonFunctions.SendMail(_objCompanyManager.ServiceFirstCompanyManagerContactEmail,
                    _objCompanyManager.ServiceFirstCompanyManagerUserName, _objCompanyManager.ServiceFirstCompanyManagerConfirmPassword,
                    "Company Manager");
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }

            return status;
        }

        public bool EditCompany(ServiceFirstCompanies _objCompany)
        {
            bool status = false;
            try
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    ServiceFirstCompanies CompanyData;

                    CompanyData = session.Query<ServiceFirstCompanies>().Where(b => b.ServiceFirstCompanyID
                        == _objCompany.ServiceFirstCompanyID).FirstOrDefault();
                    if (string.IsNullOrEmpty(_objCompany.ServiceFirstCompanyLogoFile))
                    {
                        _objCompany.ServiceFirstCompanyLogoFile = CompanyData.ServiceFirstCompanyLogoFile;
                    }
                    _objCompany.ServiceFirstCompanyIsActive = CompanyData.ServiceFirstCompanyIsActive;
                    _objCompany.ServiceFirstCompanyCreatedDate = DateTime.Now;
                    session.Clear();
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(_objCompany);
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

        public bool EditCompanyManager(ServiceFirstCompanyManager _objCompanyManager)
        {
            bool status = false;
            try
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    ServiceFirstCompanyManager CompanyData;
                    CompanyData = session.Query<ServiceFirstCompanyManager>().Where(b => b.ServiceFirstCompanyManagerID
                        == _objCompanyManager.ServiceFirstCompanyManagerID).FirstOrDefault();
                    if (string.IsNullOrEmpty(_objCompanyManager.ServiceFirstCompanyManagerLogoFile))
                    {
                        _objCompanyManager.ServiceFirstCompanyManagerLogoFile = CompanyData.ServiceFirstCompanyManagerLogoFile;
                    }
                    _objCompanyManager.ServiceFirstCompanyManagerIsActive = CompanyData.ServiceFirstCompanyManagerIsActive;
                    _objCompanyManager.ServiceFirstCompanyManagerCreatedDate = DateTime.Now;
                    string _salt = string.Empty;
                    string _password = string.Empty;
                    CommonFunctions.GeneratePassword(_objCompanyManager.ServiceFirstCompanyManagerPassword,
        "new", ref _salt, ref _password);

                    _objCompanyManager.ServiceFirstCompanyManagerPassword = _password;
                    _objCompanyManager.ServiceFirstCompanyManagerPasswordSalt = _salt;
                    session.Clear();
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(_objCompanyManager);
                        transaction.Commit();
                    }
                }
                CommonFunctions.SendMail(_objCompanyManager.ServiceFirstCompanyManagerContactEmail,
                      _objCompanyManager.ServiceFirstCompanyManagerUserName, _objCompanyManager.ServiceFirstCompanyManagerConfirmPassword,
                      "Company Manager");
                status = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                status = false;
            }
            return status;
        }

        public ServiceFirstCompanies GetEditCompanyData(long? id)
        {
            ServiceFirstCompanies Company;
            using (ISession session = NHibernateSession.OpenSession())
            {
                Company = session.Query<ServiceFirstCompanies>().Where(b => b.ServiceFirstCompanyID
                == id).FirstOrDefault();
            }
            return Company;
        }

        public ServiceFirstCompanyManager GetEditCompanyManagerData(long? id)
        {
            ServiceFirstCompanyManager CompanyManager;
            using (ISession session = NHibernateSession.OpenSession())
            {
                CompanyManager = session.Query<ServiceFirstCompanyManager>().Where(b => b.ServiceFirstCompanyManagerID == id).FirstOrDefault();
            }
            return CompanyManager;
        }

        public bool InActiveCompany(int? id)
        {
            bool status = false;
            try
            {
                ServiceFirstCompanies company;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    company = session.Query<ServiceFirstCompanies>().Where(b => b.ServiceFirstCompanyID == id).FirstOrDefault();
                    company.ServiceFirstCompanyIsActive = false;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(company);
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

        public bool ActiveCompany(int? Id)
        {
            bool status = false;
            try
            {
                ServiceFirstCompanies company;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    company = session.Query<ServiceFirstCompanies>().Where(b => b.ServiceFirstCompanyID == Id).FirstOrDefault();
                    company.ServiceFirstCompanyIsActive = true;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(company);
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

        public bool InActiveCompanyManager(int? Id)
        {
            bool status = false;
            try
            {
                ServiceFirstCompanyManager companyManager;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    companyManager = session.Query<ServiceFirstCompanyManager>().Where(b => b.ServiceFirstCompanyManagerID
                    == Id).FirstOrDefault();
                    companyManager.ServiceFirstCompanyManagerIsActive = false;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(companyManager);
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

        public bool ActiveCompanyManager(int? Id)
        {
            bool status = false;
            try
            {
                ServiceFirstCompanyManager companyManager;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    companyManager = session.Query<ServiceFirstCompanyManager>().Where(b => b.ServiceFirstCompanyManagerID
                    == Id).FirstOrDefault();
                    companyManager.ServiceFirstCompanyManagerIsActive = true;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(companyManager);
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

        public List<CompaniesViewModel> GetCompanies()
        {
            List<CompaniesViewModel> _CompaniesData = new List<CompaniesViewModel>();
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                List<ServiceFirstCompanies> CompaniesList = new List<Models.ServiceFirstCompanies>();
                List<ServiceFirstCompanyManager> ManagerList = new List<ServiceFirstCompanyManager>();
                try
                {
                    CompaniesList = session.Query<ServiceFirstCompanies>().ToList();
                    ManagerList = session.Query<ServiceFirstCompanyManager>().ToList();
                    string Projects = String.Empty;
                    string Tickets = String.Empty;
                    string Clients = String.Empty;
                    string ManagerName = String.Empty;
                    string ManagerId = String.Empty;

                    var data = CompaniesList.GroupJoin(ManagerList, company => company.ServiceFirstCompanyID, manager => manager.ServiceFirstCompanyManagerCompanyID,
                                                      (x, y) => new { CompaniesList = x, ManagerList = y }).SelectMany(
                                                       x => x.ManagerList.DefaultIfEmpty(), (x, y) => new { Companies = x.CompaniesList, Managers = y });
                    foreach (var item in data)
                    {
                        Clients = item.Companies.ServiceFirstCompanyNoOfCustomers != 0 ? "0/" + item.Companies.ServiceFirstCompanyNoOfCustomers : "0/∞";
                        Projects = item.Companies.ServiceFirstCompanyNoOfProjects != 0 ? "0/" + item.Companies.ServiceFirstCompanyNoOfProjects : "0/∞";
                        Tickets = item.Companies.ServiceFirstCompanyNoOfTickets != 0 ? "0/" + item.Companies.ServiceFirstCompanyNoOfTickets : "0/∞";
                        ManagerName = item.Managers != null ? item.Managers.ServiceFirstCompanyManagerName : String.Empty;
                        ManagerId = item.Managers != null ? item.Managers.ServiceFirstCompanyManagerID.ToString() : String.Empty;

                        _CompaniesData.Add(new CompaniesViewModel
                        {
                            ServiceFirstCompanyID = item.Companies.ServiceFirstCompanyID,
                            ServiceFirstCompanyAddress = item.Companies.ServiceFirstCompanyAddress,
                            ServiceFirstCompanyContactEmail = item.Companies.ServiceFirstCompanyContactEmail,
                            ServiceFirstCompanyIsActive = item.Companies.ServiceFirstCompanyIsActive,
                            ServiceFirstCompanyLogoFile = item.Companies.ServiceFirstCompanyLogoFile,
                            ServiceFirstCompanyManagerName = ManagerName,
                            ServiceFirstCompanyName = item.Companies.ServiceFirstCompanyName,
                            ServiceFirstCompanyNoOfCustomers = Clients,
                            ServiceFirstCompanyNoOfProjects = Projects,
                            ServiceFirstCompanyNoOfTickets = Tickets,
                            ServiceFirstCompanyManagerId = ManagerId
                        });
                    }

                }
                catch (Exception)
                {

                }
            }
            return _CompaniesData;
        }

        public List<ServiceFirstCompanyManager> GetCompanyManager()
        {
            List<ServiceFirstCompanyManager> CompaniesList = new List<ServiceFirstCompanyManager>();
            try
            {
                using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
                {
                    try
                    {
                        CompaniesList = session.Query<ServiceFirstCompanyManager>().ToList();
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception)
            {

            }
            return CompaniesList;
        }

    public bool ChangePasswordAdmin(ResetPasswordViewModel obj)
    {
        bool status = false;
        try
        {
            var id = (long)HttpContext.Current.Session["AdminUserId"];
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
            status = false;
        }

        return status;
    }
    public SelectList GetCountrySelectList()
    {
        SelectList selectList = null;
        List<ServiceFirstCountries> countries = new List<Models.ServiceFirstCountries>();
        ServiceFirstCountries selectedValue = null;
        using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
        {
            countries = session.Query<ServiceFirstCountries>().ToList();
            selectedValue = session.Query<ServiceFirstCountries>().Where(a => a.Name.ToLower() == "Norway").FirstOrDefault();
        }
        if (selectedValue != null)
        {
            selectList = new SelectList(countries.ToArray(),
                            "Id",
                            "Name", selectedValue.Id);
        }
        else
        {
            selectList = new SelectList(countries.ToArray(),
                            "Id",
                            "Name");
        }
        return selectList;
    }

    public SelectList GetCompanyList(long? id)
    {
        List<ServiceFirstCompanyManager> companiesManager = new List<ServiceFirstCompanyManager>();
        List<ServiceFirstCompanies> companies = new List<Models.ServiceFirstCompanies>();
        SelectList selectList = null;
        using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
        {

            try
            {
                companiesManager = session.Query<ServiceFirstCompanyManager>().Where(a => a.ServiceFirstCompanyManagerID != id).ToList();

                companies = session.Query<ServiceFirstCompanies>().Where(a => a.ServiceFirstCompanyIsActive == true).ToList();

                var result = (from c in companies

                              join cm in companiesManager on c.ServiceFirstCompanyID equals cm.ServiceFirstCompanyManagerCompanyID

                              into combinedResult

                              from f in combinedResult.DefaultIfEmpty()

                              select new
                              {
                                  ServiceFirstCompanyID = c.ServiceFirstCompanyID,

                                  ServiceFirstCompanyName = c.ServiceFirstCompanyName,

                                  Missing = (f == null ? 0 : f.ServiceFirstCompanyManagerCompanyID)

                              }).Where(a => a.Missing == 0);
                selectList = new SelectList(result.ToArray(),
                           "ServiceFirstCompanyID",
                           "ServiceFirstCompanyName");

            }
            catch
            {

            }
        }
        return selectList;

    }
}
}