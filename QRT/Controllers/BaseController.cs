using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRT.helper;
using QRT.Domain.ViewModel;

namespace QRT.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public UserViewModel admin = UserInfo.Get;
    }
}