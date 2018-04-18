using NHibernate;
using NHibernate.Linq;
using ServiceFirstApplication.Models;
using ServiceFirstApplication.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceFirstApplication.Controllers
{
    
    public class AdminController : Controller
    {
        // GET: Admin 
      
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Login()
        {
            var objLogin = new LoginViewModel();
            if (Request.Cookies["loginadmin"] != null)
            {
                if (!string.IsNullOrEmpty(Request.Cookies["loginadmin"].Values["uname"]) &&
                !string.IsNullOrEmpty(Request.Cookies["loginadmin"].Values["pwd"]))
                {
                    objLogin.UserName = Request.Cookies["loginadmin"].Values["uname"];
                    objLogin.Password = Request.Cookies["loginadmin"].Values["pwd"];
                    var salt = Request.Cookies["loginadmin"].Values["salt"];
                    objLogin.Password = CommonFunctions.DecryptString(objLogin.Password, salt);
                    ViewBag.pwd = objLogin.Password;
                    objLogin.RememberMe  = true;
                }
            }
            return View(objLogin);
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel _loginUser)
        {
            /*
            string password = "123456";
            string _salt = string.Empty;
            string _password = string.Empty;
            CommonFunctions.GeneratePassword(password, "new", ref _salt, ref _password);
            */
            Users rUser = new Repository.Users();
            if (rUser.Login(_loginUser))
            {

                LoginInfo.CreateAdminLoginSession(rUser.UserObj.ServiceFirstID, "" , rUser.UserObj.ServiceFirstUserName,
                    rUser.UserObj.ServiceFirstPassword, rUser.UserObj.ServiceFirstUserName, rUser.UserObj.ServiceFirstUserName, "SuperAdmin"
                    , _loginUser.RememberMe, _loginUser.Salt);
                return RedirectToAction("CompanyList");
            }
            else
            {
                TempData["message"] = rUser.Message;
                return View();
            }

        }

      
       
        public ActionResult CompanyList()
        {

            return View();
        }

        public ActionResult AddCompany(int? id, string message)
        {
            ViewBag.Message = message;
            Users rUser = new Repository.Users();
            ViewBag.CountryList = rUser.GetCountrySelectList();
            if (id == 0)
            {
                ViewBag.TitleText = "Add Company";
                ViewBag.CompanyID = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Edit Company";
                ServiceFirstCompanies Company;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Company = session.Query<ServiceFirstCompanies>().Where(b => b.ServiceFirstCompanyID 
                    == id).FirstOrDefault();
                }

                //ServiceFirstCompany Company = db.ServiceFirstCompanies.Find(id);
                if (Company == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CompanyID = Company.ServiceFirstCompanyID;
                return View(Company);
            }

        }
        [HttpPost]
        public ActionResult AddCompany(ServiceFirstCompanies _objCompany, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string path = System.IO.Path.Combine(Server.MapPath("~/Images"),
                                               System.IO.Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    _objCompany.ServiceFirstCompanyLogoFile = file.FileName;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            if (_objCompany.ServiceFirstCompanyID == 0)
            {
                if (ModelState.IsValid)
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
                    ViewBag.Message = "Created Successfully";
                }
                else
                {
                    ViewBag.Message = "Not Created Successfully";
                    return View();
                }
            }
            else
            {
                try
                {
                    if (ModelState.IsValid)
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
                        ViewBag.Message = "Saved Successfully";
                    }
                    else
                    {
                        ViewBag.Message = "Validation data not successfully";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error occured:" + ex.Message;
                }
            }
            return RedirectToAction("AddCompany", "Admin", new { message = ViewBag.Message,id=0 });
        }
        
        [HttpPost]
        public ActionResult AddCompanyManager(ServiceFirstCompanyManager _objCompanyManager, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string path = System.IO.Path.Combine(Server.MapPath("~/Images"),
                                               System.IO.Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    _objCompanyManager.ServiceFirstCompanyManagerLogoFile = file.FileName;
                }
                catch (Exception ex)
                {
                    TempData["message"] = "ERROR:" + ex.Message.ToString();
                }
            }
            if (_objCompanyManager.ServiceFirstCompanyManagerID == 0)
            {
                if (ModelState.IsValid)
                {
                    Users obj = new Users();
                    if (obj.AddCompanyManager(_objCompanyManager))
                    {
                        ViewBag.Message = "Created Successfully";
                    }
                    else
                    {
                        ViewBag.Message = obj.Message;
                    }

                }
                else
                {
                    ViewBag.Message = "Not Created Successfully";
                }
            }
            else
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        using (ISession session = NHibernateSession.OpenSession())
                        {
                            ServiceFirstCompanies CompanyData;
                            CompanyData = session.Query<ServiceFirstCompanies>().Where(b => b.ServiceFirstCompanyID
                                == _objCompanyManager.ServiceFirstCompanyManagerID).FirstOrDefault();
                            if (string.IsNullOrEmpty(_objCompanyManager.ServiceFirstCompanyManagerLogoFile))
                            {
                                _objCompanyManager.ServiceFirstCompanyManagerLogoFile = CompanyData.ServiceFirstCompanyLogoFile;
                            }
                            _objCompanyManager.ServiceFirstCompanyManagerIsActive = CompanyData.ServiceFirstCompanyIsActive;
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
                            CommonFunctions.SendMail(_objCompanyManager);
                        }
                        ViewBag.Message = "Saved Successfully";
                        
                    }
                    else
                    {
                        ViewBag.Message = "Validation data not successfully";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error occured:" + ex.Message;
                }
            }
            return RedirectToAction("AddCompanyManager", "Admin", new { message = ViewBag.Message,id=0 });
        }
        public ActionResult AddCompanyManager(int? id, string message)
        {
            ViewBag.Message = message;
            Users obj = new Repository.Users();
            ViewBag.CompanyList = obj.GetCompanyList();
            ViewBag.CountryList = obj.GetCountrySelectList();
            if (id == 0)
            {
                ViewBag.TitleText = "Add Company Manager";
                ViewBag.ManagerID = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Edit Company Manager";
                ServiceFirstCompanyManager CompanyManager;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    CompanyManager = session.Query<ServiceFirstCompanyManager>().Where(b => b.ServiceFirstCompanyManagerID == id).FirstOrDefault();
                }
                //ServiceFirstCompanyManager CompanyManager = db.ServiceFirstCompanyManagers.Find(id);
                if (CompanyManager == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ManagerID = CompanyManager.ServiceFirstCompanyManagerID;
                return View(CompanyManager);
            }
        }
      
        

        


     
        public ActionResult InActiveCompany(int? Id)
        {
            ServiceFirstCompanies company;
            using (ISession session = NHibernateSession.OpenSession())
            {
                company = session.Query<ServiceFirstCompanies>().Where(b => b.ServiceFirstCompanyID == Id).FirstOrDefault();
                company.ServiceFirstCompanyIsActive = false ;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(company);
                    transaction.Commit();
                }
            }
            return RedirectToAction("CompanyList", "Admin");
        }

        public ActionResult ActiveCompany(int? Id)
        {
            ServiceFirstCompanies company ;
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

            /*
            db.Entry(company).State = EntityState.Modified;
            db.SaveChanges();
            */
            return RedirectToAction("CompanyList", "Admin");
        }

        public ActionResult ActiveCompanyManager(int? Id)
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
            /*
            ServiceFirstCompanyManager companyManager = db.ServiceFirstCompanyManagers.Find(Id);
            companyManager.ServiceFirstCompanyManagerIsActive = true;
            db.Entry(companyManager).State = EntityState.Modified;
            db.SaveChanges();
            */
            return RedirectToAction("CompanyManagerList", "Admin");
        }

        public ActionResult InActiveCompanyManager(int? Id)
        {
            ServiceFirstCompanyManager companyManager;
            using (ISession session = NHibernateSession.OpenSession())
            {
                companyManager = session.Query<ServiceFirstCompanyManager>().Where(b => b.ServiceFirstCompanyManagerID
                == Id).FirstOrDefault();
                companyManager.ServiceFirstCompanyManagerIsActive = false ;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(companyManager);
                    transaction.Commit();
                }
            }
            return RedirectToAction("CompanyManagerList", "Admin");
        }

        [HttpGet]
        public JsonResult GetCompanies(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            //List<ServiceFirstCompany> CompaniesList = new List<ServiceFirstCompany>();
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                // books = session.Query<Book>().ToList(); //  Querying to get all the books

                //var CompaniesList = session.Query<ServiceFirstCompanies>().Select(
                //        t => new
                //        {
                //            t.ServiceFirstCompanyContactEmail,
                //            t.ServiceFirstCompanyName,
                //            t.ServiceFirstCompanyNoOfCustomers,
                //            t.ServiceFirstCompanyNoOfTickets,
                //            t.ServiceFirstCompanyNoOfProjects,
                //            t.ServiceFirstCompanyID,
                //            t.ServiceFirstCompanyAddress,
                //            t.ServiceFirstCompanyLogoFile,
                //            t.ServiceFirstCompanyIsActive
                //        });
                List<ServiceFirstCompanies> CompaniesList = new List<Models.ServiceFirstCompanies>();
                try
                {
                    CompaniesList = session.Query<ServiceFirstCompanies>().ToList();
                }
                catch
                {

                } 

                int totalRecords = CompaniesList.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
                if (sort.ToUpper() == "DESC")
                {
                    CompaniesList = CompaniesList.OrderByDescending(t => t.ServiceFirstCompanyID).ToList();
                    CompaniesList = CompaniesList.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    CompaniesList = CompaniesList.OrderBy(t => t.ServiceFirstCompanyID).ToList();
                    CompaniesList = CompaniesList.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }

                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = CompaniesList
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetCompanyManager(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            using (ISession session = NHibernateSession.OpenSession())  // Open a session to conect to the database
            {
                //var CompaniesList = session.Query<ServiceFirstCompanyManager>().Select(
                //    t => new
                //    {
                //        t.ServiceFirstCompanyManagerContactEmail,
                //        t.ServiceFirstCompanyManagerContactPhone,
                //        t.ServiceFirstCompanyManagerName,
                //        t.ServiceFirstCompanyManagerAddress,
                //       // t.ServiceFirstCompany.ServiceFirstCompanyName,
                //        t.ServiceFirstCompanyManagerID,
                //        t.ServiceFirstCompanyManagerLogoFile,
                //        t.ServiceFirstCompanyManagerIsActive
                //    });
                List<ServiceFirstCompanyManager> CompaniesList = new List<ServiceFirstCompanyManager>();
                try
                {
                    CompaniesList = session.Query<ServiceFirstCompanyManager>().ToList();
                }
                catch
                {
                }               
                int totalRecords = CompaniesList.Count();
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
                if (sort.ToUpper() == "DESC")
                {
                    CompaniesList = CompaniesList.OrderByDescending(t => t.ServiceFirstCompanyManagerID).ToList();
                    CompaniesList = CompaniesList.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    CompaniesList = CompaniesList.OrderBy(t => t.ServiceFirstCompanyManagerID).ToList();
                    CompaniesList = CompaniesList.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = CompaniesList
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CompanyManagerList()
        {

            return View();
        }
        public ActionResult ChangePassword(string message)
        {
             ViewBag.Message = message;
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ResetPasswordViewModel _obj)
        {
            if (ModelState.IsValid)
            {
                Users obj = new Users();
                if (obj.ChangePasswordAdmin(_obj))
                {

                    ViewBag.Message = "Password is changed Successfully";

                }
                else
                {
                    ViewBag.Message = obj.Message;
                }
            }
            else
            {
                ViewBag.Message = "Not Created Successfully";
            }
            return RedirectToAction("ChangePassword", "Admin", new { message = ViewBag.Message });
        }




        public ActionResult Logout()
        {
            Session["UserID"] = null;

            return RedirectToAction("Login");
        }
    }
}