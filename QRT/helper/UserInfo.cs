using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRT.Domain.ViewModel;

namespace QRT.helper
{
    public class UserInfo
    {
        public static void Set(UserViewModel model)
        {
            HttpContext.Current.Session["USERINFO"] = model;
            model = null;
        }

        public static UserViewModel Get
        {
            get
            {
                return (UserViewModel)HttpContext.Current.Session["USERINFO"];
            }
        }

        public static bool IsOffline
        {
            get
            {
                return HttpContext.Current.Session["USERINFO"] == null;
            }
        }

        public static void Logout()
        {
            HttpContext.Current.Session["USERINFO"] = null;
        }

        public static long ID
        {
            get
            {
                return Get.id;
            }
        }


        public static HttpCookie CreateEmpCookie(EmpData model)
        {
            ExpireEmpCookie();

            HttpCookie EmpCookies = new HttpCookie("EmpCookies");
            EmpCookies.Value = model.code;
            EmpCookies.Expires = DateTime.Now.AddMinutes(90);

           
            return EmpCookies;
        }


        public static HttpCookie ExpireEmpCookie()
        {
            //HttpCookie EmpCookies = new HttpCookie("EmpCookies");
            //EmpCookies.Value = model.code;
            //EmpCookies.Expires = DateTime.Now.AddHours(1);


            HttpCookie EmpCookies = HttpContext.Current.Request.Cookies["EmpCookies"];
            if (EmpCookies != null)
            {
                HttpContext.Current.Response.Cookies.Remove("EmpCookies");
                EmpCookies.Expires = DateTime.Now.AddDays(-10);
                EmpCookies.Value = null;
                HttpContext.Current.Response.SetCookie(EmpCookies);
            }
           

            return EmpCookies;
        }

        //some action method
        // Response.Cookies.Add(CreateStudentCookie());


        public static void SetEmployee(EmpData model)
        {
           





            HttpContext.Current.Session["EMPINFO"] = model;
            model = null;
        }

        public static EmpData GetEmployee
        {
            get
            {
                return (EmpData)HttpContext.Current.Session["EMPINFO"];
            }
        }

        public static void EmployeeLogout()
        {
            HttpContext.Current.Session["EMPINFO"] = null;
        }


    }
}