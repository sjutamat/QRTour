using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Op3ration.ExceptionHandler;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;

namespace QRT.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IDashboardService _dashboard;
        private readonly Imas_routeService _route;
        private readonly Imas_viewerService _viewer;
        private readonly Imas_adminService _admin;
        private ValidateHandler Validator;

        public HomeController(IDashboardService idashboardService
            ,Imas_routeService irouteService
            ,Imas_viewerService iviewService
            ,Imas_adminService iadminService
            )
        {
            _route = irouteService;
            _viewer = iviewService;
            _dashboard = idashboardService;
            _admin = iadminService;
        }

        private ValidateHandler ValidateModel(hotkey model)
        {
            Validator = new ValidateHandler();
            if (model.keycode == 0)
                Validator.AddMessage(MessageLevel.Error, "Please generate the key.");

            return Validator;
        }



        // GET: Home
        public ActionResult Index()
        {
            if (admin != null)
            {
                dashboardViewModel dashboardData = new dashboardViewModel();
                dashboardData.route = _route.GetRouteItem(admin);
                int admin_id;
                if (admin.role != (int)RolesEnum.Roles.Admin)
                {
                   admin_id = _viewer.GetAdminID(admin.id);
                }
                else
                {
                    admin_id = admin.id;
                }
                dashboardData.reportList = _dashboard.GetAnswer(admin_id);
                dashboardData.s_dashboard = new SearchDataDashboard();
                ViewBag.AdminName = admin.username;
                ViewBag.Role = admin.role;
                return View(dashboardData);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }

        [HttpPost]
        public ActionResult Index(dashboardViewModel model)
        {
            if (admin != null)
            {
                dashboardViewModel dashboardData = new dashboardViewModel();
                dashboardData.route = _route.GetRouteItem(admin);
                int admin_id;
                if (admin.role != (int)RolesEnum.Roles.Admin)
                {
                    admin_id = _viewer.GetAdminID(admin.id);
                }
                else
                {
                    admin_id = admin.id;
                }
                dashboardData.reportList = _dashboard.GetAnswerFilter(model, admin_id);
                dashboardData.s_dashboard = model.s_dashboard;
                ViewBag.AdminName = admin.username;
                ViewBag.Role = admin.role;
                return View(dashboardData);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }

        public ActionResult Authen()
        {
            string admin_name;
            if (admin.role != (int)RolesEnum.Roles.Admin)
            {
                var admin_id = _viewer.GetAdminID(admin.id);
                admin_name = _admin.GetAdminName(admin_id);
            }
            else
            {
                admin_name = admin.username;
            }
            ViewBag.Admin_Name = admin_name;
            ViewBag.AdminName = admin.username;
            ViewBag.Role = admin.role;
            return View("AuthenPage");
        }


        public ActionResult GenerateHotkey()
        {
            ViewBag.AdminName = admin.username;
            ViewBag.Role = admin.role;
            return View("HotKey");
        }


        public JsonResult GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            var hotkey = _rdm.Next(_min, _max);
            return Json(hotkey, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveHotKey(hotkey model)
        {
            string returnMsg = "";
            if (model != null)
            {
                Validator = ValidateModel(model);
                if (Validator.HasError())
                {
                    var msg = Validator._MessageList.ToList();
                    var text = "";
                    for (int i = 0; i < msg.Count(); i++)
                    {
                        text += "<p>" + msg[i].Message + "</p>" + "\n";
                    }
                    returnMsg = text;
                }
                else
                {
                    _dashboard.SaveHotKey(model, admin);
                    returnMsg = "success";
                }
            }
            else
            {
                returnMsg = "Model is null";
            }
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }
    }
}