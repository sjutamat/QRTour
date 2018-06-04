using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;
using QRT.helper;
using Op3ration.ExceptionHandler;

namespace QRT.Controllers
{
    public class RouteController : BaseController
    {
        
        private readonly Imas_routeService _routeService;
        private ValidateHandler Validator;
        public RouteController(Imas_routeService irouteservice)
        {
            _routeService = irouteservice;
            
        }

        private ValidateHandler ValidateModel(m_routeViewModel model)
        {
            Validator = new ValidateHandler();
            if (String.IsNullOrEmpty(model.title))
                Validator.AddMessage(MessageLevel.Error, "Please enter Title");


            //if (model.comp_id == null || model.comp_id == 0  )
            //    Validator.AddMessage(MessageLevel.Error, "Please select company");
            
            
            return Validator;
        }

        // GET: Route
        [HttpGet]
        public ActionResult Index()
        {
            if (admin != null)
            {
                var data = _routeService.GetAllRoute(admin);
                ViewBag.AdminName = admin.username;
                ViewBag.Role = admin.role;
                return View(data);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(m_routeViewModel model)
        {
            if (admin !=null)
            {
                var data = _routeService.FilterRoute(model, admin);
                ViewBag.AdminName = admin.username;
                ViewBag.Role = admin.role;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }


        public ActionResult Add()
        {
            if (admin != null)
            {
                var model = _routeService.GetRoute(admin);
                ViewBag.AdminName = admin.username;
                ViewBag.Role = admin.role;
                return View("Detail", model);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
            
        }


        public ActionResult Edit(long id)
        {
            if (admin != null)
            {
                var model = _routeService.GetById(id, admin);
                ViewBag.AdminName = admin.username;
                ViewBag.Role = admin.role;
                return View("Detail", model);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }

        public JsonResult Save(m_routeViewModel model)
        {
            string returnMsg = "";
            if (model!=null)
            {
                Validator = ValidateModel(model);
                if (Validator.HasError())
                {
                    // throw Validator;
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
                    _routeService.Save(model, admin);
                    returnMsg = "success";
                }
            }
            else
            {
                returnMsg = "Model is null";
            }

            return Json(returnMsg,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(long id)
        {
            _routeService.UpdateStatus(id,admin);
            var returnMsg = "success";
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }
    }
}