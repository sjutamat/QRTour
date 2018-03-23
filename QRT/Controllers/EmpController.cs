using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;

namespace QRT.Controllers
{
    public class EmpController : Controller
    {
        private readonly Imas_empService _empService;
        public EmpController(Imas_empService iempService)
        {
            _empService = iempService;
        }
        // GET: Emp
        public ActionResult Index()
        {
            var data = _empService.GetAllEmp();
            return View(data);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(m_empViewModel model)
        {
            var data = _empService.FilterEmp(model);
            return View(data);
        }

        public ActionResult Add()
        {
            var model = _empService.GetEmp();
            return View("Detail", model);
        }


        public ActionResult Edit(long id)
        {
            var model = _empService.GetById(id);
            return View("Detail", model);
        }


        public JsonResult Save(m_empViewModel model)
        {
            string returnMsg = "";
            if (model != null)
            {
                _empService.Save(model);
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
            _empService.UpdateStatus(id);
            var returnMsg = "success";
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CodeGen(string compid)
        {
            var data = "";
            if (!String.IsNullOrEmpty(compid))
            {
                data = _empService.GetNewCode(Convert.ToInt32(compid));
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                data = "";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
           
        }
    }
}