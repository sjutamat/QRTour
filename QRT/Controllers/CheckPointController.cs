using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRT.Domain.Interface.Service;
using QRT.helper;

namespace QRT.Controllers
{ 
    public class CheckPointController : Controller
    {
        private readonly Imas_empService _empservice;
        private readonly Imas_questionService _questservice;
        private readonly Itrn_answerService _answerservice;

        private EmpData empdata = UserInfo.GetEmployee;
        public CheckPointController(Imas_empService iempService
            ,Imas_questionService iquestService
            ,Itrn_answerService ianswerService)
        {
            _empservice = iempService;
            _questservice = iquestService;
            _answerservice = ianswerService;
        }



        // GET: CheckPoint
        public ActionResult Index(int location)
        {
            ViewBag.LID = location;
            return View();
        }
       
        public ActionResult SingIn(string username, string location)
        {
            var chk = _empservice.CheckEmp(username);
            if (chk != null)
            {
                UserInfo.SetEmployee(chk);
                return RedirectToAction("QuestionCheck", new { id = location });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        public ActionResult Logout()
        {
            UserInfo.EmployeeLogout();
            return Redirect("~");
        }

        public ActionResult QuestionCheck(string id)
        {
            List<QuestionList> data = _questservice.GetQuestionByLocation(Convert.ToInt32(id));
            ViewBag.Name = empdata.name;
            ViewBag.LocationName = data[0].location_name;
            return View(data);
        }

    
        public JsonResult SaveToAnwser(List<Answer> model)
        {
            string returnMsg = "";
            if (model != null)
            {
                _answerservice.SaveAnswer(model, empdata);
                returnMsg = "success";
            }
            else
            {
                returnMsg = "Model is null";
            }
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }
    }
}