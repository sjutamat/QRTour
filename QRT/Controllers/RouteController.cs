using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;

namespace QRT.Controllers
{
    public class RouteController : Controller
    {
        private readonly Imas_routeService _routeService;
        public RouteController(Imas_routeService irouteservice)
        {
            _routeService = irouteservice;
        }
        

        // GET: Route
        [HttpGet]
        public ActionResult Index()
        {
            var data = _routeService.GetAllRoute();
            return View(data);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(m_routeViewModel model)
        {
            var data = _routeService.FilterRoute(model);
            return View(model);
        }


        public ActionResult Add()
        {
            var model = _routeService.GetRoute();
            return View("Detail", model);
        }


        public ActionResult Edit(long id)
        {
            var model = _routeService.GetById(id);
            return View("Detail",model);
        }

        public JsonResult Save(m_routeViewModel model)
        {
            string returnMsg = "";
            if (model!=null)
            {
               _routeService.Save(model);
               returnMsg = "success";
            }
            else
            {
                returnMsg = "Model is null";
            }

            return Json(returnMsg,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(long id)
        {
            _routeService.UpdateStatus(id);
            var returnMsg = "success";
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }
    }
}