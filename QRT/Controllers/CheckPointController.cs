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
            var chk = _empservice.CheckEmp(vm.username, vm.password);

            EmpData model = _empservice.CheckEmp(vm.username, vm.password);
            HttpCookie cdata = UserInfo.CreateEmpCookie(model);
            if (chk.code != null)
            {
                Response.Cookies.Add(cdata);
                var id = vm.location_id;

                var chkOverSeq = _location.ChkOverSequentNumber(id, model);
                if (chkOverSeq != null)
                {
                    return RedirectToAction("OverSequent", new { previousSequent = chkOverSeq });
                }
                else
                {
                    return RedirectToAction("QuestionCheck", new { location = vm.location_id });
                }
            }
            else
            {
                return RedirectToAction("Index", new { location = vm.location_id});
            }
        }
        
       

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
                    string code = Request.Cookies["EmpCookies"]["code"];
                    string password = Request.Cookies["EmpCookies"]["password"];
                    EmpData employee = _empservice.CheckEmp(code, password);

                    var chkOverSeq = _location.ChkOverSequentNumber(locationid, employee);
                    if (chkOverSeq != null)
                    {
                        return RedirectToAction("OverSequent", new { previousSequent = chkOverSeq });
                    }
                    else
                    {
                        return RedirectToAction("QuestionCheck", new { location = locationid });
                    }
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
                string code = Request.Cookies["EmpCookies"]["code"];
                string password = Request.Cookies["EmpCookies"]["password"];
                EmpData employee = _empservice.CheckEmp(code, password);

                //var chkLocation = _location.GetLocationByCode(location).adminid_create;
                //if (chkLocation == employee.created_by)
                //{
                    ViewBag.Name = employee.name;
                    ViewBag.LocationName = data[0].location_name;
                    return View(data);
                //}
                //else
                //{
                //    return RedirectToAction("CrossRoute");
                //}
                
            }
            else
            {
                return RedirectToAction("Index", new { location = location });
            }
        }
        

        public JsonResult SaveToAnwser(answer model)
        {
            string returnMsg = "";
            var employeecode = Request.Cookies["EmpCookies"].Value;
            string code = Request.Cookies["EmpCookies"]["code"];
            string password = Request.Cookies["EmpCookies"]["password"];
            EmpData employee = _empservice.CheckEmp(code, password);
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

        public ActionResult SuccessPage(string location)
        {
            var l = Convert.ToInt32(location);
            var locationName = _location.GetLocationById(l);
            return View(locationName);
        }

        public ActionResult OverSequent(string previousSequent)
        {
            ViewBag.previousLocation = previousSequent;
            return View();
        }

        public ActionResult CrossRoute(string crossAlert)
        {
            ViewBag.CrossRoute = "ไม่สามารถสแกนจุดที่อยู่นอกเส้นทางของท่านได้ กรุณาตรวจสอบเส้นทางของท่าน และสแกนใหม่ให้ถูกต้อง";
            return View();
        }
    }
}