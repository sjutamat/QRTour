using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Op3ration.ExceptionHandler;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;

namespace QRT.Controllers
{
    public class QuestionController : BaseController
    {
        private readonly Imas_questionService _questionService;
        private ValidateHandler Validator;
        public QuestionController(Imas_questionService iquestionservice)
        {
            _questionService = iquestionservice;
        }

        private ValidateHandler ValidateModel(m_questionViewModel model)
        {
            Validator = new ValidateHandler();
            if (String.IsNullOrEmpty(model.title))
                Validator.AddMessage(MessageLevel.Error, "Please enter Title");

            if (model.route_id == 0)
                Validator.AddMessage(MessageLevel.Error, "Please select Route");

         
            return Validator;
        }

        // GET: Question
        public ActionResult Index()
        {
            if (admin != null)
            {
                var data = _questionService.GetAllQuesion(admin);
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
        public ActionResult Index(m_questionViewModel model)
        {
            if (admin != null)
            {
                var data = _questionService.FilterQuestion(model, admin);
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
                var model = _questionService.GetQuestion(admin);
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
                var model = _questionService.GetById(id, admin);
                ViewBag.AdminName = admin.username;
                ViewBag.Role = admin.role;
                return View("Detail", model);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }

        public JsonResult Save(m_questionViewModel model)
        {
            string returnMsg = "";
            if (model != null)
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
                    _questionService.Save(model, admin);
                    returnMsg = "success";
                }

                
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