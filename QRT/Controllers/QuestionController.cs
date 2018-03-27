using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;

namespace QRT.Controllers
{
    public class QuestionController : BaseController
    {
        private readonly Imas_questionService _questionService;
        public QuestionController(Imas_questionService iquestionservice)
        {
            _questionService = iquestionservice;
        }

        // GET: Question
        public ActionResult Index()
        {
            var data = _questionService.GetAllQuesion(admin);
            return View(data);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(m_questionViewModel model)
        {
            var data = _questionService.FilterQuestion(model,admin);
            return View(data);
        }

        public ActionResult Add()
        {
            var model = _questionService.GetQuestion(admin);
            return View("Detail", model);
        }

        public ActionResult Edit(long id)
        {
            var model = _questionService.GetById(id,admin);
            return View("Detail", model);
        }

        public JsonResult Save(m_questionViewModel model)
        {
            string returnMsg = "";
            if (model != null)
            {
                _questionService.Save(model,admin);
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
            _questionService.UpdateStatus(id,admin);
            var returnMsg = "success";
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }

    }
}