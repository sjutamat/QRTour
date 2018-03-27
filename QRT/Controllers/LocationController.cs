using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;


namespace QRT.Controllers
{
    public class LocationController : BaseController
    {
        private readonly Imas_locationService _locationService;
        public LocationController(Imas_locationService ilocationService
            )
        {
            _locationService = ilocationService;
         }
        
        

        // GET: Location
        public ActionResult Index()
        {
            var data = _locationService.GetAllLocation(admin);
           
            return View(data);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(m_locationViewModel model)
        {
            var data = _locationService.FilterLocation(model,admin);
            return View(data);
        }

        public ActionResult Add()
        {
            var model = _locationService.GetRoute(admin);
            return View("Detail",model);
        }

        public ActionResult Edit(long id)
        {
            var model = _locationService.GetById(id,admin);
            return View("Detail", model);
        }

        public JsonResult Save(m_locationViewModel model)
        {
            string returnMsg = "";
            if (model != null)
            {
                _locationService.Save(model,admin);
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

        //public System.Web.UI.WebControls.Image QRCode()
        //{
        //    byte[] byteImage =_locationService.GenQRCode();
        //    System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
        //    imgBarCode.Height = 150;
        //    imgBarCode.Width = 150;
        //    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
        //    return imgBarCode;
        //}
    }
}