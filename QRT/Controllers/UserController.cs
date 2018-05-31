using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;
using QRT.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRT.Controllers
{
    public class UserController : Controller
    {
        private readonly Imas_adminService _service;
        private readonly Imas_viewerService _viewer;
        public UserController(Imas_adminService imasadminservice
            , Imas_viewerService imasviewerservice)
        {
            _service = imasadminservice;
            _viewer = imasviewerservice;
        }

        // GET: User
        public ActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(Login vm)
        {
            var m = _service.Login(vm);
            var v = _viewer.Login(vm);
            if (m != null && v == null)
            {
                m.role = (int)RolesEnum.Roles.Admin;
                UserInfo.Set(m);
                return Redirect("~/Home/Authen/");
            }
            else if (v != null && m == null)
            {
                v.role = (int)RolesEnum.Roles.Viewer;
                UserInfo.Set(v);
                return Redirect("~/Home/Authen/");
            }
            else
            {
                ViewBag.ErrorMessage = " username/password ไม่ถูกต้อง";
                return View();
            }
        }

        public ActionResult Logout()
        {
            UserInfo.Logout();
            return Redirect("~");
        }

        
    }
}