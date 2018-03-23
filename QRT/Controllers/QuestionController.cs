using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;

namespace QRT.Controllers
{
    public class QuestionController : Controller
    {
        private readonly Imas_questionService _questionService;
        public QuestionController(Imas_questionService iquestionservice)
        {
            _questionService = iquestionservice;
        }

        // GET: Question
        public ActionResult Index()
        {
            var data = _questionService.GetAllQuesion();
            return View(data);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(m_questionViewModel model)
        {
            var data = _questionService.FilterQuestion(model);
            return View(data);
        }

        public ActionResult Add()
        {
            var model = _questionService.GetQuestion();
            return View("Detail", model);
        }

        public ActionResult Edit(long id)
        {
            var model = _questionService.GetById(id);
            return View("Detail", model);
        }

        public JsonResult Save(m_questionViewModel model)
        {
            string returnMsg = "";
            if (model != null)
            {
                _questionService.Save(model);
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
            _questionService.UpdateStatus(id);
            var returnMsg = "success";
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }

    }
}