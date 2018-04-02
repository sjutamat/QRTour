﻿using System;
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