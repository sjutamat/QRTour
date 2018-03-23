using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;


namespace QRT.Controllers
{
    public class LocationController : Controller
    {
        private readonly Imas_locationService _locationService;
        //private readonly Imas_questionService _questservice;
        public LocationController(Imas_locationService ilocationService
            //,Imas_questionService imasquestservice
            )
        {
            _locationService = ilocationService;
            //_questservice = imasquestservice;
        }

        // GET: Location
        public ActionResult Index()
        {
            var data = _locationService.GetAllLocation();
           
            return View(data);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(m_locationViewModel model)
        {
            var data = _locationService.FilterLocation(model);
            return View(data);
        }

        public ActionResult Add()
        {
            var model = _locationService.GetRoute();
            return View("Detail",model);
        }

        public ActionResult Edit(long id)
        {
            var model = _locationService.GetById(id);
            return View("Detail", model);
        }

        public JsonResult Save(m_locationViewModel model)
        {
            string returnMsg = "";
            if (model != null)
            {
                _locationService.Save(model);
                returnMsg = "success";
            }
            else
            {
                returnMsg = "Model is null";
            }

            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveQuestion(m_locquestionViewModel model)
        {
            string returnMsg = "";
            if (model != null)
            {
                _locationService.SaveQuestion(model);
                returnMsg = "success";
            }
            else
            {
                returnMsg = "Model is null";
            }

            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(long id)
        {
            _locationService.UpdateStatus(id);
            var returnMsg = "success";
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCode()
        {
            var code = _locationService.CodeGenerater();
            return Json(code, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult InitPopUp(string route_id, string location_id)
        {
            var rid = Convert.ToInt32(route_id);
            var lid = Convert.ToInt32(location_id);
            var data = _locationService.GetQuestion(rid,lid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}