using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRT.Controllers
{
    public class LocQuestionController : BaseController
    {
        private readonly Imas_locquestionService _locquestService;
        public LocQuestionController(Imas_locquestionService ilocquestService)
        {
            _locquestService = ilocquestService;
        }


        // GET: LocQuestion
        public ActionResult Index()
        {
            var data = _locquestService.GetAllLocQuestion(admin);
            return View(data);
        }

        public ActionResult Add()
        {
            var model = _locquestService.InitailModel(admin);
            return View("Detail", model);
        }


        public ActionResult Edit(long id)
        {
            var model = _locquestService.GetByLocationId(id,admin);
            return View("Detail", model);
        }


        public JsonResult Save(m_locquestionViewModel model)
        {
            string returnMsg = "";
            if (model != null)
            {
                _locquestService.Save(model,admin);
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
            _locquestService.UpdateStatus(id,admin);
            var returnMsg = "success";
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }
    }
}