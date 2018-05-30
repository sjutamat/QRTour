using Op3ration.ExceptionHandler;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRT.Controllers
{
    public class ViewerController : BaseController
    {
        private readonly Imas_viewerService _viewerService;
        private ValidateHandler Validator;
        public ViewerController(Imas_viewerService iviewerservice)
        {
            _viewerService = iviewerservice;
        }


        private ValidateHandler ValidateModel(m_viewerViewModel model)
        {
            Validator = new ValidateHandler();
            if (String.IsNullOrEmpty(model.first_name))
                Validator.AddMessage(MessageLevel.Error, "Please enter First Name");

            if (String.IsNullOrEmpty(model.last_name))
                Validator.AddMessage(MessageLevel.Error, "Please enter Last Name");
           
            return Validator;
        }



        // GET: Viewer
        public ActionResult Index()
        {
            if (admin != null)
            {
                var data = _viewerService.GetAllReportViewer(admin);
                ViewBag.AdminName = admin.username;
                return View(data);
            }
            else
            {
                return RedirectToAction("Index", "Viewer");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(m_viewerViewModel model)
        {
            if (admin != null)
            {
                var data = _viewerService.FilterViewer(model, admin);
                ViewBag.AdminName = admin.username;
                return View(data);
            }
            else
            {
                return RedirectToAction("Index", "Viewer");
            }
        }


        public ActionResult Add()
        {
            if (admin != null)
            {
                m_viewerViewModel model = new m_viewerViewModel();
                ViewBag.AdminName = admin.username;
                return View("Detail", model);
            }
            else
            {
                return RedirectToAction("Index", "Viewer");
            }
           
        }


        public ActionResult Edit(int id)
        {
            if (admin != null)
            {
                var model = _viewerService.GetById(id, admin);
                ViewBag.AdminName = admin.username;
                return View("Detail", model);
            }
            else
            {
                return RedirectToAction("Index", "Viewer");
            }
        }


        public JsonResult Save(m_viewerViewModel model)
        {
            string returnMsg = "";
            if (model!= null)
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
                    if (model.id == 0) //case add
                    {
                        var Chk = _viewerService.ChkDuplicate(model);
                        if (Chk == "false")
                        {
                            _viewerService.Save(model,admin);
                            returnMsg = "success";
                        }
                        else
                        {
                            returnMsg = "Username is already.";
                        }
                    }
                    else //case edit 
                    {
                        _viewerService.Save(model, admin);
                        returnMsg = "success";
                    }
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
            _viewerService.UpdateStatus(id, admin);
            string returnMsg = "success";
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }
    }
}