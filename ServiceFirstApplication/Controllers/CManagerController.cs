using NHibernate;
using NHibernate.Linq;
using ServiceFirstApplication.Models;
using ServiceFirstApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceFirstApplication.Controllers
{
    public class CManagerController : Controller
    {
        // GET: CManager

        #region Login
        public ActionResult Login()
        {
            var objLogin = new LoginViewModel();
            if (Request.Cookies["loginCManager"] != null)
            {
                if (!string.IsNullOrEmpty(Request.Cookies["loginCManager"].Values["uname"]) &&
                !string.IsNullOrEmpty(Request.Cookies["loginadmin"].Values["pwd"]))
                {
                    objLogin.UserName = Request.Cookies["loginCManager"].Values["uname"];
                    objLogin.Password = Request.Cookies["loginCManager"].Values["pwd"];
                    var salt = Request.Cookies["loginCManager"].Values["salt"];
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
            CompanyManagerUsers rUser = new Repository.CompanyManagerUsers();
            long CompanyId = 0;
            if (rUser.Login(_loginUser, out CompanyId))
            {
                LoginInfo.CreateCManagerLoginSession(rUser.UserObj.ServiceFirstCompanyManagerID, "", rUser.UserObj.ServiceFirstCompanyManagerUserName,
                    rUser.UserObj.ServiceFirstCompanyManagerPassword, rUser.UserObj.ServiceFirstCompanyManagerName, CompanyId.ToString(), "CompanyManager"
                    , _loginUser.RememberMe, _loginUser.Salt);
                Session["AdminName"] = _loginUser.UserName;
                using (ISession session = NHibernateSession.OpenSession())
                {
                    var CompanyData = session.Query<ServiceFirstCompanies>().Where(x => x.ServiceFirstCompanyID == CompanyId).FirstOrDefault();
                    Session["LogoPath"]=CompanyData!=null? "/Images/"+CompanyData.ServiceFirstCompanyLogoFile+"?w=511&h=75" : "~/img/logo.png";
                }
                return RedirectToAction("Dashboard");
            }
            else
            {
                TempData["message"] = rUser.Message;
                return View();
            }

        }

        #endregion
        public ActionResult Dashboard()
        {
            return View();
        }

        #region Edit Logo
        public ActionResult EditLogo(string message)
        {
            ViewBag.Message = message;
            return View();
        }
        [HttpPost]
        public ActionResult EditLogo(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string path = System.IO.Path.Combine(Server.MapPath("~/Images"),
                                               System.IO.Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    CompanyManagerUsers user = new CompanyManagerUsers();
                    if (user.UpdateLogo(file.FileName))
                    {
                        Session["LogoPath"] = "/Images/"+file.FileName + "?w=511&h=75";
                        ViewBag.Message = "Success: Logo is updated successfully.";
                    }
                    else
                    {
                        ViewBag.Message = "ERROR: Logo is not updated, please try again.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
                return View();
            }
            else
            {
                ViewBag.Message = "Choose Image.";
                return View();
            }
        }

        #endregion

        #region Project Categories
        public ActionResult ListProjectCategories()
        {
            return View();
        }
        public ActionResult AddProjectCategory(int? id, string message)
        {
            ViewBag.Message = message;
            CompanyManagerUsers rUser = new Repository.CompanyManagerUsers();
            if (id == 0)
            {
                ViewBag.TitleText = "Add Project Categories";
                ViewBag.ProjectCatId = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Edit Project Categories";
                var CatData = rUser.GetProjectCategoryById(id);
                ViewBag.ProjectCatId = CatData.ServiceFirstProjectCategoryId;
                return View(CatData);
            }
        }
        [HttpPost]
        public ActionResult AddProjectCategory(ServiceFirstProjectCategories _objCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyManagerUsers obj = new CompanyManagerUsers();
                    if (obj.AddEditProjectCat(_objCategory))
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
                    ViewBag.Message = "Some error is occured, please try again.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occured:" + ex.Message;
            }

            return RedirectToAction("AddProjectCategory", "CManager", new { message = ViewBag.Message, id = 0 });
        }
        [HttpGet]
        public JsonResult GetProjectCategories(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            var _Data = obj.GetProjectCategories();

            int totalRecords = _Data.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                _Data = _Data.OrderByDescending(t => t.ServiceFirstProjectCategoryId).ToList();
                _Data = _Data.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                _Data = _Data.OrderBy(t => t.ServiceFirstProjectCategoryId).ToList();
                _Data = _Data.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = _Data
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActiveInActiveProjectCat(int? Id)
        {
            CompanyManagerUsers obj = new CompanyManagerUsers();
            obj.ActiveInActiveProjectCatById(Id);
            return RedirectToAction("ListProjectCategories", "CManager");
        }

        #endregion

        #region Ticket Types
        public ActionResult AddTicketType(int? id, string message)
        {
            ViewBag.Message = message;
            CompanyManagerUsers rUser = new Repository.CompanyManagerUsers();
            if (id == 0 || id == null)
            {
                ViewBag.TitleText = "Add Ticket Type";
                ViewBag.TicketTypeId = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Edit Ticket Type";
                var data = rUser.GetTicketTypeById(id);
                ViewBag.TicketTypeId = data.ServiceFirstTickeTypeID;
                return View(data);
            }
        }
        [HttpPost]
        public ActionResult AddTicketType(ServiceFirstTicketType _objTicket, HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        string path = System.IO.Path.Combine(Server.MapPath("~/Images"),
                                                   System.IO.Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        _objTicket.ServiceFirstTicketTypeIcon = file.FileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }
                if (ModelState.IsValid)
                {
                    CompanyManagerUsers obj = new CompanyManagerUsers();
                    if (obj.AddEditTicketType(_objTicket))
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
                    ViewBag.Message = "Some error is occured, please try again.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occured:" + ex.Message;
            }

            return RedirectToAction("AddTicketType", "CManager", new { message = ViewBag.Message, id = 0 });
        }

        public ActionResult ListTicketType()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetTicketTypes(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            var _Data = obj.GetTicketTypes();

            int totalRecords = _Data.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                _Data = _Data.OrderByDescending(t => t.ServiceFirstTickeTypeID).ToList();
                _Data = _Data.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                _Data = _Data.OrderBy(t => t.ServiceFirstTickeTypeID).ToList();
                _Data = _Data.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = _Data
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActiveInActiveTicketType(int? Id)
        {
            CompanyManagerUsers obj = new CompanyManagerUsers();
            obj.ActiveInActiveTicketType(Id);
            return RedirectToAction("ListTicketType", "CManager");
        }

        #endregion

        #region Ticket Priority
        public ActionResult AddTicketPriority(int? id, string message)
        {
            ViewBag.Message = message;
            CompanyManagerUsers rUser = new Repository.CompanyManagerUsers();
            if (id == 0 || id == null)
            {
                ViewBag.TitleText = "Add Ticket Priority";
                ViewBag.TicketPriorityId = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Edit Ticket Priority";
                var data = rUser.GetTicketPriorityById(id);
                ViewBag.TicketPriorityId = data.ServiceFirstTicketPriorityID;
                return View(data);
            }
        }
        [HttpPost]
        public ActionResult AddTicketPriority(ServiceFirstTicketPriority _objTicket, HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        string path = System.IO.Path.Combine(Server.MapPath("~/Images"),
                                                   System.IO.Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        _objTicket.ServiceFirstTicketPriorityIcon = file.FileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }
                if (ModelState.IsValid)
                {
                    CompanyManagerUsers obj = new CompanyManagerUsers();
                    if (obj.AddEditTicketPriority(_objTicket))
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
                    ViewBag.Message = "Some error is occured, please try again.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occured:" + ex.Message;
            }
            return RedirectToAction("AddTicketPriority", "CManager", new { message = ViewBag.Message, id = 0 });
        }

        public ActionResult ListTicketPriority()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetTicketPriority(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            var _Data = obj.GetTicketPriorities();

            int totalRecords = _Data.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                _Data = _Data.OrderByDescending(t => t.ServiceFirstTicketPriorityID).ToList();
                _Data = _Data.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                _Data = _Data.OrderBy(t => t.ServiceFirstTicketPriorityID).ToList();
                _Data = _Data.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = _Data
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActiveInActiveTicketPriority(int? Id)
        {
            CompanyManagerUsers obj = new CompanyManagerUsers();
            obj.ActiveInActiveTicketPriorityById(Id);
            return RedirectToAction("ListTicketPriority", "CManager");
        }

        #endregion

        #region Ticket Status
        public ActionResult AddTicketStatus(int? id, string message)
        {
            ViewBag.Message = message;
            CompanyManagerUsers rUser = new Repository.CompanyManagerUsers();
            if (id == 0)
            {
                ViewBag.TitleText = "Add Ticket Status";
                ViewBag.StatusID = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Edit Ticket Status";
                var Tickets = rUser.GetEditTicketStatus(id);
                ViewBag.StatusID = Tickets.ServiceFirstTicketStatusID;
                return View(Tickets);
            }
        }
        [HttpPost]
        public ActionResult AddTicketStatus(ServiceFirstTicketStatus _objTicketStatus, HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        var _Id = Guid.NewGuid();
                        string path = System.IO.Path.Combine(Server.MapPath("~/Images"),
                                                   System.IO.Path.GetFileName(_Id + "_" + _objTicketStatus.ServiceFirstTicketStatusName + "_" + file.FileName));
                        file.SaveAs(path);
                        _objTicketStatus.ServiceFirstTicketStatusIcon = _Id + "_" + _objTicketStatus.ServiceFirstTicketStatusName + "_" + file.FileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }
                if (ModelState.IsValid)
                {
                    CompanyManagerUsers obj = new CompanyManagerUsers();
                    if (obj.AddEditTicketStatus(_objTicketStatus))
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
                    ViewBag.Message = "Some error is occured, please try again.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occured:" + ex.Message;
            }
            return RedirectToAction("AddTicketStatus", "CManager", new { message = ViewBag.Message });
        }
        public ActionResult TicketStatusList()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetTicketStatus(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            var _TicketsData = obj.GetTicketStatus();

            int totalRecords = _TicketsData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                _TicketsData = _TicketsData.OrderByDescending(t => t.ServiceFirstCompanyID).ToList();
                _TicketsData = _TicketsData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                _TicketsData = _TicketsData.OrderBy(t => t.ServiceFirstCompanyID).ToList();
                _TicketsData = _TicketsData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = _TicketsData
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActiveTicketStatus(int? Id)
        {
            CompanyManagerUsers obj = new CompanyManagerUsers();
            obj.ActiveInActiveTicketStatus(Id);
            return RedirectToAction("TicketStatusList", "CManager");
        }

        #endregion

        #region User Groups
        public ActionResult AddUserGroup(int? id, string message)
        {
            ViewBag.Message = message;
            CompanyManagerUsers rUser = new CompanyManagerUsers();
            if (id == 0)
            {
                ViewBag.TitleText = "Add User Group";
                ViewBag.GroupID = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Edit User Group";
                var userGroups = rUser.GetEditUserGroups(id);
                ViewBag.GroupID = userGroups.ServiceFirstGroupID;
                return View(userGroups);
            }
        }

        [HttpPost]
        public ActionResult AddUserGroup(ServiceFirstUserGroup _userGroups)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyManagerUsers obj = new CompanyManagerUsers();
                    if (obj.AddEditUserGroups(_userGroups))
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
                    ViewBag.Message = "Some error is occured, please try again.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occured:" + ex.Message;
            }
            return RedirectToAction("AddUserGroup", "CManager", new { message = ViewBag.Message });
        }
        public ActionResult UserGroupList()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetUserGroups(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            var UserData = obj.GetUserGroups();

            int totalRecords = UserData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                UserData = UserData.OrderByDescending(t => t.ServiceFirstCompanyID).ToList();
                UserData = UserData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                UserData = UserData.OrderBy(t => t.ServiceFirstCompanyID).ToList();
                UserData = UserData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = UserData
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActiveUserGroup(int? Id)
        {
            CompanyManagerUsers obj = new CompanyManagerUsers();
            obj.ActiveInActiveUserGroup(Id);
            return RedirectToAction("UserGroupList", "CManager");
        }

        #endregion

        #region Ticket Resolution
        public ActionResult AddTicketResolution(int? id, string message)
        {
            ViewBag.Message = message;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            if (id == 0)
            {
                ViewBag.TitleText = "Add Ticket Resolution";
                ViewBag.ResolutionId = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Edit Ticket Resolution";
                var Tickets = obj.GetEditTicketResolution(id);
                ViewBag.ResolutionId = Tickets.ServiceFirstTicketResolutionID;
                return View(Tickets);
            }
        }

        [HttpPost]
        public ActionResult AddTicketResolution(ServiceFirstTicketResolution objTicketResolution, HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        var Id = Guid.NewGuid();
                        string path = System.IO.Path.Combine(Server.MapPath("~/Images"),
                                                   System.IO.Path.GetFileName(Id + "_" + objTicketResolution.ServiceFirstTicketResolutionName + "_" + file.FileName));
                        file.SaveAs(path);
                        objTicketResolution.ServiceFirstTicketResolutionIcon = Id + "_" + objTicketResolution.ServiceFirstTicketResolutionName + "_" + file.FileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }
                if (ModelState.IsValid)
                {
                    CompanyManagerUsers obj = new CompanyManagerUsers();
                    if (obj.AddEditTicketResolution(objTicketResolution))
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
                    ViewBag.Message = "Some error is occured, please try again.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occured:" + ex.Message;
            }

            return RedirectToAction("AddTicketResolution", "CManager", new { message = ViewBag.Message });
        }
        public ActionResult TicketResolutionList()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTicketResolution(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            var TicketsData = obj.GetTicketResolution();

            int totalRecords = TicketsData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                TicketsData = TicketsData.OrderByDescending(t => t.ServiceFirstCompanyID).ToList();
                TicketsData = TicketsData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                TicketsData = TicketsData.OrderBy(t => t.ServiceFirstCompanyID).ToList();
                TicketsData = TicketsData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = TicketsData
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActiveTicketResolution(int? Id)
        {
            CompanyManagerUsers obj = new CompanyManagerUsers();
            obj.InActiveTicketResolution(Id);
            return RedirectToAction("TicketResolutionList", "CManager");
        }

        #endregion

        #region Customers
        public ActionResult AddCustomers(int? id, string message)
        {
            ViewBag.Message = message;
            AdminUsers objAdmin = new AdminUsers();
            ViewBag.CountryList = objAdmin.GetCountrySelectList();
            if (id == 0)
            {
                ViewBag.TitleText = "Add Customers";
                ViewBag.CustomerID = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Edit Customers";
                CompanyManagerUsers obj = new CompanyManagerUsers();
                var customers = obj.GetEditCustomersList(id);
                ViewBag.CustomerID = customers.ServiceFirstCustomerID;
                return View(customers);
            }
        }

        [HttpPost]
        public ActionResult AddCustomers(ServiceFirstCustomers _objCustomers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyManagerUsers obj = new CompanyManagerUsers();
                    if (obj.AddEditCustomers(_objCustomers))
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
                    ViewBag.Message = "Some error is occured, please try again.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occured:" + ex.Message;
            }
            return RedirectToAction("AddCustomers", "CManager", new
            {
                message = ViewBag.Message
            });
        }
        public ActionResult CustomersList()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCustomersList(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            var CustomersData = obj.GetCustomersList();

            int totalRecords = CustomersData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                CustomersData = CustomersData.OrderByDescending(t => t.ServiceFirstCompanyID).ToList();
                CustomersData = CustomersData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                CustomersData = CustomersData.OrderBy(t => t.ServiceFirstCompanyID).ToList();
                CustomersData = CustomersData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = CustomersData
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActiveCustomers(int? Id)
        {
            CompanyManagerUsers obj = new CompanyManagerUsers();
            obj.ActiveInActiveCustomers(Id);
            return RedirectToAction("CustomersList", "CManager");
        }

        public ActionResult UploadCustomerCSV(string message)
        {
            ViewBag.Message = message;
            return View();
        }
        [HttpPost]
        public ActionResult UploadCustomerCSV(HttpPostedFileBase file,
                   string upload, string download)
        {
            try
            {
                if (upload == "upload")
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        try
                        {
                            List<ServiceFirstCustomers> customers = new List<ServiceFirstCustomers>();
                            string path = System.IO.Path.Combine(Server.MapPath("~/Uploads"),
                                                       System.IO.Path.GetFileName(file.FileName));
                            file.SaveAs(path);
                            string csvData = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF7);
                            //Execute a loop over the rows.
                            int count = 0;
                            foreach (string row in csvData.Split('\n'))
                            {
                                if (count != 0)
                                {
                                    if (!string.IsNullOrEmpty(row))
                                    {
                                        customers.Add(new ServiceFirstCustomers
                                        {
                                            ServiceFirstCustomerNumber = row.Split(',')[0],
                                            ServiceFirstCustomerName = row.Split(',')[1],
                                            ServiceFirstCustomerContactPerson = row.Split(',')[2],
                                            ServiceFirstCustomerContactPhone = row.Split(',')[3],
                                            ServiceFirstCustomerContactEmail = row.Split(',')[4],
                                            ServiceFirstCustomerAddress = row.Split(',')[5],
                                            ServiceFirstCustomerAddressII = row.Split(',')[6],
                                            ServiceFirstCustomerPostalPlace = row.Split(',')[7],
                                            ServiceFirstCustomerPostalNumber = row.Split(',')[8],
                                            ServiceFirstCustomerCountry = !string.IsNullOrEmpty(row.Split(',')[9])
                                            ? (Convert.ToInt64(row.Split(',')[9])) : 0,
                                            ServiceFirstCustomerOrganisationNumber = row.Split(',')[10],
                                        });
                                    }
                                }
                                count += 1;
                            }
                            CompanyManagerUsers users = new CompanyManagerUsers();
                            if (users.UploadCustomers(customers))
                            {
                                ViewBag.Message = "File data is saved successfully.";
                            }
                            else
                            {
                                ViewBag.Message = users.Message;
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Choose file.";
                    }

                    return RedirectToAction("UploadCustomerCSV", "CManager", new { message = ViewBag.Message });
                }
                else
                {
                    string path = System.IO.Path.Combine(Server.MapPath("~/"),
                                                       System.IO.Path.GetFileName("customerstest.csv"));
                    byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "customerstest.csv");
                }
            }

            catch (Exception ex)
            {
                ViewBag.Message = "Error occured:" + ex.Message;
                return RedirectToAction("UploadCustomerCSV", "CManager", new { message = ViewBag.Message });
            }


        }

        #endregion

        #region Projects
        public ActionResult AddProjects(int? id, string message)
        {
            ViewBag.Message = message;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            ViewBag.Customers = obj.GetCustomersSelectList();
            ViewBag.Owners = obj.GetUsersSelectList();
            if (id == 0)
            {
                ViewBag.TitleText = "Add Projects";
                ViewBag.ProjectID = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Edit Projects";
                var projects = obj.GetEditProjectsList(id);
                ViewBag.ProjectID = projects.ServiceFirstProjectID;
                return View(projects);
            }
        }

        [HttpPost]
        public ActionResult AddProjects(ServiceFirstProjects _objProjects)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyManagerUsers obj = new CompanyManagerUsers();
                    if (obj.AddEditProjects(_objProjects))
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
                    ViewBag.Message = "Some error is occured, please try again.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occured:" + ex.Message;
            }
            return RedirectToAction("AddProjects", "CManager", new
            {
                message = ViewBag.Message
            });
        }
        public ActionResult ProjectsList()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetProjectsList(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            var ProjectsData = obj.GetProjectsList();

            int totalRecords = ProjectsData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                ProjectsData = ProjectsData.OrderByDescending(t => t.ServiceFirstCompanyID).ToList();
                ProjectsData = ProjectsData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                ProjectsData = ProjectsData.OrderBy(t => t.ServiceFirstCompanyID).ToList();
                ProjectsData = ProjectsData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = ProjectsData
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActiveProjects(int? Id)
        {
            CompanyManagerUsers obj = new CompanyManagerUsers();
            obj.ActiveInActiveProjects(Id);
            return RedirectToAction("ProjectsList", "CManager");
        }

        #endregion

        #region Users
        public ActionResult AddUser(int? id, string message)
        {
            ViewBag.Message = message;
            CompanyManagerUsers rUser = new Repository.CompanyManagerUsers();
            ViewBag.UserGroups = rUser.GetUserGroupsSelectList();
            if (id == 0 || id == null)
            {
                ViewBag.TitleText = "Add User";
                ViewBag.UserId = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Edit User";
                var data = rUser.GetUserById(id);
                data.ServiceFirstUserPassword = CommonFunctions.DecryptPassword(data.ServiceFirstUserPassword
                    , data.ServiceFirstUserPassworSalt);
                ViewBag.UserId = data.ServiceFirstUserID;
                return View(data);
            }
        }

        [HttpPost]
        public ActionResult AddUser(ServiceFirst_UserLogin objUser, HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        string path = System.IO.Path.Combine(Server.MapPath("~/Images"),
                                                   System.IO.Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        objUser.ServiceFirstUserPicture = file.FileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }
                if (ModelState.IsValid)
                {
                    CompanyManagerUsers obj = new CompanyManagerUsers();
                    if (obj.AddEditUser(objUser))
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
                    ViewBag.Message = "Some error is occured, please try again.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occured:" + ex.Message;
            }

            return RedirectToAction("AddUser", "CManager", new { message = ViewBag.Message, id = 0 });
        }

        public ActionResult ListUsers()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetUserList(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            var CustomersData = obj.GetUsersList();

            int totalRecords = CustomersData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                CustomersData = CustomersData.OrderByDescending(t => t.ServiceFirstUserID).ToList();
                CustomersData = CustomersData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                CustomersData = CustomersData.OrderBy(t => t.ServiceFirstUserID).ToList();
                CustomersData = CustomersData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = CustomersData
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActiveInActiveUser(int? Id)
        {
            CompanyManagerUsers obj = new CompanyManagerUsers();
            obj.ActiveInActiveUser(Id);
            return RedirectToAction("ListUsers", "CManager");
        }

        #endregion

        #region Change Password
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
                CompanyManagerUsers obj = new CompanyManagerUsers();
                if (obj.ChangePassword(_obj))
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
            return RedirectToAction("ChangePassword", "CManager", new { message = ViewBag.Message });
        }

        #endregion


        #region Contact Persons
        public ActionResult AddContactPersons(int?id,string message)
        {
            ViewBag.Message = message;
            CompanyManagerUsers rUser = new Repository.CompanyManagerUsers();
            ViewBag.CustomersData = rUser.GetCustomersSelectList();
            if (id == 0 || id == null)
            {
                ViewBag.TitleText = "Add Contact Persons";
                ViewBag.UserId = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Add Contact Persons";
                var data = rUser.GetEditContactPersons(id);
                data.ServiceFirstPassword = CommonFunctions.DecryptPassword(data.ServiceFirstPassword
                    , data.ServiceFirstPassworSalt);
                ViewBag.UserId = data.ServiceFirstID;
                return View(data);
            }
        }       
        [HttpPost]
        public ActionResult AddContactPersons(ServiceFirst_Login _objContactPersons, HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        var _id = Guid.NewGuid();
                        string path = System.IO.Path.Combine(Server.MapPath("~/Images"),
                                                   System.IO.Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        _objContactPersons.ServiceFirstUserPicture = _id+"_"+file.FileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                }
                if (ModelState.IsValid)
                {
                    CompanyManagerUsers obj = new CompanyManagerUsers();
                    if (obj.AddEditContactPersons(_objContactPersons))
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
                    ViewBag.Message = "Some error is occured, please try again.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occured:" + ex.Message;
            }

            return RedirectToAction("AddContactPersons", "CManager", new { message = ViewBag.Message, id = 0 });
        }

        public ActionResult ContactPersonsList()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetContactPersons(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            var _Data = obj.GetContactPersons();

            int totalRecords = _Data.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                _Data = _Data.OrderByDescending(t => t.ServiceFirstID).ToList();
                _Data = _Data.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                _Data = _Data.OrderBy(t => t.ServiceFirstID).ToList();
                _Data = _Data.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = _Data
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActiveInActiveContactPersons(int? Id)
        {
            CompanyManagerUsers obj = new CompanyManagerUsers();
            obj.InActiveContactPersons(Id);
            return RedirectToAction("ContactPersonsList", "CManager");
        }

        #endregion

        #region Tickets
        public ActionResult AddTicket(int? id, string message)
        {
            ViewBag.Message = message;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            ViewBag.Customers = obj.GetCustomersSelectList();
            ViewBag.Projects = obj.GetProjectsSelectList();
            ViewBag.Reporters = obj.GetUsersSelectList();
            ViewBag.Assignees = obj.GetContactPersonsSelectList();
            ViewBag.Types = obj.GetTicketTypesSelectList();
            ViewBag.Priorities = obj.GetTicketPrioritySelectList();
            ViewBag.Statuses = obj.GetTicketStatusSelectList();
            ViewBag.Resolutions = obj.GetTicketResolutionSelectList();
            if (id == 0 || id == null)
            {
                ViewBag.TitleText = "Add Ticket";
                ViewBag.TicketId = id;
                return View();
            }
            else
            {
                ViewBag.TitleText = "Edit Ticket";
                var data = obj.GetTicketById(id);
                ViewBag.TicketId = data.ServiceFirstTicketID;
                return View(data);
            }
        }

        [HttpPost]
        public ActionResult AddTicket(ServiceFirstTickets objTicket)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CompanyManagerUsers obj = new CompanyManagerUsers();
                    if (obj.AddEditTicket(objTicket))
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
                    ViewBag.Message = "Some error is occured, please try again.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occured:" + ex.Message;
            }

            return RedirectToAction("AddTicket", "CManager", new { message = ViewBag.Message, id = 0 });
        }

        public ActionResult ListTicket()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTicketList(string sidx, string sort, int page, int rows)
        {
            sort = (sort == null) ? "" : sort;
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            CompanyManagerUsers obj = new CompanyManagerUsers();
            var CustomersData = obj.GetTicketList();

            int totalRecords = CustomersData.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sort.ToUpper() == "DESC")
            {
                CustomersData = CustomersData.OrderByDescending(t => t.ServiceFirstTicketID).ToList();
                CustomersData = CustomersData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                CustomersData = CustomersData.OrderBy(t => t.ServiceFirstTicketID).ToList();
                CustomersData = CustomersData.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = CustomersData
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        

        #endregion
        public ActionResult DownloadUsers()
        {
            CompanyManagerUsers objusers = new CompanyManagerUsers();
            var users = objusers.GetUsersList();
            List<ExportData> finalUserList = new List<Models.ExportData>();
            foreach (var item in users)
            {
                finalUserList.Add(new ExportData
                {
                    ID = item.ServiceFirstUserID,
                    Name = item.ServiceFirstUserFirstName,
                    ContactEmail = item.ServiceFirstUserContactEmail,
                    ContactNumber = item.ServiceFirstUserContactNumber
                });
            }
            CompanyManagerUsers.ExportToExcel(finalUserList, "UserList");
            return View();
        }

        public ActionResult DownloadCustomers()

        {
            CompanyManagerUsers objusers = new CompanyManagerUsers();
            var users = objusers.GetCustomersList();
            List<ExportData> finalUserList = new List<Models.ExportData>();
            foreach (var item in users)
            {
                finalUserList.Add(new ExportData
                {
                    ID = item.ServiceFirstCustomerID,
                    Name = item.ServiceFirstCustomerName,
                    ContactEmail = item.ServiceFirstCustomerContactEmail,
                    ContactNumber = item.ServiceFirstCustomerNumber,
                });
            }
            CompanyManagerUsers.ExportToExcel(finalUserList, "CustomerList");
            return View();
        }

        public ActionResult Logout()
        {
            Session["CManagerUserId"] = null;
            return RedirectToAction("Login");
        }
    }
}