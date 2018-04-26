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
        public UserController(Imas_adminService imasadminservice)
        {
            _service = imasadminservice;
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
            if (m!=null)
            {
                UserInfo.Set(m);
                return Redirect("~/Home/Authen/");
                //return Redirect("~/Home/Index/");
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