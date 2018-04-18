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
using ServiceFirstApplication.ViewModels;

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
                    objLogin.RememberMe = true;
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
            AdminUsers rUser = new Repository.AdminUsers();
            if (rUser.Login(_loginUser))
            {

                LoginInfo.CreateAdminLoginSession(rUser.UserObj.ServiceFirstID, "", rUser.UserObj.ServiceFirstUserName,
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
            AdminUsers rUser = new Repository.AdminUsers();
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
                var Company = rUser.GetEditCompanyData(id);
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
                    AdminUsers obj = new AdminUsers();
                    if (obj.AddCompany(_objCompany))
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
                    return View();
                }
            }
            else
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        AdminUsers obj = new AdminUsers();
                        if (obj.EditCompany(_objCompany))
                        {
                            ViewBag.Message = "Saved Successfully";
                        }
                        else
                        {
                            ViewBag.Message = obj.Message;
                        }
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
            return RedirectToAction("AddCompany", "Admin", new { message = ViewBag.Message, id = 0 });
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
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            if (_objCompanyManager.ServiceFirstCompanyManagerID == 0)
            {
                if (ModelState.IsValid)
                {
                    AdminUsers obj = new AdminUsers();
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
                        AdminUsers obj = new AdminUsers();
                        if (obj.EditCompanyManager(_objCompanyManager))
                        {
                            ViewBag.Message = "Saved Successfully";
                        }
                        else
                        {
                            ViewBag.Message = obj.Message;
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Not Saved successfully";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error occured:" + ex.Message;
                }
            }
            return RedirectToAction("AddCompanyManager", "Admin", new { message = ViewBag.Message, id = 0 });
        }
        public ActionResult AddCompanyManager(int? id, string message)
        {
            ViewBag.Message = message;
            AdminUsers obj = new Repository.AdminUsers();

            ViewBag.CountryList = obj.GetCountrySelectList();
            if (id == 0)
            {
                ViewBag.CompanyList = obj.GetCompanyList(0);
                ViewBag.TitleText = "Add Company Manager";
                ViewBag.ManagerID = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Edit Company Manager";
                var CompanyManager = obj.GetEditCompanyManagerData(id);
                CompanyManager.ServiceFirstCompanyManagerPassword = CommonFunctions.DecryptPassword(CompanyManager.ServiceFirstCompanyManagerPassword,CompanyManager.ServiceFirstCompanyManagerPasswordSalt);
                ViewBag.CompanyList = obj.GetCompanyList(CompanyManager.ServiceFirstCompanyManagerID);
                ViewBag.ManagerID = CompanyManager.ServiceFirstCompanyManagerID;
                return View(CompanyManager);
            }
        }

        public ActionResult InActiveCompany(int? Id)
        {
            AdminUsers obj = new AdminUsers();
            obj.InActiveCompany(Id);
            return RedirectToAction("CompanyList", "Admin");
        }

        public ActionResult ActiveCompany(int? Id)
        {
            AdminUsers obj = new AdminUsers();
            obj.ActiveCompany(Id);
            return RedirectToAction("CompanyList", "Admin");
        }

        public ActionResult ActiveCompanyManager(int? Id)
        {
            AdminUsers obj = new AdminUsers();
            obj.ActiveCompanyManager(Id);
            return RedirectToAction("CompanyManagerList", "Admin");
        }

        public ActionResult InActiveCompanyManager(int? Id)
        {
            AdminUsers obj = new AdminUsers();
            obj.InActiveCompanyManager(Id);
            return RedirectToAction("CompanyManagerList", "Admin");
        }

        [HttpGet]
        public JsonResult GetCompanies(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            AdminUsers obj = new AdminUsers();
            var _CompaniesData = obj.GetCompanies();

            int totalRecords = _CompaniesData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                _CompaniesData = _CompaniesData.OrderByDescending(t => t.ServiceFirstCompanyID).ToList();
                _CompaniesData = _CompaniesData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                _CompaniesData = _CompaniesData.OrderBy(t => t.ServiceFirstCompanyID).ToList();
                _CompaniesData = _CompaniesData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = _CompaniesData
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCompanyManager(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            AdminUsers obj = new AdminUsers();
            var CompaniesList = obj.GetCompanyManager();
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
                AdminUsers obj = new AdminUsers();
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