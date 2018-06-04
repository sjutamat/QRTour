using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;
using Op3ration.ExceptionHandler;


namespace QRT.Controllers
{
    public class LocationController : BaseController
    {
        private readonly Imas_locationService _locationService;
        private ValidateHandler Validator;
        public LocationController(Imas_locationService ilocationService
            )
        {
            _locationService = ilocationService;
         }

        private ValidateHandler ValidateModel(m_locationViewModel model)
        {
            Validator = new ValidateHandler();
            if (String.IsNullOrEmpty(model.title))
                Validator.AddMessage(MessageLevel.Error, "Please enter FirstName.");
           
            if (model.seq_number == null)
                Validator.AddMessage(MessageLevel.Error, "Please enter Sequent Number.");

            if (String.IsNullOrEmpty(model.code1_status) && String.IsNullOrEmpty(model.code2_status))
                Validator.AddMessage(MessageLevel.Error, "Please active some QRCode.");

            return Validator;
        }

        // GET: Location
        public ActionResult Index()
        {
            if (admin != null)
            {
                var data = _locationService.GetAllLocation(admin);
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
        public ActionResult Index(m_locationViewModel model)
        {
            if (admin != null)
            {
                var data = _locationService.FilterLocation(model, admin);
                ViewBag.AdminName = admin.username;
                ViewBag.Role = admin.role;
                return View(data);
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
                var model = _locationService.GetRoute(admin);
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
                var model = _locationService.GetById(id, admin);
                ViewBag.AdminName = admin.username;
                ViewBag.Role = admin.role;
                return View("Detail", model);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }

        public JsonResult Save(m_locationViewModel model)
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
                    _locationService.Save(model, admin);
                    returnMsg = "success";
                }
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
                _locationService.SaveQuestion(model,admin);
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
            _locationService.UpdateStatus(id,admin);
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
            var data = _locationService.GetQuestion(rid,lid,admin);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}