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
        private readonly Itrn_hotkeyService _hotkey;

        private EmpData empdata = UserInfo.GetEmployee;
        public CheckPointController(Imas_empService iempService
            ,Imas_questionService iquestService
            ,Itrn_answerService ianswerService
            ,Imas_locationService ilocationService
            ,Itrn_hotkeyService ihotkeyService)
        {
            _empservice = iempService;
            _questservice = iquestService;
            _answerservice = ianswerService;
            _location = ilocationService;
            _hotkey = ihotkeyService;
        }



        // GET: CheckPoint
        public ActionResult Index(string location)
        {
            ViewBag.LID = location;
            var dynamicObject = (dynamic)TempData["ErrorMessage"];
            ViewBag.ErrorMessage = dynamicObject;
            return View();
        }

        [HttpPost]
        public ActionResult SingIn(EmpLogin vm)
        {
            // var chk = _empservice.CheckEmp(vm.username, vm.password);
            HttpContext.Session["SUBMIT_KEY"] = null;
            EmpData model = _empservice.CheckEmp(vm.username, vm.password,vm.location_id);
            HttpCookie cdata = UserInfo.CreateEmpCookie(model, vm.location_id);
            if (model.code != null)
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
                TempData["ErrorMessage"] = "username/password ไม่ถูกต้อง";
                return RedirectToAction("Index", new { location = vm.location_id});
                //ViewBag.ErrorMessage = "username/password ไม่ถูกต้อง";
                //ViewBag.
                //return View("Index", vm.location_id);
            }
        }
        
       

        public ActionResult Logout()
        {
            UserInfo.EmployeeLogout();
            return Redirect("~");
        }


        /// <summary>
        /// Chk Emp Cookies, Chk over sequence
        /// </summary>
        /// <param name="locationid">Is QRCode</param>
        /// <returns></returns>
        public ActionResult Question(string locationid)
        {
            var chkLocation = _location.ChkQRCodeActive(locationid);
            if (chkLocation == true)
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
                        EmpData employee = _empservice.CheckEmp(code, password, locationid);

                        var chkOverSeq = _location.ChkOverSequentNumber(locationid, employee);
                        if (chkOverSeq != null)
                        {
                            //var current_location = _location.GetLocationByCode(locationid).location_title;
                            
                            return RedirectToAction("OverSequent", new { previousSequent = chkOverSeq, currentLocation = locationid });
                        }
                        else
                        {
                            HttpContext.Session["SUBMIT_KEY"] = 0;
                            return RedirectToAction("QuestionCheck", new { location = locationid });
                        }
                    }
                    else  //if not has cookie redirect to Login
                    {
                        return RedirectToAction("Index", new { location = locationid });
                    }
                }
            }
            else
            {
                var AlertMsg = "QRCode นี้ยังไม่เปิดใช้งาน กรุณาติดต่อ admin ของท่าน";
                return RedirectToAction("currentLocation",new { previousSequent  = AlertMsg, currentLocation = locationid});
            }
        }


        public ActionResult QuestionCheck(string location, int pin_id = 0, string pin_remark = "")
        {
            if (HttpContext.Session["SUBMIT_KEY"]==null)
            {
                HttpContext.Session["SUBMIT_KEY"] = 0;
            }
            
            List<QuestionList> data = _questservice.GetQuestionByLocation(location);
            var employeecode = Request.Cookies["EmpCookies"].Value;
            if (employeecode != null && data.Any())
            {
                var aa = HttpContext.Session["SUBMIT_KEY"].ToString();
                string code = Request.Cookies["EmpCookies"]["code"];
                string password = Request.Cookies["EmpCookies"]["password"];
                string enable = Request.Cookies["EmpCookies"]["EnableSave"];
                ViewBag.EnableSave = enable;
                EmpData employee = _empservice.CheckEmp(code, password, location);

                ViewBag.Name = employee.name;
                ViewBag.LocationName = data[0].location_name;
                ViewBag.PIN_ID = pin_id;
                ViewBag.PIN_REMARK = pin_remark;
                return View(data);
                
            }
            else
            {
                HttpContext.Session["SUBMIT_KEY"] = 1;
                return RedirectToAction("Alert");
            }
        }
        

        public JsonResult SaveToAnwser(answer model)
        {
            string returnMsg = "";
            var employeecode = Request.Cookies["EmpCookies"].Value;
            string code = Request.Cookies["EmpCookies"]["code"];
            string password = Request.Cookies["EmpCookies"]["password"];
            string location = Request.Cookies["EmpCookies"]["Location"];
            string enableSave = Request.Cookies["EmpCookies"]["EnableSave"];
            EmpData employee = _empservice.CheckEmp(code, password, location);
            var key = HttpContext.Session["SUBMIT_KEY"].ToString();
            if (model != null)
            {
                if (key == "0")
                {
                    _answerservice.SaveAnswer(model, employee);
                    _hotkey.UpdateHotKey(model.pin_id);
                    HttpContext.Session["SUBMIT_KEY"] = 1;
                    Request.Cookies["EmpCookies"]["EnableSave"] = "0";
                    var aaa = Request.Cookies["EmpCookies"].Value;
                    returnMsg = "success";
                }
                else
                {
                    returnMsg = "disable";
                }
            }
            else
            {
                returnMsg = "Model is null";
            }            
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ChkPINCode(string locationid, string keycode, string remark)
        {
            var chk = _hotkey.chkHotKey(keycode, locationid);
            var returnMsg = "";
            if (chk == true)
            {
                HttpContext.Session["SUBMIT_KEY"] = 0;
                returnMsg = "true";
            }
            else
            {
                returnMsg = "false";
            }
            
            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }

        #region Page Alert
        public ActionResult SuccessPage(string location)
        {
            var l = Convert.ToInt32(location);
            var locationName = _location.GetLocationById(l);
            return View(locationName);
        }

        public ActionResult OverSequent(string previousSequent, string currentLocation = "")
        {
            ViewBag.previousLocation = previousSequent;
            ViewBag.currentLocation = currentLocation;
            return View();
        }

        public ActionResult CrossRoute(string crossAlert)
        {
            ViewBag.CrossRoute = "ไม่สามารถสแกนจุดที่อยู่นอกเส้นทางของท่านได้ กรุณาตรวจสอบเส้นทางของท่าน และสแกนใหม่ให้ถูกต้อง";
            return View();
        }


        /// <summary>
        /// not have any question for the location.
        /// </summary>
        /// <returns></returns>
        public ActionResult Alert()
        {
            ViewBag.CrossRoute = "ไม่พบคำถาม โปรดติดต่อ admin ของท่าน";
            return View("CrossRoute");
        }
        #endregion




    }
}