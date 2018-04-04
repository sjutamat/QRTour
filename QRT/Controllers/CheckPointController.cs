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
        private readonly Imas_locationService _location;

        private EmpData empdata = UserInfo.GetEmployee;
        public CheckPointController(Imas_empService iempService
            ,Imas_questionService iquestService
            ,Itrn_answerService ianswerService
            ,Imas_locationService ilocationService)
        {
            _empservice = iempService;
            _questservice = iquestService;
            _answerservice = ianswerService;
            _location = ilocationService;
        }



        // GET: CheckPoint
        public ActionResult Index(string location)
        {
            ViewBag.LID = location;
            return View();
        }

        [HttpPost]
        public ActionResult SingIn(EmpLogin vm)
        {
            var chk = _empservice.CheckEmp(vm.username);

            EmpData model = _empservice.CheckEmp(vm.username);
            HttpCookie cdata = UserInfo.CreateEmpCookie(model);
            if (chk != null)
            {
                
                Response.Cookies.Add(cdata);
                //UserInfo.SetEmployee(chk);
                var id = vm.location_id;
                return RedirectToAction("QuestionCheck", new { location = vm.location_id });
                //return Json(id, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Index");
                //return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        
        //public ActionResult SingIn(string username, string location)
        //{
        //    var chk = _empservice.CheckEmp(username);

        //    EmpData model = _empservice.CheckEmp(username);
        //    HttpCookie cdata = UserInfo.CreateEmpCookie(model);
        //    if (chk != null)
        //    {
        //        Response.Cookies.Add(cdata);
        //        //UserInfo.SetEmployee(chk);
        //        var id = location;
        //        return RedirectToAction("QuestionCheck", new { id = location });
        //        //return Json(id, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //        //return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //}


        public ActionResult Logout()
        {
            UserInfo.EmployeeLogout();
            return Redirect("~");
        }



        public ActionResult Question(string locationid)
        {
            var chkSeq = _location.ChkSequentNumber(locationid);
            if (chkSeq == true) //chk node 1
            {
                if (Request.Cookies["EmpCookies"] != null) //if has cookie, clear cookie and redirect to Login.
                {
                    UserInfo.ExpireEmpCookie();
                    return RedirectToAction("Index", new { location = locationid });
                }
                else //if not have cookie, redirect to Login.
                {
                    return RedirectToAction("Index", new { location = locationid });
                }
                //chk cookie
            }
            else
            { //chk cookie
                if (Request.Cookies["EmpCookies"] != null)  //if has cookie return view.
                {
                    return RedirectToAction("QuestionCheck", new { location = locationid });
                }
                else  //if not has cookie redirect to Login
                {
                    return RedirectToAction("Index", new { location = locationid });
                }
            }
        }


        public ActionResult QuestionCheck(string location)
        {
            List<QuestionList> data = _questservice.GetQuestionByLocation(location);
            var employeecode = Request.Cookies["EmpCookies"].Value;
            if (employeecode != null && data.Any())
            {
                EmpData employee = _empservice.CheckEmp(employeecode);

                ViewBag.Name = employee.name;
                ViewBag.LocationName = data[0].location_name;
                return View(data);
            }
            else
            {
                return RedirectToAction("Index", new { location = location });
            }
        }

    
        public JsonResult SaveToAnwser(List<Answer> model)
        {
            string returnMsg = "";
            var employeecode = Request.Cookies["EmpCookies"].Value;
            EmpData employee = _empservice.CheckEmp(employeecode);
            if (model != null)
            {
                _answerservice.SaveAnswer(model, employee);
                returnMsg = "success";
            }
            else
            {
                returnMsg = "Model is null";
            }
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SuccessPage()
        {
            return View();
        }
    }
}